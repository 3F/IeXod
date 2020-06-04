// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Xml;
using net.r_eg.IeXod.Construction;
using net.r_eg.IeXod.Evaluation;
using net.r_eg.IeXod.Execution;
using Xunit;

namespace net.r_eg.IeXod.UnitTests.OM.Instance
{
    /// <summary>
    /// Tests for the ProjectTaskOutputItemInstance class.
    /// </summary>
    public class ProjectTaskOutputItemInstance_Tests
    {
        /// <summary>
        /// Test accessors
        /// </summary>
        [Fact]
        public void Accessors()
        {
            var output = GetSampleTaskOutputInstance();

            Assert.Equal("p", output.TaskParameter);
            Assert.Equal("c", output.Condition);
            Assert.Equal("i", output.ItemType);
        }

        /// <summary>
        /// Create a TaskInstance with some parameters
        /// </summary>
        private static ProjectTaskOutputItemInstance GetSampleTaskOutputInstance()
        {
            string content = @"
                    <Project xmlns='http://schemas.microsoft.com/developer/msbuild/2003' >
                       <Target Name='t'>
                            <t1>
                                <Output TaskParameter='p' Condition='c' ItemName='i'/>
                            </t1>
                        </Target>
                    </Project>
                ";

            ProjectRootElement xml = ProjectRootElement.Create(XmlReader.Create(new StringReader(content)));
            Project project = new Project(xml);
            ProjectInstance instance = project.CreateProjectInstance();
            ProjectTaskInstance task = (ProjectTaskInstance)instance.Targets["t"].Children[0];
            ProjectTaskOutputItemInstance output = (ProjectTaskOutputItemInstance)task.Outputs[0];

            return output;
        }
    }
}
