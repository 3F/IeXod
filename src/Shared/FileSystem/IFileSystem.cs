﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;

namespace net.r_eg.IeXod.Shared.FileSystem
{
    /// <summary>
    /// Abstracts away some file system operations
    /// </summary>
    internal interface IFileSystem
    {
        /// <summary>
        /// Returns an enumerable collection of file names that match a search pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        IEnumerable<string> EnumerateFiles(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly);

        /// <summary>
        /// Returns an enumerable collection of directory names that match a search pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        IEnumerable<string> EnumerateDirectories(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly);

        /// <summary>
        /// Returns an enumerable collection of file names and directory names that match a search pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly);

        /// <summary>
        /// Determines whether the given path refers to an existing directory on disk.
        /// </summary>
        bool DirectoryExists(string path);

        /// <summary>
        /// Determines whether the given path refers to an existing file on disk.
        /// </summary>
        bool FileExists(string path);

        /// <summary>
        /// Determines whether the given path refers to an existing entry in the directory service.
        /// </summary>
        bool DirectoryEntryExists(string path);
    }
}
