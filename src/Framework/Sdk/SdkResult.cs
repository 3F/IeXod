// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace net.r_eg.IeXod.Sdk
{
    /// <summary>
    ///     An abstract interface class to indicate SDK resolver success or failure.
    ///     <remarks>
    ///         Note: Use <see cref="SdkResultFactory" /> to create instances of this class. Do not
    ///         inherit from this class.
    ///     </remarks>
    /// </summary>
    public abstract class SdkResult
    {
        /// <summary>
        ///     Indicates the resolution was successful.
        /// </summary>
        public virtual bool Success { get; protected set; }

        /// <summary>
        ///     Resolved path to the SDK.
        /// 
        ///     Null if <see cref="Success"/> == false
        /// </summary>
        public virtual string Path { get; protected set; }

        /// <summary>
        ///     Resolved version of the SDK.
        ///     Can be null or empty if the resolver did not provide a version (e.g. a path based resolver)
        /// 
        ///     Null if <see cref="Success"/> == false
        /// </summary>
        public virtual string Version { get; protected set; }

        /// <summary>
        ///     The Sdk reference
        /// </summary>
        public virtual SdkReference SdkReference { get; protected set; }
    }
}
