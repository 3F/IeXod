// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.DotNet.Cli.Utils;

public static class AssertionHelper
{
    public static string[] AppendApphostOnNonMacOS(string ProjectName, string[] expectedFiles)
    {
        string apphost = $"{ProjectName}{Constants.ExeSuffix}";
        // No UseApphost is false by default on macOS
        return !RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
            ? expectedFiles.Append(apphost).ToArray()
            : expectedFiles;
    }
}
