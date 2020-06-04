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
    /// Verify the functioning of the TaskFinishedEventArgs class.
    /// </summary>
    public class TaskFinishedEventArgs_Tests
    {
        /// <summary>
        /// Trivially exercise event args default ctors to boost Frameworks code coverage
        /// </summary>
        [Fact]
        public void EventArgsCtors()
        {
            TaskFinishedEventArgs targetFinishedEvent = new TaskFinishedEventArgs2();
            targetFinishedEvent = new TaskFinishedEventArgs("Message", "HelpKeyword", "ProjectFile", "TaskFile", "TaskName", true);
            targetFinishedEvent = new TaskFinishedEventArgs("Message", "HelpKeyword", "ProjectFile", "TaskFile", "TaskName", true, DateTime.Now);
            targetFinishedEvent = new TaskFinishedEventArgs(null, null, null, null, null, true);
            targetFinishedEvent = new TaskFinishedEventArgs(null, null, null, null, null, true, DateTime.Now);
        }

        /// <summary>
        /// Create a derived class so that we can test the default constructor in order to increase code coverage and 
        /// verify this code path does not cause any exceptions.
        /// </summary>
        private class TaskFinishedEventArgs2 : TaskFinishedEventArgs
        {
            /// <summary>
            /// Default constructor
            /// </summary>
            public TaskFinishedEventArgs2()
                : base()
            {
            }
        }
    }
}
