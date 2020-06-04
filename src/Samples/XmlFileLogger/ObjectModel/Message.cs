﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Xml.Linq;

namespace net.r_eg.IeXod.Logging.StructuredLogger
{
    /// <summary>
    /// Class representation of a logged generic string output message.
    /// </summary>
    internal class Message : ILogNode
    {
        private readonly string _message;
        private readonly DateTime _timestamp;

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="message">The message string.</param>
        /// <param name="timestamp">The time stamp of the logged event.</param>
        public Message(string message, DateTime timestamp)
        {
            _message = message;
            _timestamp = timestamp;
        }

        /// <summary>
        /// Writes the message to XML XElement representation.
        /// </summary>
        /// <param name="element">The parent element.</param>
        public void SaveToElement(XElement element)
        {
            element.Add(new XElement("Message", new XAttribute("Timestamp", _timestamp), new XText(_message)));
        }
    }
}
