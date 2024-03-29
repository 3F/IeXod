// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP

using Microsoft.DotNet.Cli.Utils;
using System.IO;
using System.Runtime.InteropServices;
using Xunit.Abstractions;

namespace Microsoft.NET.TestFramework.Commands
{
    public sealed class ComposeStoreCommand : MSBuildCommand
    {
        private const string PublishSubfolderName = "packages";

        public ComposeStoreCommand(ITestOutputHelper log, string projectPath, string relativePathToProject = null)
            : base(log, "ComposeStore", projectPath, relativePathToProject)
        {
        }

        public override DirectoryInfo GetOutputDirectory(string targetFramework = "netcoreapp1.0", string configuration = "Debug", string runtimeIdentifier = "")
        {
            string output = Path.Combine(ProjectRootPath, "bin", BuildRelativeOutputPath(targetFramework, configuration, runtimeIdentifier));
            return new DirectoryInfo(output);
        }

        public string GetPublishedAppPath(string appName)
        {
            return Path.Combine(GetOutputDirectory().FullName, $"{appName}.dll");
        }

        private string BuildRelativeOutputPath(string targetFramework, string configuration, string runtimeIdentifier)
        {
            if (runtimeIdentifier.Length == 0)
            {
                runtimeIdentifier = DotNet.PlatformAbstractions.RuntimeEnvironment.GetRuntimeIdentifier();
            }
            string arch = runtimeIdentifier.Substring(runtimeIdentifier.LastIndexOf("-") + 1);
            return Path.Combine(configuration, arch, targetFramework, PublishSubfolderName);
        }
    }
}

#endif
