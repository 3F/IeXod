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
    public sealed class PackCommand : MSBuildCommand
    {
        public PackCommand(ITestOutputHelper log, string projectPath, string relativePathToProject = null)
            : base(log, "Pack", projectPath, relativePathToProject)
        {
        }

        public string GetIntermediateNuspecPath(string packageId = null, string configuration = "Debug", string packageVersion = "1.0.0")
        {
            if (packageId == null)
            {
                packageId = Path.GetFileNameWithoutExtension(ProjectFile);
            }

            return Path.Combine(GetBaseIntermediateDirectory().FullName, configuration, $"{packageId}.{packageVersion}.nuspec");
        }

        public string GetNuGetPackage(string packageId = null, string configuration = "Debug", string packageVersion = "1.0.0")
        {
            if (packageId == null)
            {
                packageId = Path.GetFileNameWithoutExtension(ProjectFile);
            }

            return Path.Combine(GetOutputDirectory(null, configuration).FullName, $"{packageId}.{packageVersion}.nupkg");
        }
    }
}
