# セッションタイトル

今更聞けない。
プラットフォーム依存アセンブリを動的にロードするには？
AssembyResolve 編

## アジェンダ
- はじめに
- 今回のプロジェクト構成
- .NET アプリがアセンブリを読み込むタイミング
- .NET で動的にアセンブリを読み込む簡単な方法
- 動作中のモードに沿ったプラットフォームの選択方法
- ビルド時に適切なバイナリを適切な場所に配置する方法
- exe のビルドでアセンブリをコピーするには
- おわりに

## はじめに

ARM版 Windows アプリが作成できるようになって、プロセスアーキテクチャの判定が単純ではなくなりました。  
Windows の世界だけでも、x86, x64 のほかに、ARM32, ARM64 と実に４種類もの選択肢があります。  
これらを動的に判定しつつ、プロセスアーキテクチャ向けにビルドしたDLLをロードするにはどうすればいいのか？というところをかいつまんでお話したいと思います。

### 今回のプロジェクト構成

| Project Name | Language | Type | Target Framework | Platform |
|---|---|---|---|---|
| ConsoleAppCore | C# | exe | .NET 6.0 | Any CPU |
| CppCliDllCore | C++/CLI | dll | .NET 6.0 | x86, x64, ARM32, ARM64 |
| CsDllCore | C# | dll | .NET 6.0 | x86, x64, ARM32, ARM64 |
| ConsoleAppNetfx | C# | exe | .NET Framework 4.8.1 | Any CPU |
| CppCliDll | C++/CLI | dll | .NET Framework 4.8.1 | x86, x64, ARM64 |
| CsDll | C# | dll | .NET Framework 4.8.1 | x86, x64, ARM64 |
| AssemblyResolveLoader | C# | dll | .NET Standard 2.0 | Any CPU |

## .NET アプリがアセンブリを読み込むタイミング

- .NET アプリは、そのメソッドが呼ばれるタイミングではじめてロードされる
- 実際に見てみましょう！

## .NET で動的にアセンブリを読み込む簡単な方法

- アセンブリを動的に選択するにはどうすればいいか？
    - デフォルト処理では解決出来ないようにする
- 具体的には？
1. ロードするアセンブリをexeと同じ場所に置かない
    - AppDomain.AssemblyResolve イベントを発生させる
1. 実行時に判定できる手段を用意する

## 動作中のモードに沿ったプラットフォームの選択方法

適切なプラットフォームを選択するにはどうするか？

1. IntPtr.Size で判定する
    - 32bitプロセスか、64bitプロセスかしかわからない
    - ARM バイナリが出力できる今の時代では不十分
    - .NET Framework でも AnyCPU にした場合 ARM64 で動作してしまう
1. RuntimeInformation.ProcessArchitecture を参照
    - ARM(32/64)も含めた判定が可能
    - 実際に稼働中のOSの判定は RuntimeInformation.OSArchitecture

## ビルド時に適切なバイナリを適切な場所に配置する方法

1. プロジェクトの出力先をコピーしやすい場所にする
    - C++/CLI  
    $(SolutionDir)bin\$(Configuration)\$(PlatformShortName)\
    $(SolutionDir)bin\$(Configuration)Core\$(PlatformShortName)\  
    - .NET Framework(こちらは実際はマクロは利用しない)
    bin\$(Configuration)\$(Platform)\ 
1. DLLのPostBuildEvent で、コピーしやすい場所にコピーしておく
    - .NET Core 版で採用  
    copy $(TargetPath) bin\$(Configuration)Core\$(Platform)\ でコピー

## exe のビルドでアセンブリをコピーするには？

1. PostBuildEvent を利用する(バッチファイル処理)
    - 特定フォルダ以下を XCOPY などを利用してコピーする
1. MSbuild のカスタム target を作成
    - Copy Task を利用してコピー(内容的にはバッチ処理と同じ)

## おわりに

この手法、SQLite.NET など、いくつかの有名ライブラリでも使われています。  
ほかにも、DllImport で呼び出すAPIを動的に選択したり、LoadLibrary API を C# から使うことで動的に切り替えたりと様々な方法があります。
機会があればほかの方法も紹介したいと思います。
