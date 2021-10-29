// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace net.r_eg.IeXod.Collections
{
    /// <summary>
    /// Interface allowing items and metadata and properties to go into keyed collections
    /// </summary>
    /// <remarks>
    /// This can be internal as it is a constraint only on internal collections.
    /// </remarks>
    internal interface IKeyed
    {
        /// <summary>
        /// Returns some value useful for a key in a dictionary
        /// </summary>
        string Key
        {
            get;
        }
    }
}