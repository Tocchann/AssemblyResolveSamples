using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace DotNetLab.Utility;

public static class NativeMethods
{
	public static Func<int, int, int> GetMaxValue { get; }
	static NativeMethods()
	{
		Trace.WriteLine( "in NativeMethods.NativeMethods()" );
		// 事前に解決してしまうことで、GetMaxValue を直接的に扱えるようにする
		GetMaxValue = RuntimeInformation.ProcessArchitecture switch
		{
			Architecture.X86 => GetMaxValue32,
			Architecture.X64 => GetMaxValue64,
			_ => throw new PlatformNotSupportedException( $"{RuntimeInformation.ProcessArchitecture} is Not Supported." ),
		};
		Trace.WriteLine( "out NativeMethods.NativeMethods()" );
	}
	[DllImport( "CppDll32.dll", CallingConvention=CallingConvention.Winapi, EntryPoint="GetMaxValue" )]
	private extern static int GetMaxValue32( int left, int right );

	[DllImport( "CppDll64.dll", CallingConvention = CallingConvention.Winapi, EntryPoint="GetMaxValue" )]
	private extern static int GetMaxValue64( int left, int right );
}

