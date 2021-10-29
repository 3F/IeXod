// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;

namespace net.r_eg.IeXod.Internal
{
    /// <summary>
    /// Gets the <see cref="AssemblyFileVersionAttribute"/> of net.r_eg.IeXod.dll.
    /// </summary>
    internal sealed class MSBuildAssemblyFileVersion
    {
        private static readonly Lazy<MSBuildAssemblyFileVersion> MSBuildAssemblyFileVersionLazy = new Lazy<MSBuildAssemblyFileVersion>(GetMSBuildAssemblyFileVersion, isThreadSafe: true);

        private MSBuildAssemblyFileVersion(string majorMinorBuild)
        {
            MajorMinorBuild = majorMinorBuild;
        }

        /// <summary>
        /// Gets a singleton instance of <see cref="MSBuildAssemblyFileVersion"/>.
        /// </summary>
        public static MSBuildAssemblyFileVersion Instance
        {
            get { return MSBuildAssemblyFileVersionLazy.Value; }
        }

        /// <summary>
        /// Gets the assembly file version in the format major.minor.
        /// </summary>
        public string MajorMinorBuild { get; set; }

        private static MSBuildAssemblyFileVersion GetMSBuildAssemblyFileVersion()
        {
            string versionString = typeof(MSBuildAssemblyFileVersion)
                .GetTypeInfo()
                ?.Assembly
                .GetCustomAttribute<AssemblyFileVersionAttribute>()
                ?.Version;

            Version version;

            if (String.IsNullOrEmpty(versionString) || !Version.TryParse(versionString, out version))
            {
                // Fall back to the constant AssemblyVersion
                version = Version.Parse(Constants.AssemblyVersion);
            }

            return new MSBuildAssemblyFileVersion($"{version.Major}.{version.Minor}.{version.Build}");
        }
    }
}
