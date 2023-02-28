using CppCliDll;
using CsDll;
using DotNetLab.Utility;
using System;
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

				Trace.WriteLine( "Before CallTestCs()" );
				Console.WriteLine( "Before CallTestCs()" );
				CallTestCs();
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
		private static void CallTestCs()
		{
			Trace.WriteLine( "CallTestCs()" );
			Console.WriteLine( "CallTestCs()" );
			var csClass = new CsClass();
			Trace.WriteLine( $"CsClass.Name={csClass.Name}" );
			Console.WriteLine( $"CsClass.Name={csClass.Name}" );
			Trace.WriteLine( $"CsClass.IntPtrSize={csClass.IntPtrSize}" );
			Console.WriteLine( $"CsClass.IntPtrSize={csClass.IntPtrSize}" );
		}
	}
}
