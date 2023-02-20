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
			Debug.WriteLine( "Main()" );
			Console.WriteLine( "Main()" );
			using( var loader = new AssemblyResolveLoader() )
			{
				CallTest();
			}
		}

		private static void CallTest()
		{
			Debug.WriteLine( "CallTest()" );
			Console.WriteLine( "CallTest()" );
			var cppClass = new CppManagedClass();
			var result = cppClass.Add( 1, 2 );
			Debug.WriteLine( $"CppManagedClass.Add( 1, 2 ) = {result}" );
			Console.WriteLine( $"CppManagedClass.Add( 1, 2 ) = {result}" );
		}
	}
}
