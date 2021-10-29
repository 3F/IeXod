// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace net.r_eg.IeXod.Framework
{
    /// <summary>
    /// Arguments for the project evaluation started event.
    /// </summary>
    [Serializable]
    public class ProjectEvaluationStartedEventArgs : BuildStatusEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the ProjectEvaluationStartedEventArgs class.
        /// </summary>
        public ProjectEvaluationStartedEventArgs()
        {

        }

        /// <summary>
        /// Initializes a new instance of the ProjectEvaluationStartedEventArgs class.
        /// </summary>
        public ProjectEvaluationStartedEventArgs(string message, params object[] messageArgs)
            : base(message, null, null, DateTime.UtcNow, messageArgs)
        {
        }

        /// <summary>
        /// Gets or sets the full path of the project that started evaluation.
        /// </summary>
        public string ProjectFile { get; set; }
    }
}
