using CppCliDll;
using DotNetLab.Utility;
using System.Diagnostics;

Debug.WriteLine( "Main()" );
Console.WriteLine( "Main()" );
using( var loader = new AssemblyResolveLoader() )
{
	CallTest();
}
static void CallTest()
{
	Debug.WriteLine( "CallTest()" );
	Console.WriteLine( "CallTest()" );
	var cppClass = new CppManagedClass();
	var result = cppClass.Add( 1, 2 );
	Debug.WriteLine( $"CppManagedClass.Add( 1, 2 ) = {result}" );
	Console.WriteLine( $"CppManagedClass.Add( 1, 2 ) = {result}" );
}
