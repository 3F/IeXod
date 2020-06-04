﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace net.r_eg.IeXod.Framework
{
    /// <summary>
    /// Arguments for the target skipped event.
    /// </summary>
    [Serializable]
    public class TargetSkippedEventArgs : BuildMessageEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the TargetSkippedEventArgs class.
        /// </summary>
        public TargetSkippedEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the TargetSkippedEventArgs class.
        /// </summary>
        public TargetSkippedEventArgs
        (
            string message,
            params object[] messageArgs
        )
            : base(null, null, null, 0, 0, 0, 0, message, null, null, MessageImportance.Low, DateTime.UtcNow, messageArgs)
        {
        }

        /// <summary>
        /// Gets or sets the name of the target being skipped.
        /// </summary>
        public string TargetName { get; set; }

        /// <summary>
        /// Gets or sets the parent target of the target being skipped.
        /// </summary>
        public string ParentTarget { get; set; }

        /// <summary>
        /// File where this target was declared.
        /// </summary>
        public string TargetFile { get; set; }

        /// <summary>
        /// Why the parent target built this target.
        /// </summary>
        public TargetBuiltReason BuildReason { get; set; }
    }
}
