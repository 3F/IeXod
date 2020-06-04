// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Shared;
using net.r_eg.IeXod.Tasks;
using net.r_eg.IeXod.Utilities;
using Xunit;

namespace net.r_eg.IeXod.UnitTests
{
    sealed public class Touch_Tests
    {
        internal static net.r_eg.IeXod.Shared.FileExists fileExists = new net.r_eg.IeXod.Shared.FileExists(FileExists);
        internal static net.r_eg.IeXod.Shared.FileCreate fileCreate = new net.r_eg.IeXod.Shared.FileCreate(FileCreate);
        internal static net.r_eg.IeXod.Tasks.GetAttributes fileGetAttributes = new net.r_eg.IeXod.Tasks.GetAttributes(GetAttributes);
        internal static net.r_eg.IeXod.Tasks.SetAttributes fileSetAttributes = new net.r_eg.IeXod.Tasks.SetAttributes(SetAttributes);
        internal static net.r_eg.IeXod.Tasks.SetLastAccessTime setLastAccessTime = new net.r_eg.IeXod.Tasks.SetLastAccessTime(SetLastAccessTime);
        internal static net.r_eg.IeXod.Tasks.SetLastWriteTime setLastWriteTime = new net.r_eg.IeXod.Tasks.SetLastWriteTime(SetLastWriteTime);

        internal static string myexisting_txt = NativeMethodsShared.IsWindows ? @"c:\touch\myexisting.txt" : @"/touch/myexisting.txt";
        internal static string mynonexisting_txt = NativeMethodsShared.IsWindows ? @"c:\touch\mynonexisting.txt" : @"/touch/mynonexisting.txt";
        internal static string nonexisting_txt = NativeMethodsShared.IsWindows ? @"c:\touch-nonexistent\file.txt" : @"/touch-nonexistent/file.txt";
        internal static string myreadonly_txt = NativeMethodsShared.IsWindows ? @"c:\touch\myreadonly.txt" : @"/touch/myreadonly.txt";

        private bool Execute(Touch t)
        {
            return t.ExecuteImpl
            (
                fileExists,
                fileCreate,
                fileGetAttributes,
                fileSetAttributes,
                setLastAccessTime,
                setLastWriteTime
            );
        }

        /// <summary>
        /// Mock file exists.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool FileExists(string path)
        {
            if (path == myexisting_txt)
            {
                return true;
            }

            if (path == mynonexisting_txt)
            {
                return false;
            }

            if (path == nonexisting_txt)
            {
                return false;
            }

            if (path == myreadonly_txt)
            {
                return true;
            }
            Assert.True(false, "Unexpected file exists: " + path);

            return true;
        }

        /// <summary>
        /// Mock file create.
        /// </summary>
        /// <param name="path"></param>
        private static FileStream FileCreate(string path)
        {
            if (path == mynonexisting_txt)
            {
                return null;
            }

            if (path == nonexisting_txt)
            {
                throw new DirectoryNotFoundException();
            }


            Assert.True(false, "Unexpected file create: " + path);
            return null;
        }

        /// <summary>
        /// Mock get attributes.
        /// </summary>
        /// <param name="path"></param>
        private static FileAttributes GetAttributes(string path)
        {
            FileAttributes a = new FileAttributes();
            if (path == myexisting_txt)
            {
                return a;
            }

            if (path == mynonexisting_txt)
            {
                // Has attributes because Touch created it.
                return a;
            }

            if (path == myreadonly_txt)
            {
                a = System.IO.FileAttributes.ReadOnly;
                return a;
            }

            Assert.True(false, "Unexpected file attributes: " + path);
            return a;
        }

        /// <summary>
        /// Mock get attributes.
        /// </summary>
        /// <param name="path"></param>
        private static void SetAttributes(string path, FileAttributes attributes)
        {
            if (path == myreadonly_txt)
            {
                return;
            }
            Assert.True(false, "Unexpected set file attributes: " + path);
        }

        /// <summary>
        /// Mock SetLastAccessTime.
        /// </summary>
        /// <param name="path"></param>
        private static void SetLastAccessTime(string path, DateTime timestamp)
        {
            if (path == myexisting_txt)
            {
                return;
            }

            if (path == mynonexisting_txt)
            {
                return;
            }

            if (path == myreadonly_txt)
            {
                // Read-only so throw an exception
                throw new IOException();
            }

            Assert.True(false, "Unexpected set last access time: " + path);
        }

        /// <summary>
        /// Mock SetLastWriteTime.
        /// </summary>
        /// <param name="path"></param>
        private static void SetLastWriteTime(string path, DateTime timestamp)
        {
            if (path == myexisting_txt)
            {
                return;
            }

            if (path == mynonexisting_txt)
            {
                return;
            }

            if (path == myreadonly_txt)
            {
                return;
            }


            Assert.True(false, "Unexpected set last write time: " + path);
        }

        [Fact]
        public void TouchExisting()
        {
            Touch t = new Touch();
            MockEngine engine = new MockEngine();
            t.BuildEngine = engine;

            t.Files = new ITaskItem[]
            {
                new TaskItem(myexisting_txt)
            };

            bool success = Execute(t);

            Assert.True(success);

            Assert.Single(t.TouchedFiles);

            Assert.Contains(
                String.Format(AssemblyResources.GetString("Touch.Touching"), myexisting_txt),
                engine.Log
            );
        }

        [Fact]
        public void TouchNonExisting()
        {
            Touch t = new Touch();
            MockEngine engine = new MockEngine();
            t.BuildEngine = engine;

            t.Files = new ITaskItem[]
            {
                new TaskItem(mynonexisting_txt)
            };

            bool success = Execute(t);

            // Not success because the file doesn't exist
            Assert.False(success);

            Assert.Contains(
                String.Format(AssemblyResources.GetString("Touch.FileDoesNotExist"), mynonexisting_txt),
                engine.Log
            );
        }

        [Fact]
        public void TouchNonExistingAlwaysCreate()
        {
            Touch t = new Touch();
            MockEngine engine = new MockEngine();
            t.BuildEngine = engine;
            t.AlwaysCreate = true;

            t.Files = new ITaskItem[]
            {
                new TaskItem(mynonexisting_txt)
            };

            bool success = Execute(t);

            // Success because the file was created.
            Assert.True(success);

            Assert.Contains(
                String.Format(AssemblyResources.GetString("Touch.CreatingFile"), mynonexisting_txt, "AlwaysCreate"),
                engine.Log
            );
        }

        [Fact]
        public void TouchNonExistingAlwaysCreateAndBadlyFormedTimestamp()
        {
            Touch t = new Touch();
            MockEngine engine = new MockEngine();
            t.BuildEngine = engine;
            t.AlwaysCreate = true;
            t.ForceTouch = false;
            t.Time = "Badly formed time String.";

            t.Files = new ITaskItem[]
            {
                new TaskItem(mynonexisting_txt)
            };

            bool success = Execute(t);

            // Failed because of badly formed time string.
            Assert.False(success);

            Assert.Contains("MSB3376", engine.Log);
        }

        [Fact]
        public void TouchReadonly()
        {
            Touch t = new Touch();
            MockEngine engine = new MockEngine();
            t.BuildEngine = engine;
            t.AlwaysCreate = true;

            t.Files = new ITaskItem[]
            {
                new TaskItem(myreadonly_txt)
            };

            bool success = Execute(t);

            // Failed because file is readonly.
            Assert.False(success);

            Assert.Contains("MSB3374", engine.Log);
            Assert.Contains(myreadonly_txt, engine.Log);
        }

        [Fact]
        public void TouchReadonlyForce()
        {
            Touch t = new Touch();
            MockEngine engine = new MockEngine();
            t.BuildEngine = engine;
            t.ForceTouch = true;
            t.AlwaysCreate = true;

            t.Files = new ITaskItem[]
            {
                new TaskItem(myreadonly_txt)
            };

            Execute(t);
        }

        [Fact]
        public void TouchNonExistingDirectoryDoesntExist()
        {
            Touch t = new Touch();
            MockEngine engine = new MockEngine();
            t.BuildEngine = engine;
            t.AlwaysCreate = true;

            t.Files = new ITaskItem[]
            {
                new TaskItem(nonexisting_txt)
            };

            bool success = Execute(t);

            // Failed because the target directory didn't exist.
            Assert.False(success);

            Assert.Contains("MSB3371", engine.Log);
            Assert.Contains(nonexisting_txt, engine.Log);
        }
    }
}



