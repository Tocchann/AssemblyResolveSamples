//#define USE_LOAD_LIBRARY
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
#if NET
			NativeLibrary.SetDllImportResolver( Assembly.GetExecutingAssembly(), DllImportResolver );
#else
			AddPlatformDependDirectory();
#endif

			Trace.WriteLine( "Before CallNativeCpp()" );
			CallNativeCpp();
		}


		private static void CallNativeCpp()
		{
			Trace.WriteLine( "CallNativeCpp()" );

			var minValue = GetMinValue( 10, 20 );
			Trace.WriteLine( $"GetMinValue(10,20)={minValue}" );
		}
		[DllImport( "CppNativeDll.dll" )]
		static extern int GetMinValue( int left, int right );


#if NET
		private static IntPtr DllImportResolver( string libraryName, Assembly assembly, DllImportSearchPath? searchPath )
		{
			Trace.WriteLine( $"DllImportResolver( libraryName={libraryName}, assembly=[{assembly}], searchPath={searchPath} )" );
			var appendDir = RuntimeInformation.ProcessArchitecture switch
			{
				Architecture.X86 => "x86",
				Architecture.X64 => "x64",
				Architecture.Arm64 => "ARM64",
				Architecture.Arm => "ARM",
				_ => throw new PlatformNotSupportedException( $"{RuntimeInformation.ProcessArchitecture} is Not Supported." ),
			};
			var targetPath = Path.Combine( appendDir, libraryName );
			Trace.WriteLine( $"NativeLibrary.Load( {targetPath} )" );
			return NativeLibrary.Load( targetPath, assembly, searchPath );
		}
#else
		private static void AddPlatformDependDirectory()
		{
			var appendDir = string.Empty;
			switch( RuntimeInformation.ProcessArchitecture )
			{
			case Architecture.X86: appendDir = "x86"; break;
			case Architecture.X64: appendDir = "x64"; break;
			}
			appendDir = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, appendDir );
			Trace.WriteLine( appendDir );
#if USE_LOAD_LIBRARY
			var dllPath = Path.Combine( appendDir, "CppNativeDll.dll" );
			if( LoadLibrary( dllPath ) == IntPtr.Zero )
			{
				// LoadLibrary の失敗はそのままLastErrorを投げる
				var lastError = Marshal.GetLastWin32Error();
				// 本当はラストエラーによって例外を変える必要があるけどここではやらない
				new Exception( $"Fail LoadLibrary( {dllPath} ):LastError={lastError}" );
			}
#else
			SetDllDirectory( appendDir );
#endif

		}
#if USE_LOAD_LIBRARY
		[DllImport( "kernel32", SetLastError = true, CharSet = CharSet.Auto )]
		private static extern IntPtr LoadLibrary( string lpFileName );
#else
		[DllImport( "kernel32", SetLastError = true, CharSet = CharSet.Auto )]
		private static extern bool SetDllDirectory( string lpPathName );
#endif

		//[DllImport( "kernel32", SetLastError = true )]
		//private static extern bool FreeLibrary( IntPtr hLibModule );
#endif
	}
}
