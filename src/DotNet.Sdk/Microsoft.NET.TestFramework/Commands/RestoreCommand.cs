// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.DotNet.Cli.Utils;
using NuGet.Configuration;
using Xunit.Abstractions;

namespace Microsoft.NET.TestFramework.Commands
{
    public sealed class RestoreCommand : MSBuildCommand
    {
        public RestoreCommand(ITestOutputHelper log, string projectPath, string relativePathToProject = null)
            : base(log, "Restore", projectPath, relativePathToProject)
        {
        }

        protected override bool ExecuteWithRestoreByDefault => false;
    }
}
