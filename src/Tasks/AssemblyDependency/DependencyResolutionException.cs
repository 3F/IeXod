// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace net.r_eg.IeXod.Tasks
{
    /// <summary>
    /// Exception indicates a problem finding dependencies of a reference.
    /// </summary>
    [Serializable]
    internal sealed class DependencyResolutionException : Exception
    {
        /// <summary>
        /// Construct
        /// </summary>
        internal DependencyResolutionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Construct
        /// </summary>
        private DependencyResolutionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
