// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod.Tasks
{
    /// <summary>
    /// An assembly name coupled with reference information.
    /// </summary>
    internal struct AssemblyNameReference : IComparable<AssemblyNameReference>
    {
        internal AssemblyNameExtension assemblyName;
        internal Reference reference;

        /// <summary>
        /// Display as string.
        /// </summary>
        public override string ToString()
        {
            return assemblyName + ", " + reference;
        }

        /// <summary>
        /// Compare by assembly name.
        /// </summary>
        public int CompareTo(AssemblyNameReference other)
        {
            return assemblyName.CompareTo(other.assemblyName);
        }

        /// <summary>
        /// Construct a new AssemblyNameReference.
        /// </summary>
        public static AssemblyNameReference Create(AssemblyNameExtension assemblyName, Reference reference)
        {
            AssemblyNameReference result;
            result.assemblyName = assemblyName;
            result.reference = reference;
            return result;
        }
    }
}
