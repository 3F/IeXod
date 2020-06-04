// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using net.r_eg.IeXod.Sdk;

namespace net.r_eg.IeXod.Evaluation
{
    public class ProjectToolsOptions: ICloneable
    {
        private IEnumerable<string> _sdkResolversPaths;
        private IEnumerable<SdkResolver> _sdkResolvers;

        /// <summary>
        /// <see cref="ProjectToolsOptions" /> instance which is used for any default cases.
        /// </summary>
        public readonly static ProjectToolsOptions Default = new ProjectToolsOptions(null, null);

        /// <summary>
        /// Paths to sdk resolvers in filesystem that implements <see cref="net.r_eg.IeXod.SdkResolver" />
        /// Populates all found resolvers into <see cref="SdkResolvers" /> collection.
        /// Never null.
        /// </summary>
        public IEnumerable<string> SdkResolversPaths
        {
            get => _sdkResolversPaths;
            set => _sdkResolversPaths = value ?? Enumerable.Empty<string>();
        }

        /// <summary>
        /// List of specific sdk resolvers to be used when resolving nodes.
        /// <see cref="SdkResolversPaths" /> will supplement defined resolvers.
        /// Never null.
        /// Uses <see cref="SdkResolver.Priority" /> to sort resolvers by its value.
        /// </summary>
        public IEnumerable<SdkResolver> SdkResolvers
        {
            get => _sdkResolvers;
            set => _sdkResolvers = value ?? Enumerable.Empty<SdkResolver>();
        }

        /// <summary>
        /// Affects sdk resolving behavior.
        /// False value will suppress related errors to continue work with unresolved targets and properties.
        /// True value will interrupt processing for any possible unresolved states.
        /// Can also be controlled through <see cref="ProjectLoadSettings.IgnoreFailedSdkResolving" /> flag.
        /// </summary>
        public bool ThrowExceptionIfNotResolvedSdk { get; set; } = false;

        /// <summary>
        /// In addition to <see cref="SdkResolvers"/> net.r_eg.IeXod will also try to use predefined resolvers.
        /// This option helps to avoid this scenario.
        /// </summary>
        public bool DisablePredefinedSdkResolvers { get; set; } = false;

        /// <summary>
        /// The tools version. May be null.
        /// </summary>
        public string ToolsVersion { get; set; }

        /// <summary>
        /// Sub-toolset version to explicitly evaluate the toolset with.  May be null.
        /// </summary>
        public string SubToolsetVersion { get; set; }

        /// <summary>
        /// <inheritdoc cref="Clone" />
        /// </summary>
        public ProjectToolsOptions Copy() => (ProjectToolsOptions)Clone();

        /// <param name="sdkResolversPaths">Value for <see cref="ProjectToolsOptions.SdkResolversPaths" /></param>
        /// <param name="toolsVersion">Value for <see cref="ToolsVersion" /></param>
        /// <param name="subToolsetVersion">Value for <see cref="SubToolsetVersion" /></param>
        public ProjectToolsOptions(IEnumerable<string> sdkResolversPaths, string toolsVersion = null, string subToolsetVersion = null)
            : this(toolsVersion, subToolsetVersion)
        {
            SdkResolversPaths = sdkResolversPaths;
        }

        /// <param name="toolsVersion">Value for <see cref="ToolsVersion" /></param>
        /// <param name="subToolsetVersion">Value for <see cref="SubToolsetVersion" /></param>
        public ProjectToolsOptions(string toolsVersion, string subToolsetVersion = null)
            : this()
        {
            ToolsVersion        = toolsVersion;
            SubToolsetVersion   = subToolsetVersion;
        }

        internal ProjectToolsOptions()
        {
            // To trigger `set` accessors
            SdkResolvers        = null;
            SdkResolversPaths   = null;
        }

        #region ICloneable

        /// <summary>
        /// Only shallow copy. Note <see cref="SdkResolvers" /> instances.
        /// </summary>
        public object Clone() => MemberwiseClone();

        #endregion
    }
}
