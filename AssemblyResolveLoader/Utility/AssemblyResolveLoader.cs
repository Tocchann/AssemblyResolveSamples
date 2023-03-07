using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DotNetLab.Utility
{
	/// <summary>
	/// AssemblyResolveLoader
	/// プラットフォームアーキテクチャ依存アセンブリを動的にロードするための AssemblyResolve ハンドラ管理クラス
	/// ロード対象は、.NET アセンブリである必要はあるが、C++/CLI プロジェクトである必要はない。
	/// イベントハンドラ型なので、IDisposable を持つ。
	/// アプリケーションのメイン関数の最初でusing して利用する
	/// </summary>
	public class AssemblyResolveLoader : IDisposable
	{
		/// <summary>
		/// プロセスアーキテクチャに依存したモジュールを格納しているディレクトリ
		/// </summary>
		private string ArchitectureDependDirectory { get; }
		public AssemblyResolveLoader(/*IHostEnvironment env*/)
		{
			Trace.WriteLine( $"AssemblyResolveLoader()" );
			Console.WriteLine( $"AssemblyResolveLoader()" );

			Trace.WriteLine( $"RuntimeInformation.FrameworkDescription={RuntimeInformation.FrameworkDescription}" );
			Console.WriteLine( $"RuntimeInformation.FrameworkDescription={RuntimeInformation.FrameworkDescription}" );
			Trace.WriteLine( $"RuntimeInformation.OSArchitecture={RuntimeInformation.OSArchitecture}" );
			Console.WriteLine( $"RuntimeInformation.OSArchitecture={RuntimeInformation.OSArchitecture}" );
			Trace.WriteLine( $"RuntimeInformation.OSDescription={RuntimeInformation.OSDescription}" );
			Console.WriteLine( $"RuntimeInformation.OSDescription={RuntimeInformation.OSDescription}" );
			Trace.WriteLine( $"RuntimeInformation.ProcessArchitecture={RuntimeInformation.ProcessArchitecture}" );
			Console.WriteLine( $"RuntimeInformation.ProcessArchitecture={RuntimeInformation.ProcessArchitecture}" );

			// 実行中にプロセスアーキテクチャが変わることはない
			var appendDir = RuntimeInformation.ProcessArchitecture switch
			{
				Architecture.X86 => "x86",
				Architecture.X64 => "x64",
				Architecture.Arm64 => "ARM64",
				Architecture.Arm => "ARM",
				_ => throw new PlatformNotSupportedException( $"Unknown ProcessArchitecture({RuntimeInformation.ProcessArchitecture})" ),
			};
			// 本当ならこの手の解決策としては、IHostEnvironment などを使って解決するのが一番安定する(環境依存部分は全部吸収してくれるため)
			Trace.WriteLine( $"Path.GetDirectoryName( Assembly.GetEntryAssembly()?.Location )={Path.GetDirectoryName( Assembly.GetEntryAssembly()?.Location )}" );
			Console.WriteLine( $"Path.GetDirectoryName( Assembly.GetEntryAssembly()?.Location )={Path.GetDirectoryName( Assembly.GetEntryAssembly()?.Location )}" );
			Trace.WriteLine( $"Path.GetDirectoryName( Assembly.GetExecutingAssembly()?.Location )={Path.GetDirectoryName( Assembly.GetExecutingAssembly()?.Location )}" );
			Console.WriteLine( $"Path.GetDirectoryName( Assembly.GetExecutingAssembly()?.Location )={Path.GetDirectoryName( Assembly.GetExecutingAssembly()?.Location )}" );
			Trace.WriteLine( $"AppDomain.CurrentDomain.BaseDirectory={AppDomain.CurrentDomain.BaseDirectory}" );
			Console.WriteLine( $"AppDomain.CurrentDomain.BaseDirectory={AppDomain.CurrentDomain.BaseDirectory}" );
			// GetEntryAssembly() は、COM.DLL のような場合はnullを返すため、汎用的なライブラリとしては使えない
			var baseDir = Path.GetDirectoryName( Assembly.GetEntryAssembly()?.Location );
			if( string.IsNullOrEmpty( baseDir ) )
			{
				baseDir = Path.GetDirectoryName( Assembly.GetExecutingAssembly()?.Location );
			}
			ArchitectureDependDirectory = Path.Combine( baseDir, appendDir );

			Trace.WriteLine( $"ArchitectureDependDirectory={ArchitectureDependDirectory}" );
			Console.WriteLine( $"ArchitectureDependDirectory={ArchitectureDependDirectory}" );
			AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
		}

		private Assembly AssemblyResolve( object sender, ResolveEventArgs args )
		{
			var appDomain = sender as AppDomain;
			Trace.WriteLine( $"AssemblyResolve( sender={appDomain?.FriendlyName}, args={args.Name} )" );
			Console.WriteLine( $"AssemblyResolve( sender={appDomain?.FriendlyName}, args={args.Name} )" );
			var assemblyName = new AssemblyName( args.Name );
			var targetPath = Path.Combine( ArchitectureDependDirectory, assemblyName.Name + ".dll" );
			Trace.WriteLine( $"Assembly.LoadFrom( {targetPath} )" );
			Console.WriteLine( $"Assembly.LoadFrom( {targetPath} )" );
			var assembly = Assembly.LoadFrom( targetPath );
			return assembly;
		}
		#region Dispose() の実装
		private bool disposedValue;
		protected virtual void Dispose( bool disposing )
		{
			Trace.WriteLine( $"AssemblyResolveLoader.Dispose( {disposing} )" );
			Console.WriteLine( $"AssemblyResolveLoader.Dispose( {disposing} )" );
			if( !disposedValue )
			{
				if( disposing )
				{
					AppDomain.CurrentDomain.AssemblyResolve -= AssemblyResolve;
				}
				disposedValue = true;
			}
		}
		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}
		#endregion
	}
}
