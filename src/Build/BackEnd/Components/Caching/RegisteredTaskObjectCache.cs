// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod.BackEnd.Components.Caching
{
    /// <summary>
    /// This is a cache of objects which are registered to be disposed of at a specified time.
    /// </summary>
    internal class RegisteredTaskObjectCache : RegisteredTaskObjectCacheBase, IBuildComponent, IRegisteredTaskObjectCache, IDisposable
    {
        /// <summary>
        /// Finalizer
        /// </summary>
        ~RegisteredTaskObjectCache()
        {
            Dispose(disposing: false);
        }

        #region IBuildComponent

        /// <summary>
        /// Initialize the build component.
        /// </summary>
        public void InitializeComponent(IBuildComponentHost host)
        {
        }

        /// <summary>
        /// Shuts down the build component.
        /// </summary>
        public void ShutdownComponent()
        {
            ErrorUtilities.VerifyThrow(IsCollectionEmptyOrUncreated(RegisteredTaskObjectLifetime.Build), "Build lifetime objects were not disposed at the end of the build");
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// Implementation of Dispose pattern.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>
        /// Component factory.
        /// </summary>
        internal static IBuildComponent CreateComponent(BuildComponentType type)
        {
            ErrorUtilities.VerifyThrow(type == BuildComponentType.RegisteredTaskObjectCache, "Cannot create components of type {0}", type);
            return new RegisteredTaskObjectCache();
        }

        /// <summary>
        /// Implementation of Dispose pattern.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ErrorUtilities.VerifyThrow(IsCollectionEmptyOrUncreated(RegisteredTaskObjectLifetime.Build), "Build lifetime objects were not disposed at the end of the build");
            }
        }
    }
}
