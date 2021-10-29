// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace net.r_eg.IeXod.Construction
{
    /// <summary>
    /// Represents the location of an implicit import.
    /// </summary>
    public enum ImplicitImportLocation
    {
        /// <summary>
        /// The import is not implicitly added and is explicitly added in a user-specified location.
        /// </summary>
        None,
        /// <summary>
        /// The import was implicitly added at the top of the project.
        /// </summary>
        Top,
        /// <summary>
        /// The import was implicitly added at the bottom of the project.
        /// </summary>
        Bottom
    }
}