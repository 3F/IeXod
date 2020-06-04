﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod.Utilities
{
    /// <summary>
    /// This helper base class provides default functionality for loggers that handle events raised by the build engine. This
    /// class can only be instantiated in a derived form.
    /// </summary>
    public abstract class Logger : ILogger
    {
        /// <summary>
        /// Gets or sets the level of detail to show in the event log.
        /// </summary>
        /// <value>The verbosity level.</value>
        public virtual LoggerVerbosity Verbosity { get; set; }

        /// <summary>
        /// Gets or sets the user-specified parameters to the logger. Derived class can ignore if it doesn't take parameters.
        /// </summary>
        /// <value>The parameter string (can be null).</value>
        public virtual string Parameters { get; set; }

        /// <summary>
        /// Must be implemented by derived class to subscribe to build events.
        /// </summary>
        /// <param name="eventSource">The available events that a logger can subscribe to.</param>
        public abstract void Initialize(IEventSource eventSource);

        /// <summary>
        /// Derived class should override if it needs to release any resources.
        /// </summary>
        public virtual void Shutdown()
        {
            // do nothing
        }

        /// <summary>
        /// Generates a message in the default format from a BuildErrorEventArgs object.
        /// </summary>
        /// <param name="args">Error event arguments</param>
        /// <returns>Error message in canonical format</returns>
        public virtual string FormatErrorEvent(BuildErrorEventArgs args) => EventArgsFormatting.FormatEventMessage(args);

        /// <summary>
        /// Generates a message in the default format from a BuildWarningEventArgs object.
        /// </summary>
        /// <param name="args">Warning event arguments</param>
        /// <returns>Warning message in canonical format</returns>
        public virtual string FormatWarningEvent(BuildWarningEventArgs args) => EventArgsFormatting.FormatEventMessage(args);

        /// <summary>
        /// Determines whether the current verbosity setting is at least the value
        /// passed in.
        /// </summary>
        /// <param name="checkVerbosity"></param>
        /// <returns></returns>
        public bool IsVerbosityAtLeast(LoggerVerbosity checkVerbosity) => Verbosity >= checkVerbosity;
    }
}
