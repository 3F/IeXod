// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.Tasks;
using Xunit;

namespace net.r_eg.IeXod.UnitTests
{
    sealed public class AssemblyRegistrationCache_Tests
    {
        [Fact]
        public void ExerciseCache()
        {
            AssemblyRegistrationCache arc = new AssemblyRegistrationCache();

            Assert.Equal(0, arc.Count);

            arc.AddEntry("foo", "bar");

            Assert.Equal(1, arc.Count);

            string assembly;
            string tlb;
            arc.GetEntry(0, out assembly, out tlb);

            Assert.Equal("foo", assembly);
            Assert.Equal("bar", tlb);
        }
    }
}
