// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.DotNet.Cli.Utils;

namespace Microsoft.NET.TestFramework
{
    public class TestAssetSubdirectories
    {
        public static string DesktopTestProjects = "DesktopTestProjects";

        public static string TestProjects = "TestProjects";

        public static string NonRestoredTestProjects = "NonRestoredTestProjects";
    }
}
