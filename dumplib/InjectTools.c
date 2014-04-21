#include "InjectTools.h"

#pragma comment(lib, __FILE__"/../LDE64.lib")
// imported from LDE:
int __stdcall LDE(void*, int);

// Define a structure for the remote thread to use
typedef struct {
    // called APIs
    HMODULE(WINAPI *fnLoadLibraryW) (LPCWSTR lpLibFileName);
    BOOL(WINAPI *fnFreeLibrary) (HMODULE hMod);
    // path to injected dll
    WCHAR libName[MAX_PATH];
    // offset of the callback inside dll, or 0
    DWORD callbackOffset;
} LoadLib_data;

// we need to disable all runtime checks for injected functions
#pragma runtime_checks("", off)

// force sequential layout so we can calculate code size
#pragma code_seg(push)
#pragma code_seg("sequential$1")

// Thread to inject to remote process.
// Must not make ANY calls to code in THIS process.
static DWORD WINAPI LoadLib(void* pParam)
{
    LoadLib_data* data = (LoadLib_data*) pParam;
    HMODULE hMod = data->fnLoadLibraryW(data->libName);
    if (!data->callbackOffset) {
        // just load the library and return handle
        return (DWORD) hMod;
    }
    PTHREAD_START_ROUTINE callback = (void*) ((DWORD) hMod + data->callbackOffset);
    DWORD result = callback(NULL);
    // cleanup
    data->fnFreeLibrary(hMod);
    return result;
}

#pragma code_seg("sequential$2")

static void AfterLoadLib(void) { }

#pragma code_seg(pop)

#pragma runtime_checks("", restore)

#define JMP_SIZE 5

static PBYTE GenJmp(PBYTE pbCode, PBYTE pbJmpDst)
{
    *pbCode = 0xE9;
    *(UINT32*) (pbCode + 1) = pbJmpDst - (pbCode + JMP_SIZE);
    return pbCode + JMP_SIZE;
}

static int MoveCode(PBYTE dst, PBYTE src, int len)
{
    int instrLen, pos = 0;
    for (; pos < len; pos += instrLen) {
        instrLen = LDE(src + pos, 0);
        if (instrLen == -1) {
            MessageBoxW(0, L"MoveCode: instrLen == -1", L"ERROR", 0);
            break;
        }

        MoveMemory(dst + pos, src + pos, instrLen);

        // fixup relative calls and jumps
        switch (dst[pos]) {
        case 0xE8 /* CALL, 4bytes operand */:
        case 0xE9 /* JMP, 4bytes operand */:
        {
            *((UINT32*) &dst[pos + 1]) += src - dst;
            break;
        }
        }
    }
    return pos;
}

#define ALLOC_SIZE 4096

static PBYTE UseScratchpad(int size)
{
    static PBYTE scratchpad = NULL;
    static int scratchpadFree = 0;
    PBYTE result;

    if (scratchpadFree < size) {
        // allocate in 4kB blocks
        scratchpad = (PBYTE) VirtualAlloc(0, ALLOC_SIZE, MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);
        scratchpadFree = ALLOC_SIZE;
    }

    result = &scratchpad[ALLOC_SIZE - scratchpadFree];
    scratchpadFree -= size;
    return result;
}

///////////////////////////////////////////////////////////////////////////////////////
//
// Public API implementation
//

DWORD InjectRemoteThread(HANDLE hProcess, LPTHREAD_START_ROUTINE callback, DWORD callbackSize, LPVOID data, DWORD dataSize, DWORD* returnCode)
{
    // based on InjectThread.c work by J Brown (2002)
    HANDLE hRemoteThread = 0; //handle to the injected thread
    DWORD  dwRemoteThreadId = 0; //ID of the injected thread

    DWORD dwWritten = 0; // Number of bytes written to the remote process
    DWORD dwRead = 0;
    DWORD dwExitCode;

    void* pRemoteData = 0;

    // The address to which code will be copied in the remote process
    DWORD* pdwRemoteCode;

    // Total size of all memory copied into remote process
    const int cbMemSize = callbackSize + dataSize + 3;

    pdwRemoteCode = VirtualAllocEx(hProcess, 0, cbMemSize, MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);
    if (pdwRemoteCode == 0)
        return FALSE;

    // Write a copy of our injection thread into the remote process
    WriteProcessMemory(hProcess, pdwRemoteCode, callback, callbackSize, &dwWritten);

    pRemoteData = (void *) ((BYTE *) pdwRemoteCode + ((callbackSize + 4) & ~3));

    // Put DATA in the remote thread's memory block
    WriteProcessMemory(hProcess, pRemoteData, data, dataSize, &dwWritten);

    // Create the remote thread
    hRemoteThread = CreateRemoteThread(hProcess, NULL, 0, (LPTHREAD_START_ROUTINE) pdwRemoteCode, pRemoteData, 0, &dwRemoteThreadId);

    // Wait for the thread to terminate
    WaitForSingleObject(hRemoteThread, INFINITE);

    // Read the user-structure back again
    if (!ReadProcessMemory(hProcess, pRemoteData, data, dataSize, &dwRead))
        return FALSE;

    GetExitCodeThread(hRemoteThread, &dwExitCode);
    if (returnCode)
        *returnCode = dwExitCode;

    CloseHandle(hRemoteThread);

    // Free the memory in the remote process
    VirtualFreeEx(hProcess, pdwRemoteCode, 0, MEM_RELEASE);

    return TRUE;
}

DWORD InjectDll(HANDLE hProcess, WCHAR* dllPath)
{
    return InjectDllAndCall(hProcess, dllPath, 0);
}

DWORD InjectDllAndCall(HANDLE hProcess, WCHAR* dllPath, DWORD callbackOffset)
{
    LoadLib_data data;
    (FARPROC) data.fnLoadLibraryW = GetProcAddress(GetModuleHandleW(L"kernel32"), "LoadLibraryW");
    (FARPROC) data.fnFreeLibrary = GetProcAddress(GetModuleHandleW(L"kernel32"), "FreeLibrary");
    MoveMemory(data.libName, dllPath, MAX_PATH * sizeof(WCHAR));
    data.callbackOffset = callbackOffset;
    return InjectRemoteThread(hProcess, LoadLib, (DWORD) AfterLoadLib - (DWORD) LoadLib, &data, sizeof(data), NULL);
}

DWORD InjectSelf(HANDLE hProcess, LPTHREAD_START_ROUTINE callback)
{
    HMODULE hMod;
    WCHAR dllPath[MAX_PATH];
    GetModuleHandleExW(GET_MODULE_HANDLE_EX_FLAG_FROM_ADDRESS | GET_MODULE_HANDLE_EX_FLAG_UNCHANGED_REFCOUNT, (LPCWSTR) callback, &hMod);
    GetModuleFileNameW(hMod, dllPath, MAX_PATH);
    return InjectDllAndCall(hProcess, dllPath, (DWORD) callback - (DWORD) hMod);
}

DWORD InjectDetour(LPVOID detour, LPVOID detoured, LPVOID* original)
{
    const int maxDetourSize = 64; // just in case
    int movedSz;
    DWORD oldProtect, ignored; // saved memprotect
    HANDLE hProcess = GetCurrentProcess(); // used by VirtualProtect
    PBYTE moved = UseScratchpad(maxDetourSize);

    // allow us to write on this page
    if (!FlushInstructionCache(hProcess, detoured, maxDetourSize))
        return 1;
    if (!VirtualProtect(detoured, maxDetourSize, PAGE_EXECUTE_READWRITE, &oldProtect))
        return 2;

    // move prologue to the scratchpad
    movedSz = MoveCode(moved, detoured, JMP_SIZE);
    // emit jump back
    GenJmp(moved + movedSz, (PBYTE) detoured + movedSz);

    // emit jump to detour
    GenJmp((PBYTE) detoured, detour);

    // restore page protection back
    if (!FlushInstructionCache(hProcess, detoured, maxDetourSize))
        return 3;
    if (!VirtualProtect(detoured, maxDetourSize, oldProtect, &ignored))
        return 4;

    *original = (LPVOID) moved;
    return 0;
}

HANDLE OpenProcessForInject(DWORD processId)
{
    return OpenProcess(PROCESS_CREATE_THREAD | PROCESS_QUERY_INFORMATION | PROCESS_VM_OPERATION | PROCESS_VM_READ | PROCESS_VM_WRITE,
        FALSE, processId);
}
