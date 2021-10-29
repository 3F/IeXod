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
    /// Verify the functioning of the TargetStartedEventArgs class.
    /// </summary>
    public class TargetStartedEventArgs_Tests
    {
        /// <summary>
        /// Trivially exercise event args default ctors to boost Frameworks code coverage
        /// </summary>
        [Fact]
        public void EventArgsCtors()
        {
            TargetStartedEventArgs targetStartedEvent = new TargetStartedEventArgs2();
            targetStartedEvent = new TargetStartedEventArgs("Message", "HelpKeyword", "TargetName", "ProjectFile", "TargetFile");
            targetStartedEvent = new TargetStartedEventArgs("Message", "HelpKeyword", "TargetName", "ProjectFile", "TargetFile", "ParentTarget", DateTime.Now);
            targetStartedEvent = new TargetStartedEventArgs("Message", "HelpKeyword", "TargetName", "ProjectFile", "TargetFile", "ParentTarget", TargetBuiltReason.AfterTargets, DateTime.Now);
            targetStartedEvent = new TargetStartedEventArgs(null, null, null, null, null);
            targetStartedEvent = new TargetStartedEventArgs(null, null, null, null, null, null, DateTime.Now);
            targetStartedEvent = new TargetStartedEventArgs(null, null, null, null, null, null, TargetBuiltReason.AfterTargets, DateTime.Now);
        }

        /// <summary>
        /// Create a derived class so that we can test the default constructor in order to increase code coverage and 
        /// verify this code path does not cause any exceptions.
        /// </summary>
        private class TargetStartedEventArgs2 : TargetStartedEventArgs
        {
            /// <summary>
            /// Default constructor
            /// </summary>
            public TargetStartedEventArgs2()
                : base()
            {
            }
        }
    }
}
