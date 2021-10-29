// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace net.r_eg.IeXod.Utilities
{
    /// <summary>
    /// Type of SDK
    /// </summary>
    public enum SDKType
    {
        /// <summary>
        /// Not specified
        /// </summary>
        Unspecified,

        /// <summary>
        /// Traditional 3rd party SDK
        /// </summary>
        External,

        /// <summary>
        /// Platform extension SDK
        /// </summary>
        Platform,

        /// <summary>
        /// Framework extension SDK
        /// </summary>
        Framework
    }
}
