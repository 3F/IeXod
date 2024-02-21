// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using net.r_eg.IeXod.Internal;
using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod
{
    /// <summary>
    /// Some minimal set with most known or unspecified values
    /// which should be used in a fallback states.
    /// </summary>
    internal static class FallbackPropertyValues
    {
        internal static readonly ReadOnlyDictionary<string, string> MSBuildCurrentToolset = new(new Dictionary<string, string>(25)
        {
            { ReservedPropertyNames.extensionsPath, "$([MSBuild]::GetMSBuildExtensionsPath())" },
            { ReservedPropertyNames.extensionsPath32, "$([MSBuild]::GetMSBuildExtensionsPath())" },
            { ReservedPropertyNames.extensionsPath64, "$([MSBuild]::GetMSBuildExtensionsPath())" },
            //{ ReservedPropertyNames.extensionsPath64, @"$(MSBuildProgramFiles32)\MSBuild" },

            { "MSBuildToolsPath32", "$([MSBuild]::GetToolsDirectory32())" },
            { "MSBuildToolsPath64", "$([MSBuild]::GetToolsDirectory64())" },
            { "MSBuildToolsPath", "$([MSBuild]::GetCurrentToolsDirectory())" },

            { "MSBuildRuntimeVersion", "4.0.30319" },

            { "MSBuildSDKsPath", "$([MSBuild]::GetMSBuildSDKsPath())" },
            { "MSBuildFrameworkToolsPath", @"$(SystemRoot)\Microsoft.NET\Framework\v$(MSBuildRuntimeVersion)\" },
            { "MSBuildFrameworkToolsPath32", @"$(SystemRoot)\Microsoft.NET\Framework\v$(MSBuildRuntimeVersion)\" },
            { "MSBuildFrameworkToolsPath64", @"$(SystemRoot)\Microsoft.NET\Framework64\v$(MSBuildRuntimeVersion)\" },
            { "MSBuildFrameworkToolsPathArm64", @"$(SystemRoot)\Microsoft.NET\FrameworkArm64\v$(MSBuildRuntimeVersion)\" },

            { ReservedPropertyNames.frameworkToolsRoot, $"{NativeMethodsShared.FrameworkBasePath}\\" },
            { "VsInstallRoot", "$([MSBuild]::GetVsInstallRoot())" },
            { "MSBuildToolsRoot", @"$(VsInstallRoot)\MSBuild" },
            { "RoslynTargetsPath", @"$([MSBuild]::GetToolsDirectory32())\Roslyn" },

            // VC Specific Paths
            { "VCTargetsPath16", @"$([MSBuild]::ValueOrDefault('$(VCTargetsPath16)','$(MSBuildExtensionsPath32)\Microsoft\VC\v160\'))" },
            { "VCTargetsPath14", @"$([MSBuild]::ValueOrDefault('$(VCTargetsPath14)','$([MSBuild]::GetProgramFiles32())\MSBuild\Microsoft.Cpp\v4.0\V140\'))" },
            { "VCTargetsPath12", @"$([MSBuild]::ValueOrDefault('$(VCTargetsPath12)','$([MSBuild]::GetProgramFiles32())\MSBuild\Microsoft.Cpp\v4.0\V120\'))" },
            { "VCTargetsPath11", @"$([MSBuild]::ValueOrDefault('$(VCTargetsPath11)','$([MSBuild]::GetProgramFiles32())\MSBuild\Microsoft.Cpp\v4.0\V110\'))" },
            { "VCTargetsPath10", @"$([MSBuild]::ValueOrDefault('$(VCTargetsPath10)','$([MSBuild]::GetProgramFiles32())\MSBuild\Microsoft.Cpp\v4.0\'))" },
            { "VCTargetsPath", "$(VCTargetsPath16)" },

            // VSSDK
            //{ "VSToolsPath", @"$(MSBuildProgramFiles32)\MSBuild\Microsoft\VisualStudio\v$(VisualStudioVersion)" },

        });
    }
}
