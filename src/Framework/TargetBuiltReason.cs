// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace net.r_eg.IeXod.Framework
{
    /// <summary>
    /// The reason that a target was built by its parent target.
    /// </summary>
    public enum TargetBuiltReason
    {
        /// <summary>
        /// This wasn't built on because of a parent.
        /// </summary>
        None,

        /// <summary>
        /// The target was part of the parent's BeforeTargets list.
        /// </summary>
        BeforeTargets,

        /// <summary>
        /// The target was part of the parent's DependsOn list.
        /// </summary>
        DependsOn,

        /// <summary>
        /// The target was part of the parent's AfterTargets list.
        /// </summary>
        AfterTargets
    }
}
