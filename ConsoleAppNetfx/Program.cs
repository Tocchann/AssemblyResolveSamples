﻿//#define ENABLE_ASSEMBLY_RESOLVE
using CppCliDll;
using CsDll;
using DotNetLab.Utility;
using System.Diagnostics;
using System.IO;

namespace ConsoleApp
{
	internal class Program
	{
		static void Main( string[] args )
		{
			Trace.Listeners.Add( new ConsoleTraceListener() );
			Trace.WriteLine( "Main()" );

			Trace.WriteLine( Directory.GetCurrentDirectory() );

#if ENABLE_ASSEMBLY_RESOLVE
			using( var loader = new AssemblyResolveLoader() )
			{
				Trace.WriteLine( "Before CallTest()" );
				CallTest();

				Trace.WriteLine( "Second time CallTest()" );
				CallTest();

				Trace.WriteLine( "Before CallTestCs()" );
				CallTestCs();
			}
#endif // ENABLE_ASSEMBLY_RESOLVE
			DynamicLoad.CallTestDynamicLoad();

			CallTestCppDll();

			ConsoleAppCore.CoreOnly.CoreOnlyMethod();

			DynamicLoad.CallTestDynamicLoad();
		}

		private static void CallTestCppDll()
		{
			Trace.WriteLine( "inside CallTestCppDll()" );
			var result = NativeMethods.GetMaxValue( 10, 20 );
			Trace.WriteLine( $"NativeMethods.GetMaxValue( 10, 20 )={result}" );
		}

		private static void CallTest()
		{
			Trace.WriteLine( "CallTest()" );
			var cppClass = new CppManagedClass();
			var result = cppClass.Add( 1, 2 );
			Trace.WriteLine( $"CppManagedClass.Add( 1, 2 ) = {result}" );
		}
		private static void CallTestCs()
		{
			Trace.WriteLine( "CallTestCs()" );
			var csClass = new CsClass();
			Trace.WriteLine( $"CsClass.Name={csClass.Name}" );
			Trace.WriteLine( $"CsClass.IntPtrSize={csClass.IntPtrSize}" );
		}
		
	}
}
