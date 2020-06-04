// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//-----------------------------------------------------------------------
// Contains information about entries in the AssemblyFoldersEx registry keys.
//-----------------------------------------------------------------------
#if FEATURE_WIN32_REGISTRY

using System;
using net.r_eg.IeXod.Shared;
using Microsoft.Win32;

namespace net.r_eg.IeXod.Utilities
{
    /// <summary>
    /// Contains information about entries in the AssemblyFoldersEx registry keys.
    /// </summary>
    public class AssemblyFoldersExInfo
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AssemblyFoldersExInfo(RegistryHive hive, RegistryView view, string registryKey, string directoryPath, Version targetFrameworkVersion)
        {
            ErrorUtilities.VerifyThrowArgumentNull(registryKey, nameof(registryKey));
            ErrorUtilities.VerifyThrowArgumentNull(directoryPath, nameof(directoryPath));
            ErrorUtilities.VerifyThrowArgumentNull(targetFrameworkVersion, nameof(targetFrameworkVersion));

            Hive = hive;
            View = view;
            Key = registryKey;
            DirectoryPath = directoryPath;
            TargetFrameworkVersion = targetFrameworkVersion;
        }

        /// <summary>
        /// Registry hive used
        /// </summary>
        public RegistryHive Hive { get; }

        /// <summary>
        /// Registry view used
        /// </summary>
        public RegistryView View { get; }

        /// <summary>
        /// The registry key to the component
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Folder found at the registry keys default value
        /// </summary>
        public string DirectoryPath { get; }

        /// <summary>
        /// Target framework version for the registry key
        /// </summary>
        public Version TargetFrameworkVersion { get; }
    }
}
#endif
