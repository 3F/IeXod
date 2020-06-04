﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Markup;

namespace net.r_eg.IeXod.Framework.XamlTypes
{
    /// <summary>
    /// Used to deserialize the item type information 
    /// </summary>
    public sealed class ItemType : ISupportInitialize, IProjectSchemaNode
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ItemType()
        {
            // by default it is included in up-to-date check
            UpToDateCheckInput = true;
        }

        /// <summary>
        /// serializes IItemType.Name
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// serializes IItemType.DisplayName
        /// </summary>
        [Localizable(true)]
        public string DisplayName
        {
            get;
            set;
        }

        /// <summary>
        /// serializes IItemType.ItemType
        /// </summary>
        public string DefaultContentType
        {
            get;
            set;
        }

        /// <summary>
        /// serializes IItemType.UpToDateCheckInput
        /// </summary>
        public bool UpToDateCheckInput
        {
            get;
            set;
        }

        #region ISupportInitialize Members

        /// <summary>
        /// See ISupportInitialize.
        /// </summary>
        public void BeginInit()
        {
        }

        /// <summary>
        /// See ISupportInitialize.
        /// </summary>
        public void EndInit()
        {
        }

        #endregion

        #region IProjectSchemaNode Members
        /// <summary>
        /// see IProjectSchemaNode
        /// </summary>
        public IEnumerable<Type> GetSchemaObjectTypes()
        {
            yield return typeof(ItemType);
        }

        /// <summary>
        /// see IProjectSchemaNode
        /// </summary>
        public IEnumerable<object> GetSchemaObjects(Type type)
        {
            if (type == typeof(ItemType))
            {
                yield return this;
            }
        }
        #endregion
    }
}
