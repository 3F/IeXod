# [IeXod](https://github.com/3F/IeXod)

The most portable alternative to *Microsoft.Build* for evaluating, manipulating, and other progressive data processing in a compatible XML-like syntax.

```r
Copyright (c) .NET Foundation and contributors
Copyright (c) 2020-2024  Denis Kuzmin <x-3F@outlook.com> github/3F
Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
```

[ 「 ❤ 」 ](https://3F.github.io/fund) [![License](https://img.shields.io/badge/License-MIT-74A5C2.svg)](https://github.com/3F/IeXod/blob/master/LICENSE)
[![Build status](https://ci.appveyor.com/api/projects/status/mclqcptonbch6jjv/branch/master?svg=true)](https://ci.appveyor.com/project/3Fs/iexod/branch/master)
[![release](https://img.shields.io/github/v/release/3F/IeXod?include_prereleases&sort=semver)](https://github.com/3F/IeXod/releases/latest)
[![NuGet IeXod](https://img.shields.io/nuget/v/IeXod.svg)](https://www.nuget.org/packages/IeXod/)


## Why IeXod

https://github.com/3F/MvsSln/issues/23

### 🔍 Easy to use

*Microsoft.Build* with its typical error [[?]](https://github.com/3F/MvsSln/wiki/Advanced-Features#about--possible--problems)

```csharp
// 'The SDK 'Microsoft.NET.Sdk' specified could not be found.
new Project("<path to Sdk-style project file>");
```

**[IeXod](https://github.com/3F/IeXod)** 👇

```csharp
new Project("<path to Sdk-style project file>"); // Microsoft.NET.Sdk -> 
/* ~
+ Imports  Count = 30
+ AllEvaluatedItemDefinitionMetadata  Count = 21
+ AllEvaluatedItems  Count = 108
+ AllEvaluatedProperties  Count = 367
...
*/
```

### 🔧 Configurable Sdk resolvers at runtime

```csharp
new Project("...", properties, ProjectToolsOptions.Default);

new Project("...", properties, new ProjectToolsOptions(new[] { 
    @"path_to_\sys\resolvers\", 
    @"path2\",
    ...
}));

ProjectToolsOptions.Default.SdkResolvers = new SdkResolver[] { 
    new SysResolver(), 
    new ProdResolver(),
    ...
};
```

### 🧦🎯 Automatic searching of the modern Toolsets

VS setup **API** + Registry + [hMSBuild.bat](https://3F.github.io/hMSBuild/releases/latest/) + Configuration files + ...;

That continues direction of https://github.com/3F/hMSBuild

### 🔨 Exposing Toolsets in classical notation

```
> {[Current, ToolsPath=...\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\amd64]}
...
{[14.0, ToolsPath=C:\Program Files (x86)\MSBuild\14.0\bin\amd64]}
{[15.0, ToolsPath=...\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\amd64]}
{[16.0, ToolsPath=...\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\amd64]}
>> {[17.0, ToolsPath=...\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\amd64]}
```

### 🧰 Portability, Compatibility, and Functionality

> Modern #MSBuild assemblies are much more closely integrated with #VisualStudio and much more difficult to maintain independently [[?]](https://twitter.com/GitHub3F/status/1184170248532119552)

This is most important goal for [IeXod](https://github.com/3F/IeXod) project. To help to avoid the following nightmare: 

❌ From difficulty use (\~ [Microsoft.Build.Locator](https://www.nuget.org/packages/Microsoft.Build.Locator/)) to unpredictable behavior in various products (\~ Visual Studio etc) due to active integration inside a single environment with *Microsoft.Build.*

### 🎈 Something More

in progress ...

Follow the news;

* https://github.com/3F
* https://github.com/3F/IeXod

Contribute;

Enjoy!

## IeXod and custom Sdk Resolvers

IeXod provides independent interface for the easiest implementation of any new Sdk Resolvers.

[![NuGet IeXod.SdkResolver](https://img.shields.io/nuget/v/IeXod.SdkResolver.svg)](https://www.nuget.org/packages/IeXod.SdkResolver/)

Extend evaluation as you need; Then, easily register and configure new resolvers on the fly!

## Where is used

( 📅 Planned at least after first stable IeXod release )

* [MvsSln](https://github.com/3F/MvsSln)
* [E-MSBuild](https://github.com/3F/E-MSBuild)
* [.NET DllExport](https://github.com/3F/DllExport)
* [SobaScript](https://github.com/3F/SobaScript)
* [vsSolutionBuildEvent](https://github.com/3F/vsSolutionBuildEvent)
* ...