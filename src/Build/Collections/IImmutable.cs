// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace net.r_eg.IeXod.Collections
{
    /// <summary>
    /// Interface indicating a type is immutable, to constrain generic types.
    /// </summary>
    /// <remarks>
    /// This can be internal as it is a constraint only on internal collections.
    /// </remarks>
    internal interface IImmutable
    {
    }
}
