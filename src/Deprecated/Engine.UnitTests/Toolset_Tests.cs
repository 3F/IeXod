// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using NUnit.Framework;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.BuildEngine;
using System.Xml;

namespace net.r_eg.IeXod.UnitTests
{
    [TestFixture]
    public class Toolset_Tests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToolsetCtorErrors1()
        {
            Toolset t = new Toolset(null, "x");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToolsetCtorErrors2()
        {
            Toolset t = new Toolset("x", null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ToolsetCtorErrors3()
        {
            Toolset t = new Toolset(String.Empty, "x");
        }

        [Test]
        public void Regress27993_TrailingSlashTrimmedFromMSBuildToolsPath()
        {
            Toolset t;

            t = new Toolset("x", "C:");
            Assertion.AssertEquals(@"C:", t.ToolsPath);
            t = new Toolset("x", @"C:\");
            Assertion.AssertEquals(@"C:\", t.ToolsPath);
            t = new Toolset("x", @"C:\\");
            Assertion.AssertEquals(@"C:\", t.ToolsPath);

            t = new Toolset("x", @"C:\foo");
            Assertion.AssertEquals(@"C:\foo", t.ToolsPath);
            t = new Toolset("x", @"C:\foo\");
            Assertion.AssertEquals(@"C:\foo", t.ToolsPath);
            t = new Toolset("x", @"C:\foo\\");
            Assertion.AssertEquals(@"C:\foo\", t.ToolsPath); // trim at most one slash

            t = new Toolset("x", @"\\foo\share");
            Assertion.AssertEquals(@"\\foo\share", t.ToolsPath);
            t = new Toolset("x", @"\\foo\share\");
            Assertion.AssertEquals(@"\\foo\share", t.ToolsPath);
            t = new Toolset("x", @"\\foo\share\\");
            Assertion.AssertEquals(@"\\foo\share\", t.ToolsPath); // trim at most one slash
        }
    }
 }
