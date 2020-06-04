// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Xml;
using net.r_eg.IeXod.BuildEngine;
using net.r_eg.IeXod.BuildEngine.Shared;

namespace net.r_eg.IeXod.UnitTests
{
    internal static class XmlTestUtilities
    {
        internal static XmlElement CreateBasicElementWithOneAttribute(string elementName, string attributeName, string attributeValue)
        {
            XmlElement element = CreateBasicElement(elementName);
            AddAttribute(element, attributeName, attributeValue);
            return element;
        }

        internal static void AddAttribute(XmlNode element, string attributeName, string attributeValue)
        {
            XmlAttribute attribute = element.OwnerDocument.CreateAttribute(attributeName);
            element.Attributes.Append(attribute);
            attribute.Value = attributeValue;
        }

        internal static XmlElement AddChildElement(XmlNode parentElement, string childName)
        {
            XmlElement element = parentElement.OwnerDocument.CreateElement(childName, XMakeAttributes.defaultXmlNamespace);
            parentElement.AppendChild(element);
            return element;
        }

        internal static XmlElement AddChildElementWithInnerText(XmlNode parentElement, string childName, string innerText)
        {
            XmlElement element = AddChildElement(parentElement, childName);
            element.InnerText = innerText;
            return element;
        }

        internal static XmlElement CreateBasicElement(string elementName)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement element = doc.CreateElement(elementName, XMakeAttributes.defaultXmlNamespace);
            return element;
        }
    }
}
