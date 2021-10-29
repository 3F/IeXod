// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using Microsoft.DotNet.Cli.Utils;
using System.IO;
using Xunit.Abstractions;

namespace Microsoft.NET.TestFramework.Commands
{
    public sealed class RebuildCommand : MSBuildCommand
    {
        public RebuildCommand(ITestOutputHelper log, string projectPath, string relativePathToProject = null)
            : base(log, "Rebuild", projectPath, relativePathToProject)
        {
        }
    }
}
