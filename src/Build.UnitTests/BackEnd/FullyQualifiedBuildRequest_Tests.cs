// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.BackEnd;
using net.r_eg.IeXod.Execution;
using net.r_eg.IeXod.Evaluation;
using Xunit;

namespace net.r_eg.IeXod.UnitTests.BackEnd
{
    public class FullyQualifiedBuildRequest_Tests
    {
        [Fact]
        public void TestConstructorGood()
        {
            BuildRequestData data1 = new BuildRequestData("foo", new Dictionary<string, string>(), "tools", new string[0], null);
            FullyQualifiedBuildRequest request = new FullyQualifiedBuildRequest(new BuildRequestConfiguration(data1, "2.0"), new string[1] { "foo" }, true);

            request = new FullyQualifiedBuildRequest(new BuildRequestConfiguration(data1, "2.0"), new string[0] { }, true);

            BuildRequestData data3 = new BuildRequestData("foo", new Dictionary<string, string>(), "tools", new string[0], null);
            request = new FullyQualifiedBuildRequest(new BuildRequestConfiguration(data1, "2.0"), new string[0] { }, false);
        }

        [Fact]
        public void TestConstructorBad1()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                FullyQualifiedBuildRequest request = new FullyQualifiedBuildRequest(null, new string[1] { "foo" }, true);
            }
           );
        }

        [Fact]
        public void TestConstructorBad2()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                FullyQualifiedBuildRequest request = new FullyQualifiedBuildRequest(new BuildRequestConfiguration(new BuildRequestData("foo", new Dictionary<string, string>(), "tools", new string[0], null), "2.0"), null, true);
            }
           );
        }
        [Fact]
        public void TestProperties()
        {
            BuildRequestData data = new BuildRequestData("foo", new Dictionary<string, string>(), "tools", new string[0], null);
            BuildRequestConfiguration config = new BuildRequestConfiguration(data, "2.0");
            FullyQualifiedBuildRequest request = new FullyQualifiedBuildRequest(config, new string[1] { "foo" }, true);
            Assert.Equal(request.Config, config);
            Assert.Single(request.Targets);
            Assert.Equal("foo", request.Targets[0]);
            Assert.True(request.ResultsNeeded);
        }
    }
}