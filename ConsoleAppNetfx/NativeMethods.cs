using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
	internal static class NativeMethods
	{
		static NativeMethods()
		{
			// 名前が違うので何もしなくてよい
		}
		public static int GetMaxValue( int left, int right )
		{
			Trace.WriteLine( $"Called GetMaxValue()" );
			int result = 0;
			switch( RuntimeInformation.ProcessArchitecture )
			{
			case Architecture.X86:
				Trace.WriteLine( $"Calling GetMaxValue32( {left}, {right} )" );
				result = GetMaxValue32( left, right );
				Trace.WriteLine( $"Return GetMaxValue32( {left}, {right} ):{result}" );
				break;
			case Architecture.X64:
				Trace.WriteLine( $"Calling GetMaxValue64( {left}, {right} )" );
				result = GetMaxValue64( left, right );
				Trace.WriteLine( $"Return GetMaxValue64( {left}, {right} ):{result}" );
				break;
			default:
				throw new PlatformNotSupportedException( $"Unknown ProcessArchitecture({RuntimeInformation.ProcessArchitecture})" );
			}
			return result;
		}
		[DllImport( "CppDll32.dll", CallingConvention=CallingConvention.Winapi, EntryPoint="GetMaxValue" )]
		private extern static int GetMaxValue32( int left, int right );

		[DllImport( "CppDll64.dll", CallingConvention = CallingConvention.Winapi, EntryPoint="GetMaxValue" )]
		private extern static int GetMaxValue64( int left, int right );
	}
}

