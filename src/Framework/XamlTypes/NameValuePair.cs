// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace net.r_eg.IeXod.Framework.XamlTypes
{
    /// <summary>
    /// Represents a name-value pair. The name cannot be null or empty.
    /// </summary>
    public class NameValuePair
    {
        #region Constructor

        /// <summary>
        /// Default constructor needed for 
        /// </summary>
        public NameValuePair()
        {
        }

        #endregion // Constructor

        #region Properties

        /// <summary>
        /// The name.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// The value.
        /// </summary>
        [Localizable(true)]
        public string Value
        {
            get;
            set;
        }

        #endregion // Properties
    }
}
