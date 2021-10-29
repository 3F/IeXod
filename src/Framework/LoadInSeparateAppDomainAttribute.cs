// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace net.r_eg.IeXod.Framework
{
    /// <summary>
    /// This attribute is used to mark tasks that need to be run in their own app domains. The build engine will create a new app
    /// domain each time it needs to run such a task, and immediately unload it when the task is finished.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class LoadInSeparateAppDomainAttribute : Attribute
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public LoadInSeparateAppDomainAttribute()
        {
            // do nothing
        }
    }
}
