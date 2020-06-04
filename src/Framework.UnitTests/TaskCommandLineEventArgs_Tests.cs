// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using net.r_eg.IeXod.Framework;
using Xunit;

namespace net.r_eg.IeXod.UnitTests
{
    /// <summary>
    /// Verify the functioning of the TaskCommandLineEventArgs class.
    /// </summary>
    public class TaskCommandLineEventArgs_Tests
    {
        /// <summary>
        /// Trivially exercise event args default ctors to boost Frameworks code coverage
        /// </summary>
        [Fact]
        public void EventArgsCtors()
        {
            TaskCommandLineEventArgs taskCommandLineEvent = new TaskCommandLineEventArgs2();
            taskCommandLineEvent = new TaskCommandLineEventArgs("Commandline", "taskName", MessageImportance.High);
            taskCommandLineEvent = new TaskCommandLineEventArgs("Commandline", "taskName", MessageImportance.High, DateTime.Now);
            taskCommandLineEvent = new TaskCommandLineEventArgs(null, null, MessageImportance.High);
            taskCommandLineEvent = new TaskCommandLineEventArgs(null, null, MessageImportance.High, DateTime.Now);
        }

        /// <summary>
        /// Create a derived class so that we can test the default constructor in order to increase code coverage and 
        /// verify this code path does not cause any exceptions.
        /// </summary>
        private class TaskCommandLineEventArgs2 : TaskCommandLineEventArgs
        {
            /// <summary>
            /// Default constructor
            /// </summary>
            public TaskCommandLineEventArgs2() : base()
            {
            }
        }
    }
}