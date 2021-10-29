// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace net.r_eg.IeXod.BackEnd
{
    /// <summary>
    /// An exception representing the case where a BuildRequest has caused a circular project dependency.  This is used to
    /// terminate the request builder which initiated the failure path.
    /// </summary>
    /// <remarks>
    /// If you add fields to this class, add a custom serialization constructor and override GetObjectData().
    /// </remarks>
    [Serializable]
    internal class CircularDependencyException : Exception
    {
        /// <summary>
        /// Constructs a standard BuildAbortedException.
        /// </summary>
        internal CircularDependencyException()
        {
        }

        internal CircularDependencyException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        protected CircularDependencyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
