﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace net.r_eg.IeXod.Tasks
{
    /// <summary>
    /// Reasons why a resolution might fail.
    /// </summary>
    internal enum NoMatchReason
    {
        /// <summary>
        /// The default state.
        /// </summary>
        Unknown,

        /// <summary>
        /// There was no file found.
        /// </summary>
        FileNotFound,

        /// <summary>
        /// The file was found, but its fusion name didn't match.
        /// </summary>
        FusionNamesDidNotMatch,

        /// <summary>
        /// The file was found, but it didn't have a fusion name. 
        /// </summary>
        TargetHadNoFusionName,

        /// <summary>
        /// The file is not in the GAC.
        /// </summary>
        NotInGac,

        /// <summary>
        /// If treated as a filename, the file doesn't exist on disk.
        /// </summary>
        NotAFileNameOnDisk,

        /// <summary>
        /// The processor architecture does not match the targeted processor architecture.
        /// </summary>
        ProcessorArchitectureDoesNotMatch
    }
}
