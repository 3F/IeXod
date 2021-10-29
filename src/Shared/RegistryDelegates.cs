// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
#if FEATURE_WIN32_REGISTRY

using System;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using Microsoft.Win32;
using System.Collections;
using System.Collections.Generic;

namespace net.r_eg.IeXod.Shared
{  /// <summary>
   /// Given a registry hive and a request view open the base key for that registry location.
   /// </summary>
    internal delegate RegistryKey OpenBaseKey(RegistryHive hive, RegistryView view);

    /// <summary>
    /// Simplified registry access delegate. Given a baseKey and a subKey, get all of the subkey
    /// names.
    /// </summary>
    /// <param name="baseKey">The base registry key.</param>
    /// <param name="subKey">The subkey</param>
    /// <returns>An enumeration of strings.</returns>
    internal delegate IEnumerable<string> GetRegistrySubKeyNames(RegistryKey baseKey, string subKey);

    /// <summary>
    /// Simplified registry access delegate. Given a baseKey and subKey, get the default value
    /// of the subKey.
    /// </summary>
    /// <param name="baseKey">The base registry key.</param>
    /// <param name="subKey">The subkey</param>
    /// <returns>A string containing the default value.</returns>
    internal delegate string GetRegistrySubKeyDefaultValue(RegistryKey baseKey, string subKey);
}
#endif
