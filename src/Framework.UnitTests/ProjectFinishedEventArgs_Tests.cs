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
    /// Verify the functioning of the ProjectFinishedEventArgs class.
    /// </summary>
    public class ProjectFinishedEventArgs_Tests
    {
        /// <summary>
        /// Trivially exercise event args default ctors to boost Frameworks code coverage
        /// </summary>
        [Fact]
        public void EventArgsCtors()
        {
            ProjectFinishedEventArgs projectFinishedEvent = new ProjectFinishedEventArgs2();
            projectFinishedEvent = new ProjectFinishedEventArgs("Message", "HelpKeyword", "ProjectFile", true);
            projectFinishedEvent = new ProjectFinishedEventArgs("Message", "HelpKeyword", "ProjectFile", true, DateTime.Now);
            projectFinishedEvent = new ProjectFinishedEventArgs(null, null, null, true);
            projectFinishedEvent = new ProjectFinishedEventArgs(null, null, null, true, DateTime.Now);
        }

        /// <summary>
        /// Create a derived class so that we can test the default constructor in order to increase code coverage and 
        /// verify this code path does not cause any exceptions.
        /// </summary>
        private class ProjectFinishedEventArgs2 : ProjectFinishedEventArgs
        {
            /// <summary>
            /// Default constructor
            /// </summary>
            public ProjectFinishedEventArgs2()
                : base()
            {
            }
        }
    }
}
