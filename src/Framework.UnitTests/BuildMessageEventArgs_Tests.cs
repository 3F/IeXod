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
    /// Verify the functioning of the BuildMessageEventArgs class.
    /// </summary>
    public class BuildMessageEventArgs_Tests
    {
        /// <summary>
        /// Trivially exercise event args default ctors to boost Frameworks code coverage
        /// </summary>
        [Fact]
        public void EventArgsCtors()
        {
            BuildMessageEventArgs bmea = new BuildMessageEventArgs2();
            bmea = new BuildMessageEventArgs("Message", "HelpKeyword", "Sender", MessageImportance.Low);
            bmea = new BuildMessageEventArgs("Message", "HelpKeyword", "Sender", MessageImportance.Low, DateTime.Now);
            bmea = new BuildMessageEventArgs("{0}", "HelpKeyword", "Sender", MessageImportance.Low, DateTime.Now, "Message");
            bmea = new BuildMessageEventArgs("Subcategory", "Code", "File", 1, 2, 3, 4, "Message", "HelpKeyword", "Sender", MessageImportance.Low);
            bmea = new BuildMessageEventArgs("Subcategory", "Code", "File", 1, 2, 3, 4, "Message", "HelpKeyword", "Sender", MessageImportance.Low, DateTime.Now);
            bmea = new BuildMessageEventArgs("Subcategory", "Code", "File", 1, 2, 3, 4, "{0}", "HelpKeyword", "Sender", MessageImportance.Low, DateTime.Now, "Message");
            bmea = new BuildMessageEventArgs(null, null, null, MessageImportance.Low);
            bmea = new BuildMessageEventArgs(null, null, null, MessageImportance.Low, DateTime.Now);
            bmea = new BuildMessageEventArgs(null, null, null, MessageImportance.Low, DateTime.Now, null);
            bmea = new BuildMessageEventArgs(null, null, null, 0, 0, 0, 0, null, null, null, MessageImportance.Low);
            bmea = new BuildMessageEventArgs(null, null, null, 0, 0, 0, 0, null, null, null, MessageImportance.Low, DateTime.Now);
            bmea = new BuildMessageEventArgs(null, null, null, 0, 0, 0, 0, null, null, null, MessageImportance.Low, DateTime.Now, null);
        }

        /// <summary>
        /// Create a derived class so that we can test the default constructor in order to increase code coverage and 
        /// verify this code path does not cause any exceptions.
        /// </summary>
        private class BuildMessageEventArgs2 : BuildMessageEventArgs
        {
            /// <summary>
            /// Default constructor
            /// </summary>
            public BuildMessageEventArgs2()
                : base()
            {
            }
        }
    }
}
