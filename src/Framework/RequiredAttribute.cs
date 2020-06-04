// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace net.r_eg.IeXod.Framework
{
    /// <summary>
    /// This class defines the attribute that a task writer can apply to a task's property to declare the property to be a
    /// required property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class RequiredAttribute : Attribute
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public RequiredAttribute()
        {
        }
    }
}
