// dllmain.cpp : DLL アプリケーションのエントリ ポイントを定義します。
#include "pch.h"

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    const wchar_t* pszReasonName=nullptr;
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
        pszReasonName = L"CppDll.dll Called DllMain( DLL_PROCESS_ATTACH )\n";
        break;
    case DLL_THREAD_ATTACH:
        pszReasonName = L"CppDll.dll Called DllMain( DLL_THREAD_ATTACH )\n";
        break;
    case DLL_THREAD_DETACH:
        pszReasonName = L"CppDll.dll Called DllMain( DLL_THREAD_DETACH )\n";
        break;
    case DLL_PROCESS_DETACH:
        pszReasonName = L"CppDll.dll Called DllMain( DLL_PROCESS_DETACH )\n";
        break;
    }
    OutputDebugString( pszReasonName );
    std::wcout << pszReasonName;
    return TRUE;
}

