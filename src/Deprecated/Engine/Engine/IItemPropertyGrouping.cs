// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace net.r_eg.IeXod.BuildEngine
{
    /// <summary>
    /// Just an empty interface that is "implemented" by BuildPropertyGroup, BuildItemGroup, and Choose.
    /// It's just so we can pass these objects around as similar things.  The other alternative would
    /// have been just to use "Object", but that's even less strongly typed.
    /// </summary>
    /// <owner>DavidLe, RGoel</owner>
    internal interface IItemPropertyGrouping
    {
    }
}
