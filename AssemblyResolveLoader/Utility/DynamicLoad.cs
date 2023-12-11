using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DotNetLab.Utility;

public class DynamicLoad
{
	public static void CallTestDynamicLoad()
	{
		Trace.WriteLine( "CallTestDynamicLoad" );
		Trace.WriteLine( "Before GetMinValue()" );
		int result = GetMaxValue( 10, 20 );
		Trace.WriteLine( $"After GetMinValue( 10, 20 ):{result}" );
	}

	public static int GetMaxValue( int left, int right )
	{
		GetMaxValueDelegate? getMaxValue = default;

		// 呼び出しごとにロードするのではなく事前に解決しておくほうが効率的に動かせる
		//////
		var moduleName = RuntimeInformation.ProcessArchitecture switch
		{
			Architecture.X86 => "CppDll32.dll",
			Architecture.X64 => "CppDll64.dll",
			_ => throw new PlatformNotSupportedException( $"{RuntimeInformation.ProcessArchitecture} is Not Supported." ),
		};

		var hModule = LoadLibrary( moduleName );
		if( hModule == IntPtr.Zero )
		{
			switch( Marshal.GetLastWin32Error() )
			{
			case 193: // == ERROR_BAD_EXE_FORMAT
				throw new BadImageFormatException( $"ModuleName={moduleName}, Win32Error={Marshal.GetLastWin32Error()}" );
			//case 126: // == ERROR_MOD_NOT_FOUND
			default:
				throw new DllNotFoundException( $"ModuleName={moduleName}, Win32Error={Marshal.GetLastWin32Error()}" );
			}
		}
		var proc = GetProcAddress( hModule, "GetMaxValue" );
		if( proc == IntPtr.Zero )
		{
			throw new EntryPointNotFoundException( $"EntryName=GetMaxValue, Win32Error={Marshal.GetLastWin32Error()}" );
		}
		getMaxValue = Marshal.GetDelegateForFunctionPointer<GetMaxValueDelegate>( proc );
		//////
		var result = getMaxValue( left, right );


		FreeLibrary( hModule );
		return result;
	}
	private delegate int GetMaxValueDelegate( int left, int right );

	[DllImport( "kernel32", SetLastError = true, CharSet = CharSet.Auto )]
	private static extern IntPtr LoadLibrary( string lpFileName );

	[DllImport( "kernel32", SetLastError = true, CharSet=CharSet.Ansi)]
	private static extern IntPtr GetProcAddress( IntPtr hModule, string lpFunction );
	
	[DllImport("kernel32", SetLastError =true, CharSet = CharSet.Auto)]
	private static extern bool FreeLibrary( IntPtr hLibModule );
}
