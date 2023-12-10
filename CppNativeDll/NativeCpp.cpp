// Native C++ の関数を呼び出す
#include "pch.h"

EXTERN_C int APIENTRY GetMinValue(int left, int right)
{
	const wchar_t* msg = L"Called GetMinValue()\n";
	OutputDebugString( msg );
	std::wcout << msg;
	return left < right ? left : right;
}

