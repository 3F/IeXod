// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace net.r_eg.IeXod.Evaluation
{
    /// <summary>
    /// Represents the parent of a ProjectMetadata object -
    /// either a ProjectItem or a ProjectItemDefinition.
    /// </summary>
    internal interface IProjectMetadataParent : IMetadataTable
    {
        /// <summary>
        /// The owning project
        /// </summary>
        Project Project
        {
            get;
        }

        /// <summary>
        /// The item type of the parent item definition or item.
        /// </summary>
        string ItemType
        {
            get;
        }
    }
}
