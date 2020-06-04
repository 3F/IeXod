// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace net.r_eg.IeXod.Tasks
{
    /// <summary>
    /// There reference is not a well-formed fusion name *and* its not a file 
    /// that exists on disk.
    /// </summary>
    [Serializable]
    internal sealed class InvalidReferenceAssemblyNameException : Exception
    {
        /// <summary>
        /// Construct
        /// </summary>
        internal InvalidReferenceAssemblyNameException(string sourceItemSpec)
        {
            SourceItemSpec = sourceItemSpec;
        }

        /// <summary>
        /// Construct
        /// </summary>
        private InvalidReferenceAssemblyNameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// The item spec of the item that is the source fo the problem.
        /// </summary>
        internal string SourceItemSpec { get; }
    }
}
