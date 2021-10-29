// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace net.r_eg.IeXod.Framework
{
    /// <summary>
    /// This attribute is used to mark a task class as being required to run in a Single Threaded Apartment for COM.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "STA", Justification = "It is cased correctly.")]
    public sealed class RunInSTAAttribute : Attribute
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public RunInSTAAttribute()
        {
            // do nothing
        }
    }
}