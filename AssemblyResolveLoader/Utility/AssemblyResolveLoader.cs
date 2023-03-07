using System;
using System.Collections.Generic;
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
			// 理想的には IHostEnvironment.ContentRootPath を使うのが良い
			foreach( var dependDir in EnumBaseDirectories( appendDir ) )
			{
				if( Directory.Exists( dependDir ) )
				{
					ArchitectureDependDirectory = dependDir;
					break;
				}
			}
			if( ArchitectureDependDirectory == null )
			{
				throw new DirectoryNotFoundException( "アセンブリロードパスが見つかりません。" );
			}
			Trace.WriteLine( $"ArchitectureDependDirectory={ArchitectureDependDirectory}" );
			Console.WriteLine( $"ArchitectureDependDirectory={ArchitectureDependDirectory}" );
			AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
		}
		private static IEnumerable<string> EnumBaseDirectories( string appendDir )
		{
			Trace.WriteLine( $"Path.GetDirectoryName( Assembly.GetEntryAssembly()?.Location )={Path.GetDirectoryName( Assembly.GetEntryAssembly()?.Location )}" );
			Console.WriteLine( $"Path.GetDirectoryName( Assembly.GetEntryAssembly()?.Location )={Path.GetDirectoryName( Assembly.GetEntryAssembly()?.Location )}" );
			var dir = Path.GetDirectoryName( Assembly.GetEntryAssembly()?.Location );
			if( !string.IsNullOrEmpty( dir ) )
			{
				yield return Path.Combine( dir, appendDir );
			}
			Trace.WriteLine( $"Path.GetDirectoryName( Assembly.GetExecutingAssembly()?.Location )={Path.GetDirectoryName( Assembly.GetExecutingAssembly()?.Location )}" );
			Console.WriteLine( $"Path.GetDirectoryName( Assembly.GetExecutingAssembly()?.Location )={Path.GetDirectoryName( Assembly.GetExecutingAssembly()?.Location )}" );
			dir = Path.GetDirectoryName( Assembly.GetExecutingAssembly()?.Location );
			if( !string.IsNullOrEmpty( dir ) )
			{
				yield return Path.Combine( dir, appendDir );
			}
			Trace.WriteLine( $"AppDomain.CurrentDomain.BaseDirectory={AppDomain.CurrentDomain.BaseDirectory}" );
			Console.WriteLine( $"AppDomain.CurrentDomain.BaseDirectory={AppDomain.CurrentDomain.BaseDirectory}" );
			yield return Path.Combine( AppDomain.CurrentDomain.BaseDirectory, appendDir );
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
