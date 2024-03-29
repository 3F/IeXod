﻿// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.NET.TestFramework
{
    public static class FileConstants
    {
        public static readonly string DynamicLibPrefix = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "" : "lib";

        public static readonly string DynamicLibSuffix = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ".dll" :
                                                         RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? ".dylib" : ".so";
        public static readonly string UserProfileFolder = Environment.GetEnvironmentVariable(
                                                                RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
                                                                    "USERPROFILE" : "HOME");
    }
}
