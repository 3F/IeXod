﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace net.r_eg.IeXod.Framework
{
    /// <summary>
    /// This interface adds escaping support to the ITaskItem interface.
    /// </summary>
    [ComVisible(true)]
    [Guid("ac6d5a59-f877-461b-88e3-b2f06fce0cb9")]
    public interface ITaskItem2 : ITaskItem
    {
        /// <summary>
        /// Gets or sets the item include value e.g. for disk-based items this would be the file path.
        /// </summary>
        /// <remarks>
        /// Taking the opportunity to fix the property name, although this doesn't
        /// make it obvious it's an improvement on ItemSpec.
        /// </remarks>
        string EvaluatedIncludeEscaped
        {
            get;
            set;
        }

        /// <summary>
        /// Allows the values of metadata on the item to be queried.
        /// </summary>
        /// <remarks>
        /// Taking the opportunity to fix the property name, although this doesn't
        /// make it obvious it's an improvement on GetMetadata.
        /// </remarks>
        string GetMetadataValueEscaped(string metadataName);

        /// <summary>
        /// Allows a piece of custom metadata to be set on the item.  Assumes that the value passed
        /// in is unescaped, and escapes the value as necessary in order to maintain its value. 
        /// </summary>
        /// <remarks>
        /// Taking the opportunity to fix the property name, although this doesn't
        /// make it obvious it's an improvement on SetMetadata.
        /// </remarks>
        void SetMetadataValueLiteral(string metadataName, string metadataValue);

        /// <summary>
        /// ITaskItem2 implementation which returns a clone of the metadata on this object.
        /// Values returned are in their original escaped form. 
        /// </summary>
        /// <returns>The cloned metadata, with values' escaping preserved.</returns>
        IDictionary CloneCustomMetadataEscaped();
    }
}
