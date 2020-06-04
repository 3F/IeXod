﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace net.r_eg.IeXod.Framework
{
    /// <summary>
    /// The argument for a property initial value set event.
    /// </summary>
    [Serializable]
    public class PropertyInitialValueSetEventArgs : BuildMessageEventArgs
    {
        /// <summary>
        /// Creates an instance of the <see cref="PropertyInitialValueSetEventArgs"/> class.
        /// </summary>
        public PropertyInitialValueSetEventArgs() { }

        /// <summary>
        /// Creates an instance of the <see cref="PropertyInitialValueSetEventArgs"/> class.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="propertyValue">The value of the property.</param>
        /// <param name="propertySource">The source of the property.</param>
        public PropertyInitialValueSetEventArgs(
            string propertyName,
            string propertyValue,
            string propertySource,
            string message,
            string helpKeyword = null,
            string senderName = null,
            MessageImportance importance = MessageImportance.Low) : base(message, helpKeyword, senderName, importance)
        {
            this.PropertyName = propertyName;
            this.PropertyValue = propertyValue;
            this.PropertySource = propertySource;
        }

        /// <summary>
        /// The name of the property.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// The value of the property.
        /// </summary>
        public string PropertyValue { get; set; }

        /// <summary>
        /// The source of the property.
        /// </summary>
        public string PropertySource { get; set; }
    }
}
