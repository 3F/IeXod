﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using net.r_eg.IeXod.Framework;
using Xunit;

namespace net.r_eg.IeXod.UnitTests
{
    /// <summary>
    /// Verify the functioning of the ExternalProjectFinishedEventArgs class.
    /// </summary>
    public class ExternalProjectFinishedEventArgs_Tests
    {
        /// <summary>
        /// Trivially exercise event args default ctors to boost Frameworks code coverage
        /// </summary>
        [Fact]
        public void EventArgsCtors()
        {
            ExternalProjectFinishedEventArgs externalProjectFinishedEvent = new ExternalProjectFinishedEventArgs2();
            externalProjectFinishedEvent = new ExternalProjectFinishedEventArgs("Message", "HelpKeyword", "Sender", "ProjectFile", true);
            externalProjectFinishedEvent = new ExternalProjectFinishedEventArgs("Message", "HelpKeyword", "Sender", "ProjectFile", true, DateTime.Now);
            externalProjectFinishedEvent = new ExternalProjectFinishedEventArgs(null, null, null, null, true);
            externalProjectFinishedEvent = new ExternalProjectFinishedEventArgs(null, null, null, null, true, DateTime.Now);
        }

        /// <summary>
        /// Create a derived class so that we can test the default constructor in order to increase code coverage and 
        /// verify this code path does not cause any exceptions.
        /// </summary>
        private class ExternalProjectFinishedEventArgs2 : ExternalProjectFinishedEventArgs
        {
            /// <summary>
            /// Default constructor
            /// </summary>
            public ExternalProjectFinishedEventArgs2()
                : base()
            {
            }
        }
    }
}
