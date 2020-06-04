// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using net.r_eg.IeXod.Construction;
using net.r_eg.IeXod.Evaluation;
using net.r_eg.IeXod.Execution;

namespace net.r_eg.IeXod.UnitTests.BackEnd
{
    /// <summary>
    /// Contains helper methods for creating projects for testing.
    /// </summary>
    internal static class ProjectHelpers
    {
        /// <summary>
        /// Creates a project instance with a single empty target named 'foo'
        /// </summary>
        /// <returns>A project instance.</returns>
        internal static ProjectInstance CreateEmptyProjectInstance()
        {
            XmlReader reader = XmlReader.Create(new StringReader
                (
                @"<Project xmlns='http://schemas.microsoft.com/developer/msbuild/2003' ToolsVersion='4.0'>
                      <Target Name='foo'/>
                  </Project>"
                ));

            Project project = new Project(reader);
            ProjectInstance instance = project.CreateProjectInstance();

            return instance;
        }
    }
}
