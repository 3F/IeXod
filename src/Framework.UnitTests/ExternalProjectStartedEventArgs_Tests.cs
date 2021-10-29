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
    /// Verify the functioning of the ExternalProjectStartedEventArgs class.
    /// </summary>
    public class ExternalProjectStartedEventArgs_Tests
    {
        /// <summary>
        /// Trivially exercise event args default ctors to boost Frameworks code coverage
        /// </summary>
        [Fact]
        public void EventArgsCtors()
        {
            ExternalProjectStartedEventArgs externalProjectStartedEvent = new ExternalProjectStartedEventArgs2();
            externalProjectStartedEvent = new ExternalProjectStartedEventArgs("Message", "HelpKeyword", "Sender", "ProjectFile", "TargetNames");
            externalProjectStartedEvent = new ExternalProjectStartedEventArgs("Message", "HelpKeyword", "Sender", "ProjectFile", "TargetNames", DateTime.Now);
            externalProjectStartedEvent = new ExternalProjectStartedEventArgs(null, null, null, null, null);
            externalProjectStartedEvent = new ExternalProjectStartedEventArgs(null, null, null, null, null, DateTime.Now);
        }

        /// <summary>
        /// Create a derived class so that we can test the default constructor in order to increase code coverage and 
        /// verify this code path does not cause any exceptions.
        /// </summary>
        private class ExternalProjectStartedEventArgs2 : ExternalProjectStartedEventArgs
        {
            /// <summary>
            /// Default constructor
            /// </summary>
            public ExternalProjectStartedEventArgs2() : base()
            {
            }
        }
    }
}
