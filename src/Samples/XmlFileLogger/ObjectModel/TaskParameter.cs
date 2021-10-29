﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace net.r_eg.IeXod.Logging.StructuredLogger
{
    /// <summary>
    /// Abstract base class for task input / output parameters (can be ItemGroups)
    /// </summary>
    internal abstract class TaskParameter : ILogNode
    {
        protected bool collapseSingleItem;
        protected string itemAttributeName;
        protected readonly List<Item> items = new List<Item>();
        protected readonly string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskParameter"/> class.
        /// </summary>
        /// <param name="message">The message from the logging system.</param>
        /// <param name="prefix">The prefix parsed out (e.g. 'Output Item(s): ').</param>
        /// <param name="collapseSingleItem">If set to <c>true</c>, will collapse the node to a single item when possible.</param>
        /// <param name="itemAttributeName">Name of the item 'Include' attribute.</param>
        protected TaskParameter(string message, string prefix, bool collapseSingleItem = true, string itemAttributeName = "Include")
        {
            this.collapseSingleItem = collapseSingleItem;
            this.itemAttributeName = itemAttributeName;

            foreach (var item in ItemGroupParser.ParseItemList(message, prefix, out name))
            {
                items.Add(item);
            }
        }

        /// <summary>
        /// Saves the task parameter node to XML XElement.
        /// </summary>
        /// <param name="parentElement">The parent element.</param>
        public void SaveToElement(XElement parentElement)
        {
            XElement element = new XElement(name);
            parentElement.Add(element);

            if (collapseSingleItem && items.Count == 1 && !items[0].Metadata.Any())
            {
                element.Add(items[0].Text);
            }
            else
            {
                foreach (var item in items)
                {
                    item.SaveToElement(element, itemAttributeName, collapseSingleItem);
                }
            }
        }

        /// <summary>
        /// Creates a concrete Task Parameter type based on the message logging message.
        /// </summary>
        /// <param name="message">The message string from the logger.</param>
        /// <param name="prefix">The prefix to the message string.</param>
        /// <returns>Concrete task parameter node.</returns>
        public static TaskParameter Create(string message, string prefix)
        {
            switch (prefix)
            {
                case XmlFileLogger.OutputItemsMessagePrefix:
                    return new OutputItem(message, prefix);
                case XmlFileLogger.TaskParameterMessagePrefix:
                    return new InputParameter(message, prefix);
                case XmlFileLogger.OutputPropertyMessagePrefix:
                    return new OutputProperty(message, prefix);
                case XmlFileLogger.ItemGroupIncludeMessagePrefix:
                    return new ItemGroup(message, prefix, "Include");
                case XmlFileLogger.ItemGroupRemoveMessagePrefix:
                    return new ItemGroup(message, prefix, "Remove");
                default:
                    throw new UnknownTaskParameterPrefixException(prefix);
            }
        }
    }
}
