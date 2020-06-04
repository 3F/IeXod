// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using net.r_eg.IeXod.BuildEngine;

namespace net.r_eg.IeXod.BuildEngine
{
    /// <summary>
    /// An enumerable wrapper for a hashtable-by-name of BuildItemGroups that allows read-only 
    /// access to the items.
    /// </summary>
    /// <remarks>
    /// This class is designed to be passed to loggers.
    /// The expense of copying items is only incurred if and when 
    /// a logger chooses to enumerate over it.
    /// </remarks>
    /// <owner>danmose</owner>
    internal class BuildItemGroupProxy : IEnumerable
    {
        // Item group that this proxies
        private BuildItemGroup backingItemGroup;

        private BuildItemGroupProxy()
        { 
            // Do nothing
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="itemGroup">Item group this class should proxy</param>
        public BuildItemGroupProxy(BuildItemGroup itemGroup)
        {
            this.backingItemGroup = itemGroup;
        }

        /// <summary>
        /// Returns an enumerator that provides copies of the items
        /// in the backing item group.
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            foreach (BuildItem item in backingItemGroup)
            {
                yield return new DictionaryEntry(item.Name, new TaskItem(item));
            }
        }
    }
}
