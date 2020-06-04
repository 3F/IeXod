// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.Construction;

namespace net.r_eg.IeXod.ObjectModelRemoting
{
    /// <summary>
    /// External projects support.
    /// Allow for creating a local representation to external object of type <see cref="ProjectPropertyElement"/>
    /// </summary>
    public abstract class ProjectPropertyElementLink : ProjectElementLink
    {
        /// <summary>
        /// Access to remote <see cref="ProjectMetadataElement.Value"/>.
        /// </summary>
        public abstract string Value { get; set; }

        /// <summary>
        /// Help implement rename.
        /// </summary>
        public abstract void ChangeName(string newName);
    }
}
