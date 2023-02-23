//'ConsoleAppCore.exe'( CoreCLR: DefaultDomain ): 'System.Private.CoreLib.dll' が読み込まれました。
//'ConsoleAppCore.exe'( CoreCLR: clrhost ): 'bin\Release\net6.0\ConsoleAppCore.dll' が読み込まれました。
//'ConsoleAppCore.exe'( CoreCLR: clrhost ): 'System.Runtime.dll' が読み込まれました。
//'ConsoleAppCore.exe'( CoreCLR: clrhost ): 'bin\Release\net6.0\AssemblyResolveLoader.dll' が読み込まれました。
//'ConsoleAppCore.exe'( CoreCLR: clrhost ): 'netstandard.dll' が読み込まれました。
//'ConsoleAppCore.exe'( CoreCLR: clrhost ): 'System.Diagnostics.TraceSource.dll' が読み込まれました。
//'ConsoleAppCore.exe'( CoreCLR: clrhost ): 'System.Console.dll' が読み込まれました。
using CppCliDll;
using DotNetLab.Utility;
using System.Diagnostics;

Trace.WriteLine( "Main()" );
Console.WriteLine( "Main()" );
//'ConsoleAppCore.exe'( CoreCLR: clrhost ): 'System.Threading.dll' が読み込まれました。
//'ConsoleAppCore.exe'( CoreCLR: clrhost ): 'System.Text.Encoding.Extensions.dll' が読み込まれました。
using( var loader = new AssemblyResolveLoader() )
{
	Trace.WriteLine( "Before CallTest()" );
	Console.WriteLine( "Before CallTest()" );
	CallTest();
}
static void CallTest()
{
	Trace.WriteLine( "CallTest()" );
	Console.WriteLine( "CallTest()" );
	var cppClass = new CppManagedClass();
	var result = cppClass.Add( 1, 2 );
	Trace.WriteLine( $"CppManagedClass.Add( 1, 2 ) = {result}" );
	Console.WriteLine( $"CppManagedClass.Add( 1, 2 ) = {result}" );
}
