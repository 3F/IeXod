// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace net.r_eg.IeXod.Shared
{
    /// <summary>
    /// Contains information about entries in the AssemblyFoldersEx registry keys.
    /// </summary>
    internal class ExtensionFoldersRegistryKey
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ExtensionFoldersRegistryKey(string registryKey, Version targetFrameworkVersion)
        {
            ErrorUtilities.VerifyThrowArgumentNull(registryKey, nameof(registryKey));
            ErrorUtilities.VerifyThrowArgumentNull(targetFrameworkVersion, nameof(targetFrameworkVersion));

            RegistryKey = registryKey;
            TargetFrameworkVersion = targetFrameworkVersion;
        }

        /// <summary>
        /// The registry key to the component
        /// </summary>
        internal string RegistryKey { get; }

        /// <summary>
        /// Target framework version for the registry key
        /// </summary>
        internal Version TargetFrameworkVersion { get; }
    }
}
