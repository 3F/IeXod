// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace net.r_eg.IeXod.Collections
{
    /// <summary>
    /// Interface allowing values of things to be gotten.
    /// </summary>
    /// <remarks>
    /// This can be internal as it is a constraint only on internal collections.
    /// </remarks>
    internal interface IValued
    {
        /// <summary>
        /// Returns some value of a thing
        /// </summary>
        string EscapedValue { get; }
    }
}
