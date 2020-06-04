// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace net.r_eg.IeXod.Tasks
{
    /// <summary>
    /// Enum describing the behavior when a a primary reference has an architecture different from the project
    /// </summary>
    internal enum WarnOrErrorOnTargetArchitectureMismatchBehavior
    {
        /// <summary>
        /// Print an error
        /// </summary>
        Error,

        /// <summary>
        /// Print a warning
        /// </summary>
        Warning,

        /// <summary>
        /// Do nothing
        /// </summary>
        None
    }
}
