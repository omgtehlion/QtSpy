#ifndef ___InjectTools_h__
#define ___InjectTools_h__

#define STRICT
#define WIN32_LEAN_AND_MEAN
#include <windows.h>

#ifdef __cplusplus
extern "C" {
#endif

    DWORD InjectRemoteThread(DWORD processId, LPTHREAD_START_ROUTINE callback, DWORD callbackSize, LPVOID data, DWORD dataSize, DWORD* returnCode);

    DWORD InjectDll(DWORD processId, WCHAR* dllPath);

    DWORD InjectDllAndCall(DWORD processId, WCHAR* dllPath, DWORD callbackOffset);

    DWORD InjectSelf(DWORD processId, LPTHREAD_START_ROUTINE callback);

    DWORD InjectDetour(LPVOID detour, LPVOID detoured, LPVOID* original);

#ifdef __cplusplus
}
#endif

#endif // ___InjectTools_h__
