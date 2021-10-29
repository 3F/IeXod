// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.Collections;
using net.r_eg.IeXod.Construction;
using net.r_eg.IeXod.Execution;
using net.r_eg.IeXod.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net.r_eg.IeXod.Evaluation
{
    internal class ToolsetLocalReader : ToolsetReader
    {
        private readonly IElementLocation _sourceLocation = new RegistryLocation("ToolsetLocalReader");

        internal ToolsetLocalReader(PropertyDictionary<ProjectPropertyInstance> environmentProperties, PropertyDictionary<ProjectPropertyInstance> globalProperties)
           : base(environmentProperties, globalProperties)
        {
        }

        protected override string DefaultOverrideToolsVersion => MSBuildConstants.CurrentToolsVersion;

        protected override string DefaultToolsVersion => MSBuildConstants.CurrentToolsVersion;

        protected override string MSBuildOverrideTasksPath => BuildEnvironmentHelper.Instance.CurrentMSBuildToolsDirectory;

        protected override IEnumerable<ToolsetPropertyDefinition> ToolsVersions
        {
            get
            {
                yield return new ToolsetPropertyDefinition(MSBuildConstants.CurrentToolsVersion, string.Empty, _sourceLocation);
            }
        }

        protected override IEnumerable<ToolsetPropertyDefinition> GetPropertyDefinitions(string toolsVersion)
        {
            yield return new ToolsetPropertyDefinition(MSBuildConstants.ToolsPath, BuildEnvironmentHelper.Instance.CurrentMSBuildToolsDirectory, _sourceLocation);
            yield return new ToolsetPropertyDefinition(MSBuildConstants.SdksPath, BuildEnvironmentHelper.Instance.MSBuildSDKsPath, _sourceLocation);
            yield return new ToolsetPropertyDefinition("RoslynTargetsPath",
                System.IO.Path.Combine(BuildEnvironmentHelper.Instance.CurrentMSBuildToolsDirectory, "Roslyn"),
                _sourceLocation);
        }

        protected override IEnumerable<ToolsetPropertyDefinition> GetSubToolsetPropertyDefinitions(string toolsVersion, string subToolsetVersion)
        {
            return Enumerable.Empty<ToolsetPropertyDefinition>();
        }

        protected override Dictionary<string, ProjectImportPathMatch> GetProjectImportSearchPathsTable(string toolsVersion, string os)
        {
            return new Dictionary<string, ProjectImportPathMatch>();
        }

        protected override IEnumerable<string> GetSubToolsetVersions(string toolsVersion)
        {
            return Enumerable.Empty<string>();
        }
    }
}
