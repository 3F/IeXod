// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Reflection;
using System.Collections;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Tasks;
using net.r_eg.IeXod.Utilities;
using System.Text.RegularExpressions;
using System.Globalization;
using Xunit;
using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod.UnitTests
{
    sealed public class WinMDExpTests
    {
        /// <summary>
        /// Tests the "References" parameter on the winmdexp task, and confirms that it sets
        /// the /reference switch on the command-line correctly.  
        /// </summary>
        [Fact]
        public void References()
        {
            WinMDExp t = new WinMDExp();
            t.WinMDModule = "Foo.dll";

            TaskItem mscorlibReference = new TaskItem("mscorlib.dll");
            TaskItem windowsFoundationReference = new TaskItem("Windows.Foundation.winmd");

            t.References = new TaskItem[] { mscorlibReference, windowsFoundationReference };
            CommandLine.ValidateHasParameter(
                t,
                "/reference:mscorlib.dll",
                useResponseFile: true);
            CommandLine.ValidateHasParameter(
                t,
                "/reference:Windows.Foundation.winmd",
                useResponseFile: true);
        }

        [Fact]
        public void TestNoWarnSwitchWithWarnings()
        {
            WinMDExp t = new WinMDExp();
            t.WinMDModule = "Foo.dll";
            t.DisabledWarnings = "41999,42016";
            CommandLine.ValidateHasParameter(t, "/nowarn:41999,42016", useResponseFile: true);
        }


        // Tests the "GenerateDocumentation" and "DocumentationFile" parameters on the Vbc task,
        // and confirms that it sets the /doc switch on the command-line correctly.
        [Fact]
        public void DocumentationFile()
        {
            WinMDExp t = new WinMDExp();

            t.WinMDModule = "Foo.dll";
            t.OutputDocumentationFile = "output.xml";
            t.InputDocumentationFile = "input.xml";

            CommandLine.ValidateHasParameter(t, "/d:output.xml", useResponseFile: true);
            CommandLine.ValidateHasParameter(t, "/md:input.xml", useResponseFile: true);
        }

        [Fact]
        public void PDBFileTesting()
        {
            WinMDExp t = new WinMDExp();
            t.WinMDModule = "Foo.dll";
            t.OutputWindowsMetadataFile = "Foo.dll";
            t.OutputPDBFile = "output.pdb";
            t.InputPDBFile = "input.pdb";

            CommandLine.ValidateHasParameter(t, "/pdb:output.pdb", useResponseFile: true);
            CommandLine.ValidateHasParameter(t, "/mp:input.pdb", useResponseFile: true);
        }

        [Fact]
        public void WinMDModule()
        {
            WinMDExp t = new WinMDExp();

            t.WinMDModule = "Foo.dll";
            CommandLine.ValidateContains(t, "Foo.dll", useResponseFile: true);
        }

        [Fact]
        public void UsesrDefinedOutputFile()
        {
            WinMDExp t = new WinMDExp();
            t.WinMDModule = "Foo.dll";
            t.OutputWindowsMetadataFile = "Bob.winmd";
            CommandLine.ValidateHasParameter(t, "/out:Bob.winmd", useResponseFile: true);
        }

        [Fact]
        public void NoOutputFileDefined()
        {
            WinMDExp t = new WinMDExp();

            t.WinMDModule = "Foo.dll";
            t.OutputWindowsMetadataFile = "Foo.winmd";
            CommandLine.ValidateHasParameter(t, "/out:Foo.winmd", useResponseFile: true);
        }

        [Fact]
        public void ArgumentsAreUnquoted()
        {
            WinMDExp t = new WinMDExp
            {
                AssemblyUnificationPolicy = "sp ace",
                InputDocumentationFile = "sp ace",
                InputPDBFile = "sp ace",
                References = new ITaskItem[]
                {
                    new TaskItem(@"sp ace"),
                },
                OutputDocumentationFile = "sp ace",
                OutputPDBFile = "sp ace",
                OutputWindowsMetadataFile = "sp ace",
                WinMDModule = "sp ace",
            };

            CommandLineBuilderExtension c = new CommandLineBuilderExtension(quoteHyphensOnCommandLine: false, useNewLineSeparator: true);

            t.AddResponseFileCommands(c);

            string[] actual = c.ToString().Split(MSBuildConstants.EnvironmentNewLine, StringSplitOptions.None);
            string[] expected =
            {
                "/d:sp ace",
                "/md:sp ace",
                "/mp:sp ace",
                "/pdb:sp ace",
                "/assemblyunificationpolicy:sp ace",
                "/out:sp ace",
                "/reference:sp ace",
                "sp ace",
            };

            Assert.Equal(expected, actual);
        }
    }
}





