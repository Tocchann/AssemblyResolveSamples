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
			Debug.WriteLine( $"AssemblyResolveLoader()" );
			Console.WriteLine( $"AssemblyResolveLoader()" );

			Debug.WriteLine( $"AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;" );
			Console.WriteLine( $"AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;" );
			AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
		}
		private bool disposedValue;
		protected virtual void Dispose( bool disposing )
		{
			Debug.WriteLine( $"AssemblyResolveLoader.Dispose( {disposing} )" );
			Console.WriteLine( $"AssemblyResolveLoader.Dispose( {disposing} )" );
			if( !disposedValue )
			{
				if( disposing )
				{
					Debug.WriteLine( $"AppDomain.CurrentDomain.AssemblyResolve -= AssemblyResolve;" );
					Console.WriteLine( $"AppDomain.CurrentDomain.AssemblyResolve -= AssemblyResolve;" );
					AppDomain.CurrentDomain.AssemblyResolve -= AssemblyResolve;
				}
				disposedValue = true;
			}
		}
		public void Dispose()
		{
			Dispose( disposing: true );
			GC.SuppressFinalize( this );
		}
#if NET
		private System.Reflection.Assembly? AssemblyResolve( object? sender, ResolveEventArgs args )
#else
		private System.Reflection.Assembly AssemblyResolve( object sender, ResolveEventArgs args )
#endif
		{
			Debug.WriteLine( $"AssemblyResolve( args.Name={args.Name} )" );
			Console.WriteLine( $"AssemblyResolve( args.Name={args.Name} )" );
			// あり得ないはずではあるが、AppDomain の呼び出し以外は反応しないようにする
			if( sender is AppDomain appDomain )
			{
				var assemblyName = new AssemblyName( args.Name );
				Console.WriteLine( $"RuntimeInformation.OSArchitecture={RuntimeInformation.OSArchitecture}" );
				Console.WriteLine( $"RuntimeInformation.ProcessArchitecture={RuntimeInformation.ProcessArchitecture}" );
				// 位置は決め打ち。ファイル名はアセンブリ名と同じという前提にしている(名前を変えるとかはやらない)
#if NET
				// .NET Core の場合(将来増える可能性がある)は、不明なアーキテクチャは例外をだして開発時に解決できるようにする。
				// OSは考慮していないのでその点は注意が必要。
				var appendDir = RuntimeInformation.ProcessArchitecture switch
				{
					Architecture.X86 => "x86",
					Architecture.X64 => "x64",
					Architecture.Arm64 => "ARM64",
					Architecture.Arm => "ARM",
					_ => throw new PlatformNotSupportedException( $"Unknown ProcessArchitecture({RuntimeInformation.ProcessArchitecture})" ),
				};
#else
				// .NET Framework の場合(今後減ることはあっても増えることはない)は、デフォルトはx86環境とみなして、x64, ARM64 を場合分けする。
				var appendDir = "x86";
				switch( RuntimeInformation.ProcessArchitecture )
				{
					case Architecture.X64:
						appendDir = "x64";
						break;
					case Architecture.Arm64:
						appendDir = "ARM64";
						break;
				}
#endif
				var targetPath = Path.Combine( appendDir, assemblyName.Name + ".dll" );
				Debug.WriteLine( $"Assembly.LoadFrom( {targetPath} )" );
				Console.WriteLine( $"Assembly.LoadFrom( {targetPath} )" );
				var assembly = Assembly.LoadFrom( targetPath ); // これで見つからなければ、エラー
				return assembly;
			}
			return null;
		}
	}
}
