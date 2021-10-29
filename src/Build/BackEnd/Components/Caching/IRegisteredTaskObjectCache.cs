// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.Framework;

#if BUILD_ENGINE
namespace net.r_eg.IeXod.BackEnd.Components.Caching
#else
namespace net.r_eg.IeXod.Shared
#endif
{
    /// <summary>
    /// Defines a cache for registered task objects.
    /// </summary>
    internal interface IRegisteredTaskObjectCache
    {
        /// <summary>
        /// Disposes of all of the objects with the specified lifetime.
        /// </summary>
        void DisposeCacheObjects(RegisteredTaskObjectLifetime lifetime);

        /// <summary>
        /// Registers a task object with the specified key and lifetime.
        /// </summary>
        void RegisterTaskObject(object key, object obj, RegisteredTaskObjectLifetime lifetime, bool allowEarlyCollection);

        /// <summary>
        /// Gets a previously registered task object.
        /// </summary>
        object GetRegisteredTaskObject(object key, RegisteredTaskObjectLifetime lifetime);

        /// <summary>
        /// Unregisters a task object.
        /// </summary>
        object UnregisterTaskObject(object key, RegisteredTaskObjectLifetime lifetime);
    }
}
