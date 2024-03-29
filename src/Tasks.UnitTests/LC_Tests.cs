// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Tasks;
using net.r_eg.IeXod.Utilities;
using Xunit;

namespace net.r_eg.IeXod.UnitTests
{
    /// <summary>
    /// Unit tests for the LC task
    /// </summary>
    public class LC_Tests
    {
        /// <summary>
        /// Tests a simple case of valid arguments
        /// </summary>
        [Fact]
        public void SimpleValidArgumentsCommandLine()
        {
            LC task = new LC();

            task.BuildEngine = new MockEngine();
            task.Sources = new TaskItem[] { new TaskItem("complist.licx"), new TaskItem("othersrc.txt") };
            task.LicenseTarget = new TaskItem("target.exe");
            task.OutputDirectory = "bin\\debug";
            task.ReferencedAssemblies = new TaskItem[] { new TaskItem("LicensedControl.dll"), new TaskItem("OtherControl.dll") };
            task.NoLogo = true;
            task.TargetFrameworkVersion = "2.0";

            CommandLine.ValidateHasParameter(task, "/complist:complist.licx", false /* don't use response file */);
            CommandLine.ValidateHasParameter(task, "/complist:othersrc.txt", false /* don't use response file */);
            CommandLine.ValidateHasParameter(task, "/target:target.exe", false /* don't use response file */);
            CommandLine.ValidateHasParameter(task, "/outdir:bin\\debug", false /* don't use response file */);
            CommandLine.ValidateHasParameter(task, "/i:LicensedControl.dll", false /* don't use response file */);
            CommandLine.ValidateHasParameter(task, "/i:OtherControl.dll", false /* don't use response file */);
            CommandLine.ValidateHasParameter(task, "/nologo", false /* don't use response file */);
        }

        /// <summary>
        /// Tests a simple case of valid arguments
        /// </summary>
        [Fact]
        public void SimpleValidArgumentsResponseFile()
        {
            LC task = new LC();

            task.BuildEngine = new MockEngine();
            task.Sources = new TaskItem[] { new TaskItem("complist.licx"), new TaskItem("othersrc.txt") };
            task.LicenseTarget = new TaskItem("target.exe");
            task.OutputDirectory = "bin\\debug";
            task.ReferencedAssemblies = new TaskItem[] { new TaskItem("LicensedControl.dll"), new TaskItem("OtherControl.dll") };
            task.NoLogo = true;
            task.TargetFrameworkVersion = "4.6";

            CommandLine.ValidateHasParameter(task, "/complist:complist.licx", true /* use response file */);
            CommandLine.ValidateHasParameter(task, "/complist:othersrc.txt", true /* use response file */);
            CommandLine.ValidateHasParameter(task, "/target:target.exe", true /* use response file */);
            CommandLine.ValidateHasParameter(task, "/outdir:bin\\debug", true /* use response file */);
            CommandLine.ValidateHasParameter(task, "/i:LicensedControl.dll", true /* use response file */);
            CommandLine.ValidateHasParameter(task, "/i:OtherControl.dll", true /* use response file */);
            CommandLine.ValidateHasParameter(task, "/nologo", true /* use response file */);
        }
    }
}
