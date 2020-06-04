// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


using System.Linq;
using net.r_eg.IeXod.Globbing;
using Xunit;

namespace net.r_eg.IeXod.Engine.UnitTests.Globbing
{
    public class MSBuildGlobWithGaps_Tests
    {
        [Fact]
        public void GlobWithGapsShoulWorkWithNoGaps()
        {
            var glob = new MSBuildGlobWithGaps(MSBuildGlob.Parse("a*"), Enumerable.Empty<IMSBuildGlob>());

            Assert.True(glob.IsMatch("ab"));
        }

        [Fact]
        public void GlobWithGapsShoulMatchIfNoGapsMatch()
        {
            var glob = new MSBuildGlobWithGaps(MSBuildGlob.Parse("a*"), MSBuildGlob.Parse("b*"));

            Assert.True(glob.IsMatch("ab"));
        }

        [Fact]
        public void GlobWithGapsShoulNotMatchIfGapsMatch()
        {
            var glob = new MSBuildGlobWithGaps(MSBuildGlob.Parse("a*"), MSBuildGlob.Parse("*b"));

            Assert.False(glob.IsMatch("ab"));
        }
    }
}