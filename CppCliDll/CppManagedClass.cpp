#include "pch.h"
#include "CppManagedClass.h"
using namespace System;

namespace CppCliDll
{
CppManagedClass::CppManagedClass()
{
	Console::WriteLine( L"CppCliDll::CppManagedClass::CppManagedClass()" );
}
CppManagedClass::~CppManagedClass()
{
	Console::WriteLine( L"CppCliDll::CppManagedClass::~CppManagedClass()" );
}
int CppManagedClass::Add( int left, int right )
{
	Console::WriteLine( String::Format( L"CppCliDll::CppManagedClass::Add( {0}, {1} )", left, right ) );
	return left + right;
}
}
