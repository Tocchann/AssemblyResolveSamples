# AssemblyResolveSamples

AppDomain.AssemblyResolve イベントのハンドリング用サンプルソリューション

.NET Framework 向け構成と .NET Core 向け構成で作られているがソースコード的には同じ(ソースコードそのものはすべて共有)

## プロジェクトについて

| ProjectName | Language | Type | Framework | Version | Any CPU | ARM | ARM64 | Win32 | x64 |
|---|---|---|---|---|---|---|---|---|---|
| AssemblyResolveLoader | C# | dll | .NET Standard | 2.0 | ○ | × |× |× |× |
| ConsoleAppCore | C# | exe | .NET | 6 | ○ | × |× |× |× |
| ConsoleAppNetfx | C# | exe | .NET Framework | 4.8.1 | ○ | × |× |× |× |
| CppCliDll | C++/CLI | dll | .NET Framework | 4.8.1 | × | × | ○ | ○ | ○ | 
| CppCliDllCore | C++/CLI | dll | .NET | 6 | × | ○ | ○ | ○ | ○ | 

## ソリューションについて

| Solution Name | Detail |
|---|---|
| AssemblyResolveSamples.sln | 全部入りソリューション |
| AssemblyResolveSamplesCore.sln | .NET Core 版で利用するプロジェクト |
| AssemblyResolveSamplesDLLs.sln | .NET Framework 版で利用するプロジェクト
| AssemblyResolveSamplesNetfx.sln |　C++/CLI DLLのみ(事前バッチビルド用) |

## 開発環境について

Visual Studio 2022 で以下のワークロードと個別コンポーネントが必要です。

- ワークロード
	- .NET デスクトップ開発
	- C++ によるデスクトップ開発
- 個別コンポーネント
	- .NET Framework 4.8.1 SDK
	- .NET Framework 4.8.1 ターゲティング パック
	- MSVC v143 - VS 2022 C++ ARM ビルドツール(最新)
	- MSVC v143 - VS 2022 C++ ARM64/ARM64EC ビルドツール(最新)

もしかしたらほかにも必要かもしれません(いろいろ入ってるので絞れていない)

## ビルド手順

1. AssemblyResolveSamplesDLLs.sln を開いて、バッチビルドですべての構成をビルドする  
エラーが出るプロジェクトがある場合、ビルドに必要なツール類が入っていないことになるので、適宜インストールする
1. AssemblyResolveSamplesCore.sln または AssemblyResolveSamplesNetfx.sln を開いてビルドする。

## 動的処理の判定

AssemblyResolveLoader 内で完結する形で用意されています。
