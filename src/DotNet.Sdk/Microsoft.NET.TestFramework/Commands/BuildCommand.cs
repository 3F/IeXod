// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.DotNet.Cli.Utils;
using Xunit.Abstractions;

namespace Microsoft.NET.TestFramework.Commands
{
    public sealed class BuildCommand : MSBuildCommand
    {
        public BuildCommand(ITestOutputHelper log, string projectRootPath, string relativePathToProject = null)
            : base(log, "Build", projectRootPath, relativePathToProject)
        {
        }
    }
}
