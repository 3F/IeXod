// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace net.r_eg.IeXod.Collections
{
    /// <summary>
    ///     Defines methods to support the comparison of objects for
    ///     equality over constrained inputs.
    /// </summary>
    internal interface IConstrainedEqualityComparer<in T> : IEqualityComparer<T>
    {
        /// <summary>
        /// Determines whether the specified objects are equal, factoring in the specified bounds when comparing <paramref name="y"/>.
        /// </summary>
        bool Equals(T x, T y, int indexY, int length);

        /// <summary>
        /// Returns a hash code for the specified object factoring in the specified bounds.
        /// </summary>
        int GetHashCode(T obj, int index, int length);
    }
}
