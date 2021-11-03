// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Diagnostics;
using System.Threading;
using net.r_eg.IeXod.Execution;
using net.r_eg.IeXod.UnitTests;
using net.r_eg.IeXod.Utilities;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace net.r_eg.IeXod.Engine.UnitTests.BackEnd
{
    public sealed class TaskHostFactory_Tests
    {
        [Fact(Skip = "IeXod. L-121")]
        [SkipOnTargetFramework(TargetFrameworkMonikers.Netcoreapp, "https://github.com/microsoft/msbuild/issues/5158")]
        [Trait("Category", "mono-osx-failing")]
        public void TaskNodesDieAfterBuild()
        {
            using (TestEnvironment env = TestEnvironment.Create())
            {
                string pidTaskProject = $@"
<Project>
    <UsingTask TaskName=""ProcessIdTask"" AssemblyName=""net.r_eg.IeXod.Engine.UnitTests"" TaskFactory=""TaskHostFactory"" />
    <Target Name='AccessPID'>
        <ProcessIdTask>
            <Output PropertyName=""PID"" TaskParameter=""Pid"" />
        </ProcessIdTask>
    </Target>
</Project>";
                TransientTestFile project = env.CreateFile("testProject.csproj", pidTaskProject);
                ProjectInstance projectInstance = new ProjectInstance(project.Path);
                projectInstance.Build().ShouldBeTrue();
                string processId = projectInstance.GetPropertyValue("PID");
                string.IsNullOrEmpty(processId).ShouldBeFalse();
                Int32.TryParse(processId, out int pid).ShouldBeTrue();
                Process.GetCurrentProcess().Id.ShouldNotBe<int>(pid);
                try
                {
                    Process taskHostNode = Process.GetProcessById(pid);
                    taskHostNode.WaitForExit(2000).ShouldBeTrue();
                }
                // We expect the TaskHostNode to exit quickly. If it exits before Process.GetProcessById, it will throw an ArgumentException.
                catch (ArgumentException e)
                {
                    e.Message.ShouldBe($"Process with an Id of {pid} is not running.");
                }
            }
        }

    }
}
