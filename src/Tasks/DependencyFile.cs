﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

using net.r_eg.IeXod.Shared;
using net.r_eg.IeXod.Shared.FileSystem;

namespace net.r_eg.IeXod.Tasks
{
    /// <remarks>
    /// Represents a single input to a compilation-style task.
    /// Keeps track of timestamp for later comparison.
    /// 
    /// On-disk serialization format, don't change field names or types or use readonly.
    /// </remarks>
    [Serializable]
    internal class DependencyFile
    {
        // Filename
        private string filename;

        // Date and time the file was last modified           
        private DateTime lastModified;

        // Whether the file exists or not.
        private bool exists = false;

        /// <summary>
        /// The name of the file.
        /// </summary>
        internal string FileName
        {
            get { return filename; }
        }

        /// <summary>
        /// The last-modified timestamp when the class was instantiated.
        /// </summary>
        internal DateTime LastModified
        {
            get { return lastModified; }
        }

        /// <summary>
        /// Returns true if the file existed when this class was instantiated.
        /// </summary>
        internal bool Exists
        {
            get { return exists; }
        }

        /// <summary>
        /// Construct.
        /// </summary>
        /// <param name="filename">The file name.</param>
        internal DependencyFile(string filename)
        {
            this.filename = FileUtilities.FixFilePath(filename);

            if (FileSystems.Default.FileExists(FileName))
            {
                lastModified = File.GetLastWriteTime(FileName);
                exists = true;
            }
            else
            {
                exists = false;
            }
        }

        /// <summary>
        /// Checks whether the file has changed since the last time a timestamp was recorded.
        /// </summary>
        /// <returns></returns>
        internal bool HasFileChanged()
        {
            FileInfo info = FileUtilities.GetFileInfoNoThrow(filename);

            // Obviously if the file no longer exists then we are not up to date.
            if (info == null || !info.Exists)
            {
                return true;
            }

            // Check the saved timestamp against the current timestamp.
            // If they are different then obviously we are out of date.
            DateTime curLastModified = info.LastWriteTime;
            if (curLastModified != lastModified)
            {
                return true;
            }

            // All checks passed -- the info should still be up to date.
            return false;
        }
    }
}
