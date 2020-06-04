// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod.Tasks
{
    /// <summary>
    /// Describes a remapping entry pair
    /// </summary>
    internal class AssemblyRemapping : IEquatable<AssemblyRemapping>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AssemblyRemapping(AssemblyNameExtension from, AssemblyNameExtension to)
        {
            From = from;
            To = to;
        }

        /// <summary>
        /// The assemblyName we mapped from
        /// </summary>
        public AssemblyNameExtension From { get; }

        /// <summary>
        /// The assemblyName we mapped to
        /// </summary>
        public AssemblyNameExtension To { get; }

        /// <summary>
        /// Compare two Assembly remapping objects
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is AssemblyNameExtension name))
            {
                return false;
            }

            return Equals(name);
        }

        /// <summary>
        /// Get the hash code
        /// </summary>
        public override int GetHashCode()
        {
            return From.GetHashCode();
        }

        /// <summary>
        /// We only compare the from because in terms of what is in the redist list unique from's are expected
        /// </summary>
        public bool Equals(AssemblyRemapping other)
        {
            return From.Equals(other.From);
        }
    }
}
