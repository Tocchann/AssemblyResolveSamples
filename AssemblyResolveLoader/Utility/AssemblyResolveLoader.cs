using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DotNetLab.Utility
{
	/// <summary>
	/// AssemblyResolveLoader
	/// AppDomain.AssemblyResolve にハンドラを登録して自動的に環境依存モジュールをロードする。
	/// ロード対象は、.NET アセンブリである必要はあるが、C++/CLI プロジェクトである必要はない。
	/// 利用時の注意事項
	/// AssemblyName.Name の名前のDLLをロード対象としている(この実装ではリネームしない)
	/// 切り分けは、System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture を参照する
	/// X86 の場合は x86、X64 の場合は x64, Arm64の場合は ARM64
	/// </summary>
	public class AssemblyResolveLoader : IDisposable
	{
		public AssemblyResolveLoader()
		{
			Trace.WriteLine( $"AssemblyResolveLoader()" );
			Console.WriteLine( $"AssemblyResolveLoader()" );

			Trace.WriteLine( $"AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;" );
			Console.WriteLine( $"AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;" );
			AppDomain.CurrentDomain.AssemblyResolve += PreCall_AssemblyResolve;
		}

		private bool disposedValue;
		protected virtual void Dispose( bool disposing )
		{
			Trace.WriteLine( $"AssemblyResolveLoader.Dispose( {disposing} )" );
			Console.WriteLine( $"AssemblyResolveLoader.Dispose( {disposing} )" );
			if( !disposedValue )
			{
				if( disposing )
				{
					Trace.WriteLine( $"AppDomain.CurrentDomain.AssemblyResolve -= AssemblyResolve;" );
					Console.WriteLine( $"AppDomain.CurrentDomain.AssemblyResolve -= AssemblyResolve;" );
					AppDomain.CurrentDomain.AssemblyResolve -= PreCall_AssemblyResolve;
				}
				disposedValue = true;
			}
		}
		public void Dispose()
		{
			Dispose( disposing: true );
			GC.SuppressFinalize( this );
		}
		// ステップ実行した時にわかりやすくロードのタイミングを見せるために用意したダミーハンドラ
		private Assembly PreCall_AssemblyResolve( object sender, ResolveEventArgs args )
		{
			//'ConsoleAppCore.exe'( CoreCLR: clrhost ): 'System.Runtime.InteropServices.RuntimeInformation.dll' が読み込まれました。
			return AssemblyResolve( sender, args );
		}
		private Assembly AssemblyResolve( object sender, ResolveEventArgs args )
		{
			Trace.WriteLine( $"AssemblyResolve( args.Name={args.Name} )" );
			Console.WriteLine( $"AssemblyResolve( args.Name={args.Name} )" );
			var assemblyName = new AssemblyName( args.Name );
			Trace.WriteLine( $"RuntimeInformation.OSArchitecture={RuntimeInformation.OSArchitecture}" );
			Console.WriteLine( $"RuntimeInformation.OSArchitecture={RuntimeInformation.OSArchitecture}" );
			Trace.WriteLine( $"RuntimeInformation.ProcessArchitecture={RuntimeInformation.ProcessArchitecture}" );
			Console.WriteLine( $"RuntimeInformation.ProcessArchitecture={RuntimeInformation.ProcessArchitecture}" );
			// 位置は決め打ち。ファイル名はアセンブリ名と同じという前提にしている
			var appendDir = RuntimeInformation.ProcessArchitecture switch
			{
				Architecture.X86 => "x86",
				Architecture.X64 => "x64",
				Architecture.Arm64 => "ARM64",
				Architecture.Arm => "ARM",
				_ => throw new PlatformNotSupportedException( $"Unknown ProcessArchitecture({RuntimeInformation.ProcessArchitecture})" ),
			};
			// 相対パス参照でよい
			var targetPath = Path.Combine( appendDir, assemblyName.Name + ".dll" );
			Trace.WriteLine( $"Assembly.LoadFrom( {targetPath} )" );
			Console.WriteLine( $"Assembly.LoadFrom( {targetPath} )" );
			var assembly = Assembly.LoadFrom( targetPath );
			return assembly;
		}
	}
}
