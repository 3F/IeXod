// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using SdkResolverContextBase = net.r_eg.IeXod.Sdk.SdkResolverContext;

namespace net.r_eg.IeXod.BackEnd.SdkResolution
{
    /// <summary>
    /// An internal implementation of <see cref="Framework.SdkResolverContext"/>.
    /// </summary>
    internal sealed class SdkResolverContext : SdkResolverContextBase
    {
        public SdkResolverContext(Sdk.SdkLogger logger, string projectFilePath, string solutionPath, Version msBuildVersion, bool interactive)
        {
            Logger = logger;
            ProjectFilePath = projectFilePath;
            SolutionFilePath = solutionPath;
            MSBuildVersion = msBuildVersion;
            Interactive = interactive;
        }
    }
}
