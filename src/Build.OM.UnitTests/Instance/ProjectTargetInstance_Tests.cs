// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using System.Xml;
using net.r_eg.IeXod.Construction;
using net.r_eg.IeXod.Evaluation;
using net.r_eg.IeXod.Execution;
using Xunit;

namespace net.r_eg.IeXod.UnitTests.OM.Instance
{
    /// <summary>
    /// Tests for ProjectTargetInstance
    /// </summary>
    public class ProjectTargetInstance_Tests
    {
        /// <summary>
        /// Test accessors
        /// </summary>
        [Fact]
        public void Accessors()
        {
            ProjectTargetInstance target = GetSampleTargetInstance();

            Assert.Equal("t", target.Name);
            Assert.Equal("c", target.Condition);
            Assert.Equal("i", target.Inputs);
            Assert.Equal("o", target.Outputs);
            Assert.Equal("d", target.DependsOnTargets);
            Assert.Equal("b", target.BeforeTargets);
            Assert.Equal("a", target.AfterTargets);
            Assert.Equal("k", target.KeepDuplicateOutputs);
            Assert.Equal("r", target.Returns);
            Assert.Equal("t1", ((ProjectTaskInstance)target.Children[0]).Name);

            IList<ProjectTaskInstance> tasks = Helpers.MakeList(target.Tasks);
            Assert.Single(tasks);
            Assert.Equal("t1", tasks[0].Name);
        }

        /// <summary>
        /// Evaluation of a project with more than one target with the same name
        /// should skip all but the last one.
        /// </summary>
        [Fact]
        public void TargetOverride()
        {
            ProjectRootElement projectXml = ProjectRootElement.Create();
            projectXml.AddTarget("t").Inputs = "i1";
            projectXml.AddTarget("t").Inputs = "i2";

            Project project = new Project(projectXml);
            ProjectInstance instance = project.CreateProjectInstance();

            ProjectTargetInstance target = instance.Targets["t"];

            Assert.Equal("i2", target.Inputs);
        }

        /// <summary>
        /// Evaluation of a project with more than one target with the same name
        /// should skip all but the last one.  This is true even if the targets
        /// involved only have the same unescaped name (Orcas compat)
        /// </summary>
        [Fact]
        public void TargetOverride_Escaped()
        {
            ProjectRootElement projectXml = ProjectRootElement.Create();
            projectXml.AddTarget("t%3b").Inputs = "i1";
            projectXml.AddTarget("t;").Inputs = "i2";

            Project project = new Project(projectXml);
            ProjectInstance instance = project.CreateProjectInstance();

            ProjectTargetInstance target = instance.Targets["t;"];

            Assert.Equal("i2", target.Inputs);
        }

        /// <summary>
        /// Evaluation of a project with more than one target with the same name
        /// should skip all but the last one.  This is true even if the targets
        /// involved only have the same unescaped name (Orcas compat)
        /// </summary>
        [Fact]
        public void TargetOverride_Escaped2()
        {
            ProjectRootElement projectXml = ProjectRootElement.Create();
            projectXml.AddTarget("t;").Inputs = "i1";
            projectXml.AddTarget("t%3b").Inputs = "i2";

            Project project = new Project(projectXml);
            ProjectInstance instance = project.CreateProjectInstance();

            ProjectTargetInstance target = instance.Targets["t;"];

            Assert.Equal("i2", target.Inputs);
        }

        /// <summary>
        /// Verify that targets from a saved, but subsequently edited, project
        /// provide the correct full path.
        /// </summary>
        [Fact]
        public void FileLocationAvailableEvenAfterEdits()
        {
            string path = null;

            try
            {
                path = net.r_eg.IeXod.Shared.FileUtilities.GetTemporaryFile();
                ProjectRootElement projectXml = ProjectRootElement.Create(path);
                projectXml.Save();

                projectXml.AddTarget("t");

                Project project = new Project(projectXml);
                ProjectTargetInstance target = project.Targets["t"];

                Assert.Equal(project.FullPath, target.FullPath);
            }
            finally
            {
                File.Delete(path);
            }
        }

        /// <summary>
        /// Create a ProjectTargetInstance with some parameters
        /// </summary>
        private static ProjectTargetInstance GetSampleTargetInstance()
        {
            string content = @"
                    <Project xmlns='http://schemas.microsoft.com/developer/msbuild/2003' >
                        <Target Name='t' Inputs='i' Outputs='o' Condition='c' DependsOnTargets='d' BeforeTargets='b' AfterTargets='a' KeepDuplicateOutputs='k' Returns='r'>
                            <t1/>
                        </Target>
                    </Project>
                ";

            ProjectRootElement xml = ProjectRootElement.Create(XmlReader.Create(new StringReader(content)));
            Project project = new Project(xml);
            ProjectInstance instance = project.CreateProjectInstance();
            ProjectTargetInstance target = instance.Targets["t"];

            return target;
        }
    }
}
