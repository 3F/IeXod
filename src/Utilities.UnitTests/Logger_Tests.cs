// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Utilities;
using Shouldly;
using Xunit;

namespace net.r_eg.IeXod.UnitTests
{
    internal class EmptyLogger : Logger
    {
        /// <summary>
        /// Create a logger instance with a specific verbosity.
        /// </summary>
        /// <param name="verbosity">Verbosity level.</param>
        public EmptyLogger(LoggerVerbosity verbosity)
        {
            Verbosity = verbosity;
        }

        /// <summary>
        /// Subscribe to events.
        /// </summary>
        /// <param name="eventSource"></param>
        public override void Initialize(IEventSource eventSource)
        {
        }
    }

    public class Logger_Tests
    {
        [Fact]
        public void ExerciseMiscProperties()
        {
            EmptyLogger logger = new EmptyLogger(LoggerVerbosity.Diagnostic);
            logger.Parameters = "Parameters";
            Assert.Equal(0, string.Compare(logger.Parameters, "Parameters", StringComparison.OrdinalIgnoreCase));
            Assert.Equal(LoggerVerbosity.Diagnostic, logger.Verbosity);
            logger.Shutdown();
        }
        /// <summary>
        /// Exercises every combination of the Logger.IsVerbosityAtLeast method.
        /// </summary>
        [Fact]
        public void IsVerbosityAtLeast()
        {
            new EmptyLogger(LoggerVerbosity.Quiet).IsVerbosityAtLeast(LoggerVerbosity.Quiet).ShouldBeTrue();
            new EmptyLogger(LoggerVerbosity.Quiet).IsVerbosityAtLeast(LoggerVerbosity.Minimal).ShouldBeFalse();
            new EmptyLogger(LoggerVerbosity.Quiet).IsVerbosityAtLeast(LoggerVerbosity.Normal).ShouldBeFalse();
            new EmptyLogger(LoggerVerbosity.Quiet).IsVerbosityAtLeast(LoggerVerbosity.Detailed).ShouldBeFalse();
            new EmptyLogger(LoggerVerbosity.Quiet).IsVerbosityAtLeast(LoggerVerbosity.Diagnostic).ShouldBeFalse();

            new EmptyLogger(LoggerVerbosity.Minimal).IsVerbosityAtLeast(LoggerVerbosity.Quiet).ShouldBeTrue();
            new EmptyLogger(LoggerVerbosity.Minimal).IsVerbosityAtLeast(LoggerVerbosity.Minimal).ShouldBeTrue();
            new EmptyLogger(LoggerVerbosity.Minimal).IsVerbosityAtLeast(LoggerVerbosity.Normal).ShouldBeFalse();
            new EmptyLogger(LoggerVerbosity.Minimal).IsVerbosityAtLeast(LoggerVerbosity.Detailed).ShouldBeFalse();
            new EmptyLogger(LoggerVerbosity.Minimal).IsVerbosityAtLeast(LoggerVerbosity.Diagnostic).ShouldBeFalse();

            new EmptyLogger(LoggerVerbosity.Normal).IsVerbosityAtLeast(LoggerVerbosity.Quiet).ShouldBeTrue();
            new EmptyLogger(LoggerVerbosity.Normal).IsVerbosityAtLeast(LoggerVerbosity.Minimal).ShouldBeTrue();
            new EmptyLogger(LoggerVerbosity.Normal).IsVerbosityAtLeast(LoggerVerbosity.Normal).ShouldBeTrue();
            new EmptyLogger(LoggerVerbosity.Normal).IsVerbosityAtLeast(LoggerVerbosity.Detailed).ShouldBeFalse();
            new EmptyLogger(LoggerVerbosity.Normal).IsVerbosityAtLeast(LoggerVerbosity.Diagnostic).ShouldBeFalse();

            new EmptyLogger(LoggerVerbosity.Detailed).IsVerbosityAtLeast(LoggerVerbosity.Quiet).ShouldBeTrue();
            new EmptyLogger(LoggerVerbosity.Detailed).IsVerbosityAtLeast(LoggerVerbosity.Minimal).ShouldBeTrue();
            new EmptyLogger(LoggerVerbosity.Detailed).IsVerbosityAtLeast(LoggerVerbosity.Normal).ShouldBeTrue();
            new EmptyLogger(LoggerVerbosity.Detailed).IsVerbosityAtLeast(LoggerVerbosity.Detailed).ShouldBeTrue();
            new EmptyLogger(LoggerVerbosity.Detailed).IsVerbosityAtLeast(LoggerVerbosity.Diagnostic).ShouldBeFalse();

            new EmptyLogger(LoggerVerbosity.Diagnostic).IsVerbosityAtLeast(LoggerVerbosity.Quiet).ShouldBeTrue();
            new EmptyLogger(LoggerVerbosity.Diagnostic).IsVerbosityAtLeast(LoggerVerbosity.Minimal).ShouldBeTrue();
            new EmptyLogger(LoggerVerbosity.Diagnostic).IsVerbosityAtLeast(LoggerVerbosity.Normal).ShouldBeTrue();
            new EmptyLogger(LoggerVerbosity.Diagnostic).IsVerbosityAtLeast(LoggerVerbosity.Detailed).ShouldBeTrue();
            new EmptyLogger(LoggerVerbosity.Diagnostic).IsVerbosityAtLeast(LoggerVerbosity.Diagnostic).ShouldBeTrue();
        }
    }
}
