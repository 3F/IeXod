// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using net.r_eg.IeXod.Evaluation;
using net.r_eg.IeXod.Execution;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Tasks;
using net.r_eg.IeXod.Utilities;
using System.Text.RegularExpressions;
using Shouldly;
using Xunit;

namespace net.r_eg.IeXod.UnitTests
{
    sealed public class CallTarget_Tests : IDisposable
    {
        public CallTarget_Tests()
        {
            ProjectCollection.GlobalProjectCollection.UnloadAllProjects();
        }

        public void Dispose()
        {
            ProjectCollection.GlobalProjectCollection.UnloadAllProjects();
        }

        /// <summary>
        /// Simple test of the CallTarget task.
        /// </summary>
        [Fact]
        public void Simple()
        {
            MockLogger logger = ObjectModelHelpers.BuildProjectExpectSuccess(@"

                <Project DefaultTargets=`a` ToolsVersion=`msbuilddefaulttoolsversion` xmlns=`msbuildnamespace`>
                    <Target Name=`a` >
	                    <CallTarget Targets=`b` />
                    </Target>
                    <Target Name=`b` >
	                    <Message Text=`Inside B` />
                    </Target>
                </Project>
                ");

            logger.AssertLogContains("Inside B");
        }

        /// <summary>
        /// Simple test of the CallTarget task, where one of the middle targets invoked fails.
        /// </summary>
        [Fact]
        public void FailedTargets()
        {
            MockLogger logger = ObjectModelHelpers.BuildProjectExpectFailure(@"

                <Project DefaultTargets=`build` ToolsVersion=`msbuilddefaulttoolsversion` xmlns=`msbuildnamespace`>
                    <Target Name=`build` >
	                    <CallTarget Targets=`a; b; c` />
                    </Target>
                    <Target Name=`a` >
	                    <Message Text=`Inside A` />
                    </Target>
                    <Target Name=`b` >
	                    <Error Text=`Inside B` />
                    </Target>
                    <Target Name=`c` >
	                    <Message Text=`Inside C` />
                    </Target>
                </Project>
                ");

            logger.AssertLogContains("Inside A");
            logger.AssertLogContains("Inside B");

            // Target C should not have been run.
            logger.AssertLogDoesntContain("Inside C");
        }

        /// <summary>
        /// Test the CallTarget task, where one of the middle targets invoked fails, but we
        /// specified RunEachTargetSeparately, so all the targets should have been run anyway.
        /// </summary>
        [Fact]
        public void FailedTargetsRunSeparately()
        {
            MockLogger logger = ObjectModelHelpers.BuildProjectExpectFailure(@"

                <Project DefaultTargets=`build` ToolsVersion=`msbuilddefaulttoolsversion` xmlns=`msbuildnamespace`>
                    <Target Name=`build` >
	                    <CallTarget Targets=`a; b; c` RunEachTargetSeparately=`true` />
                    </Target>
                    <Target Name=`a` >
	                    <Message Text=`Inside A` />
                    </Target>
                    <Target Name=`b` >
	                    <Error Text=`Inside B` />
                    </Target>
                    <Target Name=`c` >
	                    <Message Text=`Inside C` />
                    </Target>
                </Project>
                ");

            // All three targets should have been run.
            logger.AssertLogContains("Inside A");
            logger.AssertLogContains("Inside B");
            logger.AssertLogContains("Inside C");
        }

        [Fact]
        public void FailsWithOnlyTargetErrors()
        {
            MockLogger logger = ObjectModelHelpers.BuildProjectExpectFailure(@"
                <Project>
                  <Target Name='Init'>
                    <CallTarget Targets='Inside' />
                  </Target>
                  <Target Name='Inside'>
                    <Error />
                  </Target>
                </Project>");

            logger.ErrorCount.ShouldBe (1);
        }

        /// <summary>
        /// Test the CallTarget task, where we don't pass in any targets.  This is expected
        /// to succeed, so that callers of the task don't have to add a Condition to ensure
        /// that the list of targets is non-empty.
        /// </summary>
        [Fact]
        public void NoTargets()
        {
            ObjectModelHelpers.BuildProjectExpectSuccess(@"

                <Project DefaultTargets=`build` ToolsVersion=`msbuilddefaulttoolsversion` xmlns=`msbuildnamespace`>
                    <Target Name=`build` >
	                    <CallTarget Targets=` @(empty) ` />
                    </Target>
                </Project>
                ");
        }

        /// <summary>
        /// Test the CallTarget task and capture the outputs of the invoked targets.
        /// </summary>
        [Fact]
        public void CaptureTargetOutputs()
        {
            Project project = ObjectModelHelpers.CreateInMemoryProject(@"

                <Project DefaultTargets=`build` ToolsVersion=`msbuilddefaulttoolsversion` xmlns=`msbuildnamespace`>
                    <Target Name=`build` >

                        <CallTarget Targets=` a; b; c ` >
                            <Output ItemName=`myfancytargetoutputs` TaskParameter=`TargetOutputs`/>
                        </CallTarget>

                    </Target>
                    <!-- include some nice characters that need escaping -->
                    <Target Name=`a` Outputs=`a.t!@#$%^xt`>
	                    <Message Text=`Inside A` />
                    </Target>
                    <Target Name=`b` Outputs=`b.txt`>
	                    <Message Text=`Inside B` />
                    </Target>
                    <Target Name=`c` Outputs=`c.txt`>
	                    <Message Text=`Inside C` />
                    </Target>
                </Project>

                ");

            ProjectInstance instance = project.CreateProjectInstance();
            bool success = instance.Build();
            Assert.True(success); // "Build failed.  See test output (Attachments in Azure Pipelines) for details"

            IEnumerable<ProjectItemInstance> targetOutputs = instance.GetItems("myfancytargetoutputs");

            // Convert to a list of TaskItems for easier verification.
            List<ITaskItem> targetOutputsTaskItems = new List<ITaskItem>();
            foreach (ProjectItemInstance item in targetOutputs)
            {
                targetOutputsTaskItems.Add(new TaskItem(item.EvaluatedInclude));
            }

            // Order independent verification of the right set of items.
            ObjectModelHelpers.AssertItemsMatch(@"
                c.txt
                b.txt
                a.t!@#$%^xt
                ",
                targetOutputsTaskItems.ToArray(), false /* ignore the order */);
        }

        [Fact]
        public void CaptureTargetOutputsRunningEachTargetSeparately()
        {
            MockLogger logger = ObjectModelHelpers.BuildProjectExpectSuccess(@"

                <Project DefaultTargets = `CallT` ToolsVersion=`msbuilddefaulttoolsversion` xmlns=`msbuildnamespace`>
	                <Target Name = `a` Outputs = `a.txt`/>
	                <Target Name = `b` Outputs = `b.txt`/>
	                <Target Name = `c` Outputs = `c.txt`/>
	                <Target Name = `CallT`>
		                <CallTarget
			                Targets = `a;b;c`
			                RunEachTargetSeparately = `true`>
			                <Output TaskParameter= `TargetOutputs` ItemName = `TaskOut`/>
		                </CallTarget>
		                <Message Text = `CallTarget Outputs: @(TaskOut)`/>
	                </Target>
                </Project>
                ");

            // All three targets should have been run.
            logger.AssertLogContains("CallTarget Outputs: a.txt;b.txt;c.txt");
        }
    }
}
