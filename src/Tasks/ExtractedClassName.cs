// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace net.r_eg.IeXod.Tasks
{
    /// <summary>
    /// Extracted class name from the source file.
    /// </summary>
    public struct ExtractedClassName
    {
        /// <summary>
        /// Whether or not we found the name inside a block of conditionally compiled code
        /// </summary>
        public bool IsInsideConditionalBlock { get; set; }

        /// <summary>
        /// Extracted class name
        /// </summary>
        public string Name { get; set; }
    }
}
