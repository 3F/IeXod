// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using net.r_eg.IeXod.Construction;
using net.r_eg.IeXod.Evaluation;
using net.r_eg.IeXod.Execution;
using Xunit;

namespace net.r_eg.IeXod.UnitTests.OM.Instance
{
    /// <summary>
    /// Tests for ProjectMetadataInstance public members
    /// </summary>
    public class ProjectMetadataInstance_Tests
    {
        /// <summary>
        /// Get name and value
        /// </summary>
        [Fact]
        public void Accessors()
        {
            ProjectMetadataInstance metadata = GetMetadataInstance();

            Assert.Equal("m", metadata.Name);
            Assert.Equal("m1", metadata.EvaluatedValue);
        }

        /// <summary>
        /// Get a single metadata instance
        /// </summary>
        private static ProjectMetadataInstance GetMetadataInstance()
        {
            Project project = new Project();
            ProjectInstance projectInstance = project.CreateProjectInstance();
            ProjectItemInstance item = projectInstance.AddItem("i", "i1");
            ProjectMetadataInstance metadata = item.SetMetadata("m", "m1");
            return metadata;
        }
    }
}
