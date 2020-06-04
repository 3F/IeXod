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
    /// An enumerable wrapper for a BuildPropertyGroup that allows read-only 
    /// access to the properties.
    /// </summary>
    /// <remarks>
    /// This class is designed to be passed to loggers.
    /// The expense of copying properties is only incurred if and when 
    /// a logger chooses to enumerate over it.
    /// </remarks>
    /// <owner>danmose</owner>
    internal class BuildPropertyGroupProxy : IEnumerable
    {
        // Property group that this proxies
        private BuildPropertyGroup backingPropertyGroup;

        private BuildPropertyGroupProxy()
        { 
            // Do nothing
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="propertyGroup">Property group this class should proxy</param>
        public BuildPropertyGroupProxy(BuildPropertyGroup propertyGroup)
        {
            this.backingPropertyGroup = propertyGroup;
        }

        /// <summary>
        /// Returns an enumerator that provides copies of the property name-value pairs
        /// in the backing property group.
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            foreach (BuildProperty prop in backingPropertyGroup)
            {
                // No need to clone the property; just return copies of the name and value
                yield return new DictionaryEntry(prop.Name, prop.FinalValue);
            }
        }
    }
}
