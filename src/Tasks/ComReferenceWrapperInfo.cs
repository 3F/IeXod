// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod.Tasks
{
    /// <summary>
    /// Class containing info about wrapper location, used for caching.
    /// </summary>
    internal class ComReferenceWrapperInfo
    {
        // path to the wrapper assembly
        internal string path;

        // wrapper assembly
        internal Assembly assembly;

        // It's possible for PIAs to get redirected to a different assembly (a newer version), so we must
        // remember the original name in case a component asks us to resolve a dependency using that old name
        internal AssemblyNameExtension originalPiaName;
    }
}
