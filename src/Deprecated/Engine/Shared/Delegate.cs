// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace net.r_eg.IeXod.BuildEngine.Shared
{
    /// <summary>
    /// GetDirectories delegate
    /// </summary>
    /// <param name="path">The path to get directories for.</param>
    /// <param name="pattern">The pattern to search for.</param>
    /// <returns>An array of directories.</returns>
    internal delegate string[] GetDirectories(string path, string pattern);
}
