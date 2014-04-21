#ifndef ___InjectTools_h__
#define ___InjectTools_h__

#ifndef STRICT
#define STRICT
#endif
#define WIN32_LEAN_AND_MEAN
#include <windows.h>

#ifdef __cplusplus
extern "C" {
#endif

    DWORD InjectRemoteThread(HANDLE hProcess, LPTHREAD_START_ROUTINE callback, DWORD callbackSize, LPVOID data, DWORD dataSize, DWORD* returnCode);

    DWORD InjectDll(HANDLE hProcess, WCHAR* dllPath);

    DWORD InjectDllAndCall(HANDLE hProcess, WCHAR* dllPath, DWORD callbackOffset);

    DWORD InjectSelf(HANDLE hProcess, LPTHREAD_START_ROUTINE callback);

    DWORD InjectDetour(HANDLE hProcess, LPVOID detoured, LPVOID* original);

    HANDLE OpenProcessForInject(DWORD processId);

#ifdef __cplusplus
}
#endif

#endif // ___InjectTools_h__
