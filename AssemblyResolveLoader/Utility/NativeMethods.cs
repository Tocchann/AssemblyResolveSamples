using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DotNetLab.Utility;

public static class NativeMethods
{
	static NativeMethods()
	{
		// 名前が違うので何もしなくてよい
	}
	public static int GetMaxValue( int left, int right )
	{
		Trace.WriteLine( $"Called GetMaxValue()" );
		var name = RuntimeInformation.ProcessArchitecture switch
		{
			Architecture.X86 => "GetMaxValue32",
			Architecture.X64 => "GetMaxValue64",
			_ => throw new PlatformNotSupportedException( $"{RuntimeInformation.ProcessArchitecture} is Not Supported." ),
		};
		Trace.WriteLine( $"Calling {name}( {left}, {right} )" );
		int result = RuntimeInformation.ProcessArchitecture switch
		{
			Architecture.X86 => GetMaxValue32( left, right ),
			Architecture.X64 => GetMaxValue64( left, right ),
			_ => throw new PlatformNotSupportedException( $"{RuntimeInformation.ProcessArchitecture} is Not Supported." ),
		};
		Trace.WriteLine( $"Return {name}( {left}, {right} ):{result}" );
		return result;
	}
	[DllImport( "CppDll32.dll", CallingConvention=CallingConvention.Winapi, EntryPoint="GetMaxValue" )]
	private extern static int GetMaxValue32( int left, int right );

	[DllImport( "CppDll64.dll", CallingConvention = CallingConvention.Winapi, EntryPoint="GetMaxValue" )]
	private extern static int GetMaxValue64( int left, int right );
}

