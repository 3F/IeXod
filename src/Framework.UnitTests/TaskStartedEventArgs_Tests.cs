// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using net.r_eg.IeXod.Framework;
using Xunit;

namespace net.r_eg.IeXod.UnitTests
{
    /// <summary>
    /// Verify the functioning of the TaskStartedEventArgs class.
    /// </summary>
    public class TaskStartedEventArgs_Tests
    {
        /// <summary>
        /// Trivially exercise event args default ctors to boost Frameworks code coverage
        /// </summary>
        [Fact]
        public void EventArgsCtors()
        {
#pragma warning disable 219
            TaskStartedEventArgs taskStartedEvent = new TaskStartedEventArgs2();
            taskStartedEvent = new TaskStartedEventArgs("Message", "HelpKeyword", "ProjectFile", "TaskFile", "TaskName");
            taskStartedEvent = new TaskStartedEventArgs("Message", "HelpKeyword", "ProjectFile", "TaskFile", "TaskName", DateTime.Now);
            taskStartedEvent = new TaskStartedEventArgs(null, null, null, null, null);
            taskStartedEvent = new TaskStartedEventArgs(null, null, null, null, null, DateTime.Now);
#pragma warning restore 219
        }

        /// <summary>
        /// Create a derived class so that we can test the default constructor in order to increase code coverage and 
        /// verify this code path does not cause any exceptions.
        /// </summary>
        private class TaskStartedEventArgs2 : TaskStartedEventArgs
        {
            /// <summary>
            /// Default constructor
            /// </summary>
            public TaskStartedEventArgs2()
                : base()
            {
            }
        }
    }
}
