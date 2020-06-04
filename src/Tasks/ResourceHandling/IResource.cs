// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Resources;

namespace net.r_eg.IeXod.Tasks.ResourceHandling
{
    internal interface IResource
    {
        /// <summary>
        /// Name of the resource, as specified in the source.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The resource's type's assembly-qualified name. May be null when the type is not knowable from the source.
        /// </summary>
        string TypeAssemblyQualifiedName { get; }

        /// <summary>
        /// The resource's type's full name. May be null when the type is not knowable from the source.
        /// </summary>
        string TypeFullName { get; }

        /// <summary>
        /// Adds the resource represented by this object to the specified writer.
        /// </summary>
        void AddTo(IResourceWriter writer);
    }
}
