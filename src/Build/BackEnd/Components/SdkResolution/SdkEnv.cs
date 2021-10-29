// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.Evaluation;

namespace net.r_eg.IeXod.BackEnd.SdkResolution
{
    /// <summary>
    /// An accompanying environment for services which resolve SDKs.
    /// </summary>
    internal struct SdkEnv
    {
        public readonly static SdkEnv Empty = new();

        /// <summary>
        /// The full path to the solution file, if any, that is resolving the SDK.
        /// </summary>
        public string solutionPath;

        /// <summary>
        /// The full path to the project file that is resolving the SDK.
        /// </summary>
        public string projectPath;

        /// <summary>
        /// Used toolset, additional sdk resolvers and/or their paths, and related.
        /// </summary>
        public ProjectToolsOptions toolsOptions;

        internal SdkEnv(string solutionPath, string projectPath, ProjectToolsOptions toolsOptions = null)
            : this()
            => (this.solutionPath, this.projectPath, this.toolsOptions) = (solutionPath, projectPath, toolsOptions);
    }
}
