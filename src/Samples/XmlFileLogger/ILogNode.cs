// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml.Linq;

namespace net.r_eg.IeXod.Logging.StructuredLogger
{
    /// <summary>
    /// Interface class for an execution MSBuild log node to be represented in XML
    /// </summary>
    internal interface ILogNode
    {
        /// <summary>
        /// Writes the node to XML XElement representation.
        /// </summary>
        /// <param name="parentElement">The parent element.</param>
        void SaveToElement(XElement parentElement);
    }
}
