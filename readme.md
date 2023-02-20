# AssemblyResolveSamples

AppDomain.AssemblyResolve イベントのハンドリング用サンプルソリューション

.NET Framework, .NET Core のいずれでも利用可能(サンプル構成はC++/CLIを利用しているためWindows用のみ)

## プロジェクトについて

- ConsoleAppCore: .NET Core のコンソールアプリ(Any CPU)
- ConsoleAppNetfx: .NET Framework のコンソールアプリ(Any CPU)
- AssemblyResolveLoader: .NET Standard 2.0 のクラスライブラリ(Any CPU)
- CppCliDll: C++/CLI のクラスライブラリ、.NET Core, .NET Framework ごとに ARM(.NET Coreのみ)、ARM64、x86、x64 の構成を用意

## ソリューションについて

- AssemblyResolveSamples.sln: 全てのプロジェクトが含まれているソリューション
- AssemblyResolveSamplesCore.sln: .NET Core 用で利用されるプロジェクトで構成されたソリューション
- AssemblyResolveSamplesNetfx.sln: .NET Framework 用で利用されるプロジェクトで構成されたソリューション
- AssemblyResolveSamplesDLLs.sln: バッチビルド用にC++/CLIだけを含むソリューション
