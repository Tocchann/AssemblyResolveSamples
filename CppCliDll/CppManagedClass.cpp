#include "pch.h"
#include "CppManagedClass.h"
using namespace System;
using namespace System::Diagnostics;

namespace CppCliDll
{
CppManagedClass::CppManagedClass()
{
	Trace::WriteLine( L"CppCliDll::CppManagedClass::CppManagedClass()" );
	Console::WriteLine( L"CppCliDll::CppManagedClass::CppManagedClass()" );
}
CppManagedClass::~CppManagedClass()
{
	Trace::WriteLine( L"CppCliDll::CppManagedClass::~CppManagedClass()" );
	Console::WriteLine( L"CppCliDll::CppManagedClass::~CppManagedClass()" );
}
int CppManagedClass::Add( int left, int right )
{
	auto value = String::Format( L"CppCliDll::CppManagedClass::Add( {0}, {1} )", left, right );
	Trace::WriteLine( value );
	Console::WriteLine( value );
	return left + right;
}
}
