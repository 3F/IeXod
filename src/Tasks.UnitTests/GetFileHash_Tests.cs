// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using net.r_eg.IeXod.Tasks;
using net.r_eg.IeXod.Tasks.UnitTests.TestResources;
using net.r_eg.IeXod.Utilities;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace net.r_eg.IeXod.UnitTests
{
    public class GetFileHash_Tests
    {
        private readonly MockEngine _mockEngine;

        public GetFileHash_Tests(ITestOutputHelper output)
        {
            _mockEngine = new MockEngine(output);
        }

        [Fact]
        public void GetFileHash_FailsForUnknownAlgorithmName()
        {
            GetFileHash task = new GetFileHash
            {
                Files = new[] { new TaskItem(TestBinary.LoremFilePath) },
                BuildEngine = _mockEngine,
                Algorithm = "BANANA",
            };
            task.Execute().ShouldBeFalse();
            _mockEngine.Log.ShouldContain("MSB3953");
        }

        [Fact]
        public void GetFileHash_FailsForUnknownHashEncoding()
        {
            GetFileHash task = new GetFileHash
            {
                Files = new[] { new TaskItem(TestBinary.LoremFilePath) },
                BuildEngine = _mockEngine,
                HashEncoding = "blue",
            };
            task.Execute().ShouldBeFalse();
            _mockEngine.Log.ShouldContain("MSB3951");
        }

        [Fact]
        public void GetFileHash_FailsForMissingFile()
        {
            GetFileHash task = new GetFileHash
            {
                Files = new[] { new TaskItem(Path.Combine(AppContext.BaseDirectory, "this_does_not_exist.txt")) },
                BuildEngine = _mockEngine,
            };
            task.Execute().ShouldBeFalse();
            _mockEngine.Log.ShouldContain("MSB3954");
        }

        [Theory]
        [MemberData(nameof(TestBinary.GetLorem), MemberType = typeof(TestBinary))]
        public void GetFileHash_ComputesCorrectChecksumForOneFile(TestBinary testBinary)
        {
            GetFileHash task = new GetFileHash
            {
                Files = new[] { new TaskItem(testBinary.FilePath) },
                BuildEngine = _mockEngine,
                Algorithm = testBinary.HashAlgorithm,
                HashEncoding = testBinary.HashEncoding,
            };
            task.Execute().ShouldBeTrue();
            task.Hash.ShouldBe(testBinary.FileHash);
        }

        [Theory]
        [MemberData(nameof(TestBinary.GetLorem), MemberType = typeof(TestBinary))]
        public void GetFileHash_ComputesCorrectChecksumForManyFiles(TestBinary testBinary)
        {
            GetFileHash task = new GetFileHash
            {
                Files = new[]
                {
                    new TaskItem(testBinary.FilePath),
                    new TaskItem(testBinary.FilePath),
                },
                BuildEngine = _mockEngine,
                Algorithm = testBinary.HashAlgorithm,
                HashEncoding = testBinary.HashEncoding,
            };

            task.Execute().ShouldBeTrue();
            task.Items.Length.ShouldBe(2);
            task.Items.ShouldAllBe(i => string.Equals(testBinary.FileHash, i.GetMetadata("FileHash"), StringComparison.Ordinal));
        }
    }
}
