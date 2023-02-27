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
