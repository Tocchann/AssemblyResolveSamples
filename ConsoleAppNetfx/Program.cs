using CppCliDll;
using System;
using DotNetLab.Utility;
using System.Diagnostics;

namespace ConsoleApp
{
	internal class Program
	{
		static void Main( string[] args )
		{
			Trace.WriteLine( "Main()" );
			Console.WriteLine( "Main()" );
			using( var loader = new AssemblyResolveLoader() )
			{
				Trace.WriteLine( "Before CallTest()" );
				Console.WriteLine( "Before CallTest()" );
				CallTest();
			}
		}

		private static void CallTest()
		{
			Trace.WriteLine( "CallTest()" );
			Console.WriteLine( "CallTest()" );
			var cppClass = new CppManagedClass();
			var result = cppClass.Add( 1, 2 );
			Trace.WriteLine( $"CppManagedClass.Add( 1, 2 ) = {result}" );
			Console.WriteLine( $"CppManagedClass.Add( 1, 2 ) = {result}" );
		}
	}
}
