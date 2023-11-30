using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsDll
{
	public class CsClass
	{
		public CsClass()
		{
			Trace.WriteLine( $"CsClass.CsClass():AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName={AppDomain.CurrentDomain?.SetupInformation?.TargetFrameworkName}, IntPtr.Size={IntPtr.Size}" );
		}
		public string Name 
		{
			get => typeof( CsClass ).Name;
		}
		public int IntPtrSize
		{
			get => IntPtr.Size;
		}
	}
}
