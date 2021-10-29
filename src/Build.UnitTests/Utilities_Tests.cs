// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Shared;



using CommunicationsUtilities = net.r_eg.IeXod.Internal.CommunicationsUtilities;
using InternalUtilities = net.r_eg.IeXod.Internal.Utilities;
using InvalidProjectFileException = net.r_eg.IeXod.Exceptions.InvalidProjectFileException;
using Project = net.r_eg.IeXod.Evaluation.Project;
using ProjectCollection = net.r_eg.IeXod.Evaluation.ProjectCollection;

using Toolset = net.r_eg.IeXod.Evaluation.Toolset;


using XmlDocumentWithLocation = net.r_eg.IeXod.Construction.XmlDocumentWithLocation;
using XmlElementWithLocation = net.r_eg.IeXod.Construction.XmlElementWithLocation;

using Xunit;
using System.Collections.Generic;
using System.IO;

namespace net.r_eg.IeXod.UnitTests
{
    public class UtilitiesTestStandard : UtilitiesTest
    {
        public UtilitiesTestStandard()
        {
            this.loadAsReadOnly = false;
        }

        [Fact]
        public void GetTextFromTextNodeWithXmlComment5()
        {
            string xmlText = "<MyXmlElement>&lt;<!-- bar; baz; --><x/><!-- bar --></MyXmlElement>";
            string xmlContents = GetXmlContents(xmlText);
            // Should get XML; note space after x added
            Assert.Equal("&lt;<!-- bar; baz; --><x /><!-- bar -->", xmlContents);
        }

        [Fact]
        public void GetTextFromTextNodeWithXmlComment6()
        {
            string xmlText = "<MyXmlElement><x/><!-- bar; baz; --><!-- bar --></MyXmlElement>";
            string xmlContents = GetXmlContents(xmlText);
            // Should get XML; note space after x added
            Assert.Equal("<x /><!-- bar; baz; --><!-- bar -->", xmlContents);
        }

        [Fact]
        public void GetTextFromTextNodeWithXmlComment7()
        {
            string xmlText = "<MyXmlElement><!-- bar; baz; --><!-- bar --><x/></MyXmlElement>";
            string xmlContents = GetXmlContents(xmlText);
            // Should get XML; note space after x added
            Assert.Equal("<!-- bar; baz; --><!-- bar --><x />", xmlContents);
        }
    }

    public class UtilitiesTestReadOnlyLoad : UtilitiesTest
    {
        public UtilitiesTestReadOnlyLoad()
        {
            this.loadAsReadOnly = true;
        }

        [Fact]
        public void GetTextFromTextNodeWithXmlComment5()
        {
            string xmlText = "<MyXmlElement>&lt;<!-- bar; baz; --><x/><!-- bar --></MyXmlElement>";
            string xmlContents = GetXmlContents(xmlText);
            // Should get XML; note space after x added
            Assert.Equal("&lt;<!----><x /><!---->", xmlContents);
        }

        [Fact]
        public void GetTextFromTextNodeWithXmlComment6()
        {
            string xmlText = "<MyXmlElement><x/><!-- bar; baz; --><!-- bar --></MyXmlElement>";
            string xmlContents = GetXmlContents(xmlText);
            // Should get XML; note space after x added
            Assert.Equal("<x /><!----><!---->", xmlContents);
        }

        [Fact]
        public void GetTextFromTextNodeWithXmlComment7()
        {
            string xmlText = "<MyXmlElement><!-- bar; baz; --><!-- bar --><x/></MyXmlElement>";
            string xmlContents = GetXmlContents(xmlText);
            // Should get XML; note space after x added
            Assert.Equal("<!----><!----><x />", xmlContents);
        }
    }

    public abstract class UtilitiesTest
    {
        public bool loadAsReadOnly;

        /// <summary>
        /// Verify Condition is illegal on ProjectExtensions tag
        /// </summary>
        [Fact]
        public void IllegalConditionOnProjectExtensions()
        {
            Assert.Throws<InvalidProjectFileException>(() =>
            {
                ObjectModelHelpers.CreateInMemoryProject(@"

                <Project ToolsVersion=`msbuilddefaulttoolsversion` xmlns=`msbuildnamespace`>
                    <ProjectExtensions Condition=`'a'=='b'`/>
                    <Import Project=`$(MSBuildBinPath)\\Microsoft.CSharp.Targets` />
                </Project>
            ");
            }
           );
        }
        /// <summary>
        /// Verify ProjectExtensions cannot exist twice
        /// </summary>
        [Fact]
        public void RepeatedProjectExtensions()
        {
            Assert.Throws<InvalidProjectFileException>(() =>
            {
                ObjectModelHelpers.CreateInMemoryProject(@"
                <Project ToolsVersion=`msbuilddefaulttoolsversion` xmlns=`msbuildnamespace`> 
                    <ProjectExtensions/>
                    <Import Project=`$(MSBuildBinPath)\\Microsoft.CSharp.Targets` />
                    <ProjectExtensions/>
                </Project>
            ");
            }
           );
        }
        /// <summary>
        /// Tests that we can correctly pass a CDATA tag containing less-than signs into a property value.
        /// </summary>
        [Fact]
        public void GetCDATAWithLessThanSignFromXmlNode()
        {
            string xmlText = "<MyXmlElement><![CDATA[<invalid<xml&&<]]></MyXmlElement>";
            string xmlContents = GetXmlContents(xmlText);
            Assert.Equal("<invalid<xml&&<", xmlContents);
        }

        /// <summary>
        /// Tests that we can correctly pass an Xml element named "CDATA" into a property value.
        /// </summary>
        [Fact]
        public void GetLiteralCDATAWithLessThanSignFromXmlNode()
        {
            string xmlText = "<MyXmlElement>This is not a real <CDATA/>, just trying to fool the reader.</MyXmlElement>";
            string xmlContents = GetXmlContents(xmlText);

            // Notice the extra space after "CDATA" because it normalized the XML.
            Assert.Equal("This is not a real <CDATA />, just trying to fool the reader.", xmlContents);
        }

        /// <summary>
        /// Tests that we can correctly pass a simple CDATA tag into a property value.
        /// </summary>
        [Fact]
        public void GetCDATAFromXmlNode()
        {
            string xmlText = "<MyXmlElement><![CDATA[whatever]]></MyXmlElement>";
            string xmlContents = GetXmlContents(xmlText);
            Assert.Equal("whatever", xmlContents);
        }

        /// <summary>
        /// Tests that we can correctly pass a literal string called "CDATA" into a property value.
        /// </summary>
        [Fact]
        public void GetLiteralCDATAFromXmlNode()
        {
            string xmlText = "<MyXmlElement>This is not a real CDATA, just trying to fool the reader.</MyXmlElement>";
            string xmlContents = GetXmlContents(xmlText);
            Assert.Equal("This is not a real CDATA, just trying to fool the reader.", xmlContents);
        }

        /// <summary>
        /// Tests that we can correctly parse a property that is Xml containing a CDATA tag.
        /// </summary>
        [Fact]
        public void GetCDATAOccurringDeeperWithMoreXml()
        {
            string xmlText = "<MyXmlElement><RootOfPropValue><![CDATA[foo]]></RootOfPropValue></MyXmlElement>";
            string xmlContents = GetXmlContents(xmlText);
            Assert.Equal("<RootOfPropValue><![CDATA[foo]]></RootOfPropValue>", xmlContents);
        }

        /// <summary>
        /// Tests that we can correctly pass CDATA where the CDATA tag itself is surrounded by whitespace
        /// </summary>
        [Fact]
        public void GetCDATAWithSurroundingWhitespace()
        {
            string xmlText = "<MyXmlElement>    <![CDATA[foo]]>    </MyXmlElement>";
            string xmlContents = GetXmlContents(xmlText);
            Assert.Equal("foo", xmlContents);
        }

        /// <summary>
        /// Tests that we can correctly parse a property that is some text concatenated with some XML.
        /// </summary>
        [Fact]
        public void GetTextContainingLessThanSignFromXmlNode()
        {
            string xmlText = "<MyXmlElement>This is some text contain a node <xml a='&lt;'/>, &amp; an escaped character.</MyXmlElement>";
            string xmlContents = GetXmlContents(xmlText);

            // Notice the extra space in the xml node because it normalized the XML, and the
            // change from single quotes to double-quotes.
            Assert.Equal("This is some text contain a node <xml a=\"&lt;\" />, &amp; an escaped character.", xmlContents);
        }

        /// <summary>
        /// Tests that we can correctly parse a property containing text with an escaped character.
        /// </summary>
        [Fact]
        public void GetTextFromXmlNode()
        {
            string xmlText = "<MyXmlElement>This is some text &amp; an escaped character.</MyXmlElement>";
            string xmlContents = GetXmlContents(xmlText);
            Assert.Equal("This is some text & an escaped character.", xmlContents);
        }

        /// <summary>
        /// Tests that comments are removed if there is no other XML in the value.
        /// In other words, .InnerText is used even if there are comments (as long as nothing else looks like XML in the string)
        /// </summary>
        [Fact]
        public void GetTextFromTextNodeWithXmlComment()
        {
            string xmlText = "<MyXmlElement>foo; <!-- bar; baz; -->biz; &amp; boz</MyXmlElement>";
            string xmlContents = GetXmlContents(xmlText);
            Assert.Equal("foo; biz; & boz", xmlContents);
        }

        [Fact]
        public void GetTextFromTextNodeWithXmlComment2()
        {
            string xmlText = "<MyXmlElement><!-- bar; baz; -->xyz<!-- bar --></MyXmlElement>";
            string xmlContents = GetXmlContents(xmlText);
            Assert.Equal("xyz", xmlContents);
        }

        [Fact]
        public void GetTextFromTextNodeWithXmlComment3()
        {
            string xmlText = "<MyXmlElement><!----></MyXmlElement>";
            string xmlContents = GetXmlContents(xmlText);
            Assert.Equal("", xmlContents);
        }

        [Fact]
        public void GetTextFromTextNodeWithXmlComment4()
        {
            string xmlText = "<MyXmlElement>--></MyXmlElement>";
            string xmlContents = GetXmlContents(xmlText);
            Assert.Equal("-->", xmlContents);
        }

        /// <summary>
        /// Check creating the tools version list for an error message
        /// </summary>
        [Fact]
        public void CreateToolsVersionString()
        {
            List<Toolset> toolsets = new List<Toolset>();
            toolsets.Add(new Toolset("66", "x", new ProjectCollection(), null));
            toolsets.Add(new Toolset("44", "y", new ProjectCollection(), null));

            string result = InternalUtilities.CreateToolsVersionListString(toolsets);

            Assert.Equal("\"66\", \"44\"", result);
        }

        /// <summary>
        /// Verify our custom way of getting env vars gives the same results as the BCL.
        /// </summary>
        [Fact]
        public void GetEnvVars()
        {
            IDictionary<string, string> envVars = CommunicationsUtilities.GetEnvironmentVariables();
            IDictionary referenceVars = Environment.GetEnvironmentVariables();
            IDictionary<string, string> referenceVars2 = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (DictionaryEntry item in referenceVars)
            {
                referenceVars2.Add((string)item.Key, (string)item.Value);
            }

            Helpers.AssertCollectionsValueEqual(envVars, referenceVars2);
        }

        protected string GetXmlContents(string xmlText)
        {
            XmlDocumentWithLocation xmldoc = new XmlDocumentWithLocation(loadAsReadOnly);
            xmldoc.LoadXml(xmlText);

            XmlElementWithLocation rootElement = (XmlElementWithLocation)xmldoc.FirstChild;
            Console.WriteLine("originalxml = " + xmlText);
            Console.WriteLine("innerText   = " + rootElement.InnerText);
            Console.WriteLine("innerXml    = " + rootElement.InnerXml);
            Console.WriteLine("-----------");

            string xmlContents = InternalUtilities.GetXmlNodeInnerContents(rootElement);
            return xmlContents;
        }
    }
}
