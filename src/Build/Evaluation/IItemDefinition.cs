// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using net.r_eg.IeXod.Construction;

namespace net.r_eg.IeXod.Evaluation
{
    /// <summary>
    /// Interface representing item definition objects for use by the Evaulator.
    /// </summary>
    /// <typeparam name="M">Type of metadata objects.</typeparam>
    internal interface IItemDefinition<M> : IMetadataTable
        where M : class, IMetadatum
    {
        /// <summary>
        /// Gets any metadatum on this item definition with the specified name.
        /// </summary>
        M GetMetadata(string name);

        /// <summary>
        /// Adds the specified metadata to the item definition.
        /// </summary>
        M SetMetadata(ProjectMetadataElement metadataElement, string evaluatedValue, M predecessor);
    }
}
