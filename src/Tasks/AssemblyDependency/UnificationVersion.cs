// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace net.r_eg.IeXod.Tasks
{
    /// <summary>
    /// A version number coupled with a reason why this version number
    /// was chosen.
    /// </summary>
    internal struct UnificationVersion
    {
        internal string referenceFullPath;
        internal Version version;
        internal UnificationReason reason;
    }
}
