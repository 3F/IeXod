// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace net.r_eg.IeXod.Tasks
{
    /// <summary>
    /// Compare the two AssemblyNameReferences by version number.
    /// </summary>
    internal sealed class AssemblyNameReferenceAscendingVersionComparer : IComparer<AssemblyNameReference>
    {
        internal static readonly IComparer<AssemblyNameReference> comparer = new AssemblyNameReferenceAscendingVersionComparer();

        private static Version DummyVersion { get; } = new Version(0, 0);

        /// <summary>
        /// Private construct so there's only one instance.
        /// </summary>
        private AssemblyNameReferenceAscendingVersionComparer()
        {
        }

        /// <summary>
        /// Compare the two AssemblyNameReferences by version number.
        /// </summary>
        public int Compare(AssemblyNameReference i1, AssemblyNameReference i2)
        {
            Version v1 = i1.assemblyName.Version;
            Version v2 = i2.assemblyName.Version;

            if (v1 == null)
            {
                v1 = DummyVersion;
            }

            if (v2 == null)
            {
                v2 = DummyVersion;
            }

            return v1.CompareTo(v2);
        }
    }
}
