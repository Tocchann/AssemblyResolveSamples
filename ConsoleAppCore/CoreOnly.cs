using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCore
{
	internal class CoreOnly
	{
		public static void CoreOnlyMethod()
		{
			NativeLibrary.SetDllImportResolver( Assembly.GetExecutingAssembly(), DllImportResolver );

			Trace.WriteLine( "Before CallNativeCpp()" );
			Console.WriteLine( "Before CallNativeCpp()" );
			CallNativeCpp();
		}
		private static void CallNativeCpp()
		{
			Trace.WriteLine( "CallNativeCpp()" );
			Console.WriteLine( "CallNativeCpp()" );

			var minValue = GetMinValue( 10, 20 );
			Trace.WriteLine( $"GetMinValue(10,20)={minValue}" );
			Console.WriteLine( $"GetMinValue(10,20)={minValue}" );
		}
		private static IntPtr DllImportResolver( string libraryName, Assembly assembly, DllImportSearchPath? searchPath )
		{
			Trace.WriteLine( $"DllImportResolver( libraryName={libraryName}, assembly=[{assembly}], searchPath={searchPath} )" );
			Console.WriteLine( $"DllImportResolver( libraryName={libraryName}, assembly=[{assembly}], searchPath={searchPath} )" );
			var appendDir = RuntimeInformation.ProcessArchitecture switch
			{
				Architecture.X86 => "x86",
				Architecture.X64 => "x64",
				Architecture.Arm64 => "ARM64",
				Architecture.Arm => "ARM",
				_ => throw new PlatformNotSupportedException( $"Unknown ProcessArchitecture({RuntimeInformation.ProcessArchitecture})" ),
			};
			var targetPath = Path.Combine( appendDir, libraryName );
			Trace.WriteLine( $"NativeLibrary.Load( {targetPath} )" );
			Console.WriteLine( $"NativeLibrary.Load( {targetPath} )" );
			return NativeLibrary.Load( targetPath, assembly, searchPath );
		}
		[DllImport( "CppNativeDll.dll" )]
		static extern int GetMinValue( int left, int right );
	}
}
