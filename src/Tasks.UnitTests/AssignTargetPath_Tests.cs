// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Shared;
using net.r_eg.IeXod.Tasks;
using net.r_eg.IeXod.Utilities;
using Xunit;

namespace net.r_eg.IeXod.UnitTests
{
    sealed public class AssignTargetPath_Tests
    {
        [Fact]
        public void Regress314791()
        {
            AssignTargetPath t = new AssignTargetPath();
            t.BuildEngine = new MockEngine();
            t.Files = new ITaskItem[]
                          { new TaskItem(NativeMethodsShared.IsWindows ? @"c:\bin2\abc.efg" : "/bin2/abc.efg") };
            t.RootFolder = NativeMethodsShared.IsWindows ? @"c:\bin" : "/bin";

            bool success = t.Execute();

            Assert.True(success);

            Assert.Single(t.AssignedFiles);
            Assert.Equal(
                NativeMethodsShared.IsWindows ? @"c:\bin2\abc.efg" : "/bin2/abc.efg",
                t.AssignedFiles[0].ItemSpec);
            Assert.Equal(@"abc.efg", t.AssignedFiles[0].GetMetadata("TargetPath"));
        }

        [Fact]
        public void AtConeRoot()
        {
            AssignTargetPath t = new AssignTargetPath();
            t.BuildEngine = new MockEngine();
            t.Files = new ITaskItem[]
                          { new TaskItem(NativeMethodsShared.IsWindows ? @"c:\f1\f2\file.txt" : "/f1/f2/file.txt") };
            t.RootFolder = NativeMethodsShared.IsWindows ? @"c:\f1\f2" : "/f1/f2";

            bool success = t.Execute();

            Assert.True(success);

            Assert.Single(t.AssignedFiles);
            Assert.Equal(@"file.txt", t.AssignedFiles[0].GetMetadata("TargetPath"));
        }

        [Fact]
        public void OutOfCone()
        {
            AssignTargetPath t = new AssignTargetPath();
            t.BuildEngine = new MockEngine();
            t.Files = new ITaskItem[]
                          {
                              new TaskItem(
                                  NativeMethodsShared.IsWindows ? @"d:\f1\f2\f3\f4\file.txt" : "/f1/f2/f3/f4/file.txt")
                          };
            // Create a path that's outside of the cone create above. On Windows this is achieved by
            // changing the drive letter from d:\ to c:\ to make sure the result is out of the cone.
            // If not Windows, where there is no drive, this is dine by changes the root directory from
            // /f1 to /x1
            t.RootFolder = NativeMethodsShared.IsWindows ? @"c:\f1" : "/x1";

            bool success = t.Execute();

            Assert.True(success);

            Assert.Single(t.AssignedFiles);
            Assert.Equal("file.txt", t.AssignedFiles[0].GetMetadata("TargetPath"));
        }

        [Fact]
        public void InConeButAbsolute()
        {
            AssignTargetPath t = new AssignTargetPath();
            t.BuildEngine = new MockEngine();
            t.Files = new ITaskItem[]
                          {
                              new TaskItem(
                                  NativeMethodsShared.IsWindows ? @"c:\f1\f2\f3\f4\file.txt" : "/f1/f2/f3/f4/file.txt")
                          };
            t.RootFolder = NativeMethodsShared.IsWindows ? @"c:\f1\f2" : "/f1/f2";

            bool success = t.Execute();

            Assert.True(success);

            Assert.Single(t.AssignedFiles);
            Assert.Equal(
                NativeMethodsShared.IsWindows ? @"f3\f4\file.txt" : "f3/f4/file.txt",
                t.AssignedFiles[0].GetMetadata("TargetPath"));
        }
    }
}



