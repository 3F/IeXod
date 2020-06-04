﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;

namespace net.r_eg.IeXod.Tasks
{
    /// <summary>
    /// Represents a cache of inputs to a compilation-style task.
    /// </summary>
    /// <remarks>On-disk serialization format, don't change field names or types or use readonly.</remarks>
    [Serializable]
    internal class Dependencies
    {
        /// <summary>
        /// Hashtable of other dependency files.
        /// Key is filename and value is DependencyFile.
        /// </summary>
        private Hashtable dependencies = new Hashtable();

        /// <summary>
        /// Look up a dependency file. Return null if its not there.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        internal DependencyFile GetDependencyFile(string filename)
        {
            return (DependencyFile)dependencies[filename];
        }

        /// <summary>
        /// Add a new dependency file.
        /// </summary>
        internal void AddDependencyFile(string filename, DependencyFile file)
        {
            dependencies[filename] = file;
        }

        /// <summary>
        /// Remove new dependency file.
        /// </summary>
        internal void RemoveDependencyFile(string filename)
        {
            dependencies.Remove(filename);
        }

        /// <summary>
        /// Remove all entries from the dependency table.
        /// </summary>
        internal void Clear()
        {
            dependencies.Clear();
        }
    }
}
