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
    /// Tests for the ProjectOnErrorInstance class.
    /// </summary>
    public class ProjectOnErrorInstance_Tests
    {
        /// <summary>
        /// Test accessors
        /// </summary>
        [Fact]
        public void Accessors()
        {
            var onError = GetSampleOnErrorInstance();

            Assert.Equal("et", onError.ExecuteTargets);
            Assert.Equal("c", onError.Condition);
        }

        /// <summary>
        /// Create a TaskInstance with some parameters
        /// </summary>
        private static ProjectOnErrorInstance GetSampleOnErrorInstance()
        {
            string content = @"
                    <Project xmlns='http://schemas.microsoft.com/developer/msbuild/2003' >
                       <Target Name='t'>
                            <OnError ExecuteTargets='et' Condition='c'/>
                        </Target>
                    </Project>
                ";

            ProjectRootElement xml = ProjectRootElement.Create(XmlReader.Create(new StringReader(content)));
            Project project = new Project(xml);
            ProjectInstance instance = project.CreateProjectInstance();
            ProjectOnErrorInstance onError = (ProjectOnErrorInstance)instance.Targets["t"].OnErrorChildren[0];

            return onError;
        }
    }
}
