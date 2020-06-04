﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace net.r_eg.IeXod.Tasks
{
    /// <summary>
    /// If this reference lost in a conflict with another reference, this reason explains 
    /// why.
    /// </summary>
    internal enum ConflictLossReason
    {
        /// <summary>
        /// This reference didn't lose a conflict.
        /// </summary>
        DidntLose,

        /// <summary>
        /// This reference matched another assembly that had a higher version number.
        /// </summary>
        HadLowerVersion,

        /// <summary>
        /// The two assemblies cannot be reconciled.
        /// </summary>
        InsolubleConflict,

        /// <summary>
        /// In this case, this reference was a dependency and the other reference was 
        /// primary (specified in the project file). 
        /// </summary>
        WasNotPrimary,

        /// <summary>
        /// The two references were equivalent according to fusion and also have the same version.
        /// Its hard to see how this could happen, but handle it.
        /// </summary>
        FusionEquivalentWithSameVersion
    }
}
