// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using net.r_eg.IeXod.Framework.Profiler;

namespace net.r_eg.IeXod.Framework
{
    /// <summary>
    /// Arguments for the project evaluation finished event.
    /// </summary>
    [Serializable]
    public sealed class ProjectEvaluationFinishedEventArgs : BuildStatusEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the ProjectEvaluationFinishedEventArgs class.
        /// </summary>
        public ProjectEvaluationFinishedEventArgs()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the ProjectEvaluationFinishedEventArgs class.
        /// </summary>
        public ProjectEvaluationFinishedEventArgs(string message, params object[] messageArgs)
            : base(message, null, null, DateTime.UtcNow, messageArgs)
        {
        }

        /// <summary>
        /// Gets or sets the full path of the project that started evaluation.
        /// </summary>
        public string ProjectFile { get; set; }

        /// <summary>
        /// The result of profiling a project.
        /// </summary>
        /// <remarks>
        /// Null if profiling is not turned on
        /// </remarks>
        public ProfilerResult? ProfilerResult { get; set; } 
    }
}
