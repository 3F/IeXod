// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace net.r_eg.IeXod.Framework.XamlTypes
{
    /// <summary>
    /// Represents the schema of a string property.
    /// </summary>
    public sealed class StringProperty : BaseProperty
    {
        #region Properties

        /// <summary>
        /// Qualifies this string property to give it a more specific classification.
        /// </summary>
        /// <remarks>
        /// The value this field is set to, must be understood by the consumer of this field
        /// (normally a UI renderer).
        /// </remarks>
        /// <example> The value of this property can be set to, say, "File", "Folder", "CarModel" etc. to specify
        /// if this is a file path, folder path, car model name etc. </example>
        public string Subtype
        {
            get;
            set;
        }

        #endregion
    }
}
