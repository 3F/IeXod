// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

namespace net.r_eg.IeXod.Shared.FileSystem
{
    /// <summary>
    /// Legacy implementation for MSBuildTaskHost which is stuck on net20 APIs
    /// </summary>
    internal class MSBuildTaskHostFileSystem : IFileSystem
    {
        private static readonly MSBuildTaskHostFileSystem Instance = new MSBuildTaskHostFileSystem();

        public static MSBuildTaskHostFileSystem Singleton() => Instance;

        public bool DirectoryEntryExists(string path)
        {
            return NativeMethodsShared.FileOrDirectoryExists(path);
        }

        public bool DirectoryExists(string path)
        {
            return NativeMethodsShared.DirectoryExists(path);
        }

        public IEnumerable<string> EnumerateDirectories(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return Directory.GetDirectories(path, searchPattern, searchOption);
        }

        public IEnumerable<string> EnumerateFiles(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return Directory.GetFiles(path, searchPattern, searchOption);
        }

        public IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            ErrorUtilities.VerifyThrow(searchOption == SearchOption.TopDirectoryOnly, $"In net20 {nameof(Directory.GetFileSystemEntries)} does not take a {nameof(SearchOption)} parameter");

            return Directory.GetFileSystemEntries(path, searchPattern);
        }

        public bool FileExists(string path)
        {
            return NativeMethodsShared.FileExists(path);
        }
    }
}
