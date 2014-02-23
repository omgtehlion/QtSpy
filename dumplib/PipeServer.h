namespace pipeServer {

    DWORD WINAPI ClientThread(void* param)
    {
        auto hPipe = (HANDLE) param;
        uint8_t buff[BUFSIZE];
        int buffCount = 0;

        // Do some extra error checking since the app will keep running even if this
        // thread fails.
        if (!hPipe) {
            ::MessageBoxW(0, L"InstanceThread got an unexpected NULL value in param.", L"Pipe Server Failure", 0);
            return -1;
        }

        // Loop until done reading
        while (1) {
            if (buffCount >= BUFSIZE)
                break; // buffer overrun
            // Read client requests from the pipe. This simplistic code only allows messages
            // up to BUFSIZE characters in length.
            DWORD read = 0;
            auto fSuccess = ::ReadFile(hPipe, &buff[buffCount], BUFSIZE - buffCount, &read, NULL);

            if (!fSuccess || read == 0) {
                //if (::GetLastError() == ERROR_BROKEN_PIPE) {
                //    //_tprintf(TEXT("InstanceThread: client disconnected.\n"), GetLastError());
                //} else {
                //    //_tprintf(TEXT("InstanceThread ReadFile failed, GLE=%d.\n"), GetLastError());
                //}
                break;
            }

            buffCount += read;

            while (buffCount) {
                auto newLine = static_cast<uint8_t*> (::memchr(buff, '\n', buffCount));
                if (!newLine)
                    break;
                auto len = newLine - buff; // excluding \n
                Dumper().ProcessCmd(buff, len, hPipe);
                buffCount -= len + 1;
                ::memmove_s(buff, BUFSIZE, &buff[len + 1], buffCount);
            }
        }

        // Flush the pipe to allow the client to read the pipe's contents 
        // before disconnecting. Then disconnect the pipe, and close the 
        // handle to this pipe instance. 
        ::FlushFileBuffers(hPipe);
        ::DisconnectNamedPipe(hPipe);
        ::CloseHandle(hPipe);
        return 0;
    }

    DWORD WINAPI ServerThread(void* param)
    {
        wchar_t pipename[256];
        swprintf_s(pipename, L"\\\\.\\pipe\\QtSpy_%i", ::GetCurrentProcessId());

        while (1) {
            auto hPipe = ::CreateNamedPipe(
                pipename,
                PIPE_ACCESS_DUPLEX,
                PIPE_TYPE_BYTE | PIPE_READMODE_BYTE | PIPE_WAIT,
                PIPE_UNLIMITED_INSTANCES,
                BUFSIZE,
                BUFSIZE,
                0,
                NULL);

            if (hPipe == INVALID_HANDLE_VALUE) {
                ::MessageBoxA(0, "CreateNamedPipe failed", "PipeServer", 0);
                return -1;
            }

            // Wait for the client to connect; if it succeeds, 
            // the function returns a nonzero value. If the function
            // returns zero, GetLastError returns ERROR_PIPE_CONNECTED. 

            auto fConnected = ::ConnectNamedPipe(hPipe, NULL) ? TRUE : (::GetLastError() == ERROR_PIPE_CONNECTED);

            if (fConnected) {
                // Create a thread for this client. 
                DWORD dwThreadId;
                ::CreateThread(NULL, 0, ClientThread, hPipe, 0, &dwThreadId);
            } else {
                // The client could not connect, so close the pipe. 
                ::CloseHandle(hPipe);
            }
        }
        return 0;
    }

    void Start()
    {
        DWORD dwThreadId;
        ::CreateThread(NULL, 0, ServerThread, NULL, 0, &dwThreadId);
    }

} // namespace pipeServer
