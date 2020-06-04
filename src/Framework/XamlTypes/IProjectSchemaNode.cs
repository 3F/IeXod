// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace net.r_eg.IeXod.Framework.XamlTypes
{
    /// <summary>
    /// Interface that we expect all root classes from project schema XAML files to implement
    /// </summary>
    public interface IProjectSchemaNode
    {
        /// <summary>
        /// Return all types of static data for data driven features this node contains
        /// </summary>
        IEnumerable<Type> GetSchemaObjectTypes();

        /// <summary>
        /// Returns all instances of static data with Type "type".  Null or Empty list if there is no objects from asked type provided by this node
        /// </summary>
        IEnumerable<object> GetSchemaObjects(Type type);
    }
}
