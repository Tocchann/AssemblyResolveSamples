// Native C++ ‚ÌŠÖ”‚ğŒÄ‚Ño‚·
#include "pch.h"

#ifdef _M_X64
EXTERN_C __declspec(dllexport) int APIENTRY GetMaxValue64( int left, int right )
#else
EXTERN_C __declspec(dllexport) int APIENTRY GetMaxValue32( int left, int right )
#endif
{
#if _M_X64
	const wchar_t* msg = L"Called GetMaxValue64()\n";
#else
	const wchar_t* msg = L"Called GetMaxValue32()\n";
#endif
	OutputDebugString( msg );
	std::wcout << msg;

	return left > right ? left : right;
}

