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

		// static コンストラクタで実行してしまえば、P/Invoke と同様(解放できなくなるが)
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
			throw new DllNotFoundException( moduleName );
		}
		var proc = GetProcAddress( hModule, "GetMaxValue" );
		if( proc == IntPtr.Zero )
		{
			throw new EntryPointNotFoundException( "GetMaxValue" );
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
