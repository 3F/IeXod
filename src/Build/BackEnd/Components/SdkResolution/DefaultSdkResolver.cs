// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using net.r_eg.IeXod.Sdk;
using net.r_eg.IeXod.Shared;

using SdkResolverBase = net.r_eg.IeXod.Sdk.SdkResolver;
using SdkResolverContextBase = net.r_eg.IeXod.Sdk.SdkResolverContext;
using SdkResultBase = net.r_eg.IeXod.Sdk.SdkResult;
using SdkResultFactoryBase = net.r_eg.IeXod.Sdk.SdkResultFactory;

namespace net.r_eg.IeXod.BackEnd.SdkResolution
{
    /// <summary>
    ///     Default SDK resolver for compatibility with VS2017 RTM.
    /// <remarks>
    ///     Default Sdk folder will to:
    ///         1) MSBuildSDKsPath environment variable if defined
    ///         2) When in Visual Studio, (VSRoot)\MSBuild\Sdks\
    ///         3) Outside of Visual Studio (MSBuild Root)\Sdks\
    /// </remarks>
    /// </summary>
    internal class DefaultSdkResolver : SdkResolverBase
    {
        public override string Name => "DefaultSdkResolver";

        public override int Priority => 10000;

        public override SdkResultBase Resolve(SdkReference sdk, SdkResolverContextBase context, SdkResultFactoryBase factory)
        {
            var sdkPath = Path.Combine(BuildEnvironmentHelper.Instance.MSBuildSDKsPath, sdk.Name, "Sdk");

            // Note: On failure MSBuild will log a generic message, no need to indicate a failure reason here.
            return FileUtilities.DirectoryExistsNoThrow(sdkPath)
                ? factory.IndicateSuccess(sdkPath, string.Empty)
                : factory.IndicateFailure(null);
        }
    }
}
