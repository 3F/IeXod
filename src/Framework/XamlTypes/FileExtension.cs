// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace net.r_eg.IeXod.Framework.XamlTypes
{
    /// <summary>
    /// simple class that deserialize extension to content type data
    /// </summary>
    public sealed class FileExtension : IProjectSchemaNode
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FileExtension()
        {
        }

        /// <summary>
        /// file extension 
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// corresponding content type
        /// </summary>
        public string ContentType
        {
            get;
            set;
        }

        #region IProjectSchemaNode Members

        /// <summary>
        /// see IProjectSchemaNode
        /// </summary>
        public IEnumerable<Type> GetSchemaObjectTypes()
        {
            yield return typeof(FileExtension);
        }

        /// <summary>
        /// see IProjectSchemaNode
        /// </summary>
        public IEnumerable<object> GetSchemaObjects(Type type)
        {
            if (type == typeof(FileExtension))
            {
                yield return this;
            }
        }

        #endregion
    }
}
