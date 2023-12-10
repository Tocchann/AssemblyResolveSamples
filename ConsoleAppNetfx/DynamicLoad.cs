using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
	internal class DynamicLoad
	{
		public static void CallTestDynamicLoad()
		{
			Trace.WriteLine( "CallTestDynamicLoad" );
			Trace.WriteLine( "Before GetMinValue()" );
			int result = GetMinValue( 10, 20 );
			Trace.WriteLine( $"After GetMinValue( 10, 20 ):{result}" );
		}

		public static int GetMinValue( int left, int right )
		{
			var appendDir = string.Empty;
			switch( RuntimeInformation.ProcessArchitecture )
			{
			case Architecture.X86:	appendDir = "x86";	break;
			case Architecture.X64:	appendDir = "x64";	break;
			default:
				throw new PlatformNotSupportedException( $"Unknown ProcessArchitecture({RuntimeInformation.ProcessArchitecture})" );
			}
			int result = 0;
			var targetPath = Path.Combine( appendDir, "CppNativeDll.dll" );
			var hModule = LoadLibrary( targetPath );
			if( hModule != IntPtr.Zero )
			{
				var proc = GetProcAddress( hModule, "GetMinValue" );
				if( proc != IntPtr.Zero )
				{
					var getMinValue = Marshal.GetDelegateForFunctionPointer<GetMinValueDelegate>( proc );
					if( getMinValue != null )
					{
						result = getMinValue( left, right );
					}
				}
				// 毎度アンロードする
				FreeLibrary( hModule );
			}
			return result;
		}
		private delegate int GetMinValueDelegate( int left, int right );

		[DllImport( "kernel32", SetLastError = true, CharSet = CharSet.Auto )]
		private static extern IntPtr LoadLibrary( string lpFileName );

		[DllImport( "kernel32", SetLastError = true, CharSet=CharSet.Ansi)]
		private static extern IntPtr GetProcAddress( IntPtr hModule, string lpFunction );
		
		[DllImport("kernel32", SetLastError =true, CharSet = CharSet.Auto)]
		private static extern bool FreeLibrary( IntPtr hLibModule );
	}
}
