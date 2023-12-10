// Native C++ ‚ÌŠÖ”‚ğŒÄ‚Ño‚·
#include "pch.h"

EXTERN_C int APIENTRY GetMaxValue( int left, int right )
{
	const wchar_t* msg = L"Called GetMaxValue()\n";
	OutputDebugString( msg );
	std::wcout << msg;

	return left > right ? left : right;
}
