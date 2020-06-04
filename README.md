# [IeXod](https://github.com/3F/IeXod)

```
Copyright (c) .NET Foundation and contributors
Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
```

🧰 The most portable alternative to Microsoft.Build for evaluating, manipulating, and other progressive data processing in a compatible XML-like syntax.

[![Build status](https://ci.appveyor.com/api/projects/status/mclqcptonbch6jjv/branch/master?svg=true)](https://ci.appveyor.com/project/3Fs/iexod/branch/master)
[![release](https://img.shields.io/github/release/3F/IeXod.svg)](https://github.com/3F/IeXod/releases/latest)
[![License](https://img.shields.io/badge/License-MIT-74A5C2.svg)](https://github.com/3F/IeXod/blob/master/LICENSE)
[![NuGet package](https://img.shields.io/nuget/v/IeXod.svg)](https://www.nuget.org/packages/IeXod/)

[![Build history](https://buildstats.info/appveyor/chart/3Fs/iexod?buildCount=20&includeBuildsFromPullRequest=true&showStats=true)](https://ci.appveyor.com/project/3Fs/iexod/history)

# License

IeXod is licensed under the [MIT license](LICENSE).

[ [ ☕ Donate ](https://3F.github.com/Donation/) ]

# Why IeXod ?

https://github.com/3F/MvsSln/issues/23

## 🔍 Easy to use:

MSBuild with its typical error [[?]](https://github.com/3F/MvsSln/wiki/Advanced-Features#about--possible--problems)

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

## 🔧 Configurable Sdk resolvers at runtime:

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

## 🧦🎯 Automatic searching of the modern Toolsets

VS setup API + Registry + Configuration files;

That continues direction of https://github.com/3F/hMSBuild

## 🔨 Exposing Toolsets in classical notation:

```csharp
> {[Current, ToolsPath=C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\amd64]}
{[14.0, ToolsPath=C:\Program Files (x86)\MSBuild\14.0\bin\amd64]}
{[15.0, ToolsPath=C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\amd64]}
>> {[16.0, ToolsPath=C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\amd64]}
```

## 🧰 Portability, Compatibility, and Functionality

> Modern #MSBuild assemblies are much more closely integrated with #VisualStudio and much more difficult to maintain independently [[?]](https://twitter.com/GitHub3F/status/1184170248532119552)

This is most important goal for [IeXod](https://github.com/3F/IeXod) project. To help to avoid the following nightmare: 

❌ From difficulty use (\~ [Microsoft.Build.Locator](https://www.nuget.org/packages/Microsoft.Build.Locator/) an official solution) to unpredictable behavior in various products (\~ Visual Studio etc) due to active integration inside a single environment with *Microsoft.Build.*

## 🎈 Something More

We were just born. Alpha state :3

in progress ...

Follow the news;

* https://github.com/3F
* https://github.com/3F/IeXod
* https://twitter.com/GitHub3F

Contribute;

Enjoy!

# IeXod and custom Sdk Resolvers

IeXod provides independent interface for the easiest implementation of any new Sdk Resolvers.

Extend evaluation as you need; Then, easily register and configure new resolvers on the fly!

# Where is used

( 📅 Planned at least after first stable IeXod release )

* [MvsSln](https://github.com/3F/MvsSln)
* [E-MSBuild](https://github.com/3F/E-MSBuild)
* [.NET DllExport](https://github.com/3F/DllExport)
* [SobaScript](https://github.com/3F/SobaScript)
* [vsSolutionBuildEvent](https://github.com/3F/vsSolutionBuildEvent)
* ...