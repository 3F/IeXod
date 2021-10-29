// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using net.r_eg.IeXod.Collections;
using net.r_eg.IeXod.Construction;

namespace net.r_eg.IeXod.Evaluation
{
    /// <summary>
    /// This interface represents an item without exposing its type.
    /// It's convenient to not genericise the base interface, to make it easier to use
    /// for the majority of code that doesn't call these methods.
    /// </summary>
    /// <typeparam name="M">Type of metadata object.</typeparam>
    internal interface IItem<M> : IItem
        where M : class, IMetadatum
    {
        /// <summary>
        /// Gets any existing metadatum on the item, or
        /// else any on an applicable item definition.
        /// </summary>
        M GetMetadata(string name);

        /// <summary>
        /// Sets the specified metadata.
        /// Predecessor is any preceding overridden metadata
        /// </summary>
        M SetMetadata(ProjectMetadataElement metadataElement, string evaluatedValue);
    }
}
