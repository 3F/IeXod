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
    /// Verify the functioning of the BuildWarningEventArgs class.
    /// </summary>
    public class BuildWarningEventArgs_Tests
    {
        /// <summary>
        /// Default event to use in tests.
        /// </summary>
        private BuildWarningEventArgs _baseWarningEvent = new BuildWarningEventArgs("Subcategory", "Code", "File", 1, 2, 3, 4, "Message", "HelpKeyword", "sender");

        /// <summary>
        /// Trivially exercise event args default ctors to boost Frameworks code coverage
        /// </summary>
        [Fact]
        public void EventArgsCtors()
        {
            BuildWarningEventArgs buildWarningEvent = new BuildWarningEventArgs2();
            buildWarningEvent = new BuildWarningEventArgs("Subcategory", "Code", "File", 1, 2, 3, 4, "Message", "HelpKeyword", "sender");
            buildWarningEvent = new BuildWarningEventArgs("Subcategory", "Code", "File", 1, 2, 3, 4, "Message", "HelpKeyword", "sender", DateTime.Now);
            buildWarningEvent = new BuildWarningEventArgs("Subcategory", "Code", "File", 1, 2, 3, 4, "{0}", "HelpKeyword", "sender", DateTime.Now, "Message");
            buildWarningEvent = new BuildWarningEventArgs(null, null, null, 1, 2, 3, 4, null, null, null);
            buildWarningEvent = new BuildWarningEventArgs(null, null, null, 1, 2, 3, 4, null, null, null, DateTime.Now);
            buildWarningEvent = new BuildWarningEventArgs(null, null, null, 1, 2, 3, 4, null, null, null, DateTime.Now, null);
        }

        /// <summary>
        /// Trivially exercise getHashCode.
        /// </summary>
        [Fact]
        public void TestGetHashCode()
        {
            _baseWarningEvent.GetHashCode();
        }

        /// <summary>
        /// Create a derived class so that we can test the default constructor in order to increase code coverage and 
        /// verify this code path does not cause any exceptions.
        /// </summary>
        private class BuildWarningEventArgs2 : BuildWarningEventArgs
        {
            /// <summary>
            /// Default constructor
            /// </summary>
            public BuildWarningEventArgs2()
                : base()
            {
            }
        }
    }
}