// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace net.r_eg.IeXod.Logging.StructuredLogger
{
    /// <summary>
    /// Class representation of a logged item group entry.
    /// </summary>
    internal class ItemGroup : TaskParameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemGroup"/> class.
        /// </summary>
        /// <param name="message">The message from the logger.</param>
        /// <param name="prefix">The prefix string (e.g. 'Added item(s): ').</param>
        /// <param name="itemAttributeName">Name of the item attribute ('Include' or 'Remove').</param>
        public ItemGroup(string message, string prefix, string itemAttributeName) :
            base(message, prefix, false, itemAttributeName)
        {
        }
    }
}
