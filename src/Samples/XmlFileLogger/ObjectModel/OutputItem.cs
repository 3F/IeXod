// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace net.r_eg.IeXod.Logging.StructuredLogger
{
    /// <summary>
    /// Class representation of a task output item group.
    /// </summary>
    internal class OutputItem : TaskParameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutputItem"/> class.
        /// </summary>
        /// <param name="message">The message from the logger..</param>
        /// <param name="prefix">The prefix string (e.g. 'Output Item(s): ').</param>
        public OutputItem(string message, string prefix)
            : base(message, prefix)
        {
        }
    }
}
