// Native C++ の関数を呼び出す
#include "pch.h"

EXTERN_C int APIENTRY GetMinValue(int left, int right)
{
	return left < right ? left : right;
}

