// dllmain.cpp : Defines the entry point for the DLL application.
#include "Tools.h"
#include "InjectTools.h"
#include "Dumper.h"
#include "PipeServer.h"

extern "C" {
    static DWORD WINAPI Injected(void* param)
    {
        // increment refcount so this DLL would not be unloaded right away
        HMODULE hMod;
        ::GetModuleHandleExW(GET_MODULE_HANDLE_EX_FLAG_FROM_ADDRESS, reinterpret_cast<LPCWSTR>(&::Injected), &hMod);

        ObjectRepo::Init();
        pipeServer::Start();
        return 1;
    }

    __declspec(dllexport) DWORD WINAPI InjectInto(DWORD pid)
    {
        auto hProcess = ::OpenProcessForInject(pid);
        auto result = ::InjectSelf(hProcess, Injected, nullptr, 0);
        if (hProcess)
            ::CloseHandle(hProcess);
        return result;
    }

    BOOL APIENTRY DllMain(HMODULE hModule, DWORD  dwReason, LPVOID lpReserved)
    {
        switch (dwReason) {
        case DLL_PROCESS_ATTACH:
        case DLL_THREAD_ATTACH:
        case DLL_THREAD_DETACH:
        case DLL_PROCESS_DETACH:
            break;
        }
        return TRUE;
    }
}
