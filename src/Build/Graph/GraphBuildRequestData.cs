﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using net.r_eg.IeXod.Execution;
using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod.Graph
{
    /// <summary>
    /// GraphBuildRequestData encapsulates all of the data needed to submit a graph build request.
    /// </summary>
    public sealed class GraphBuildRequestData
    {
        /// <summary>
        /// Constructs a GraphBuildRequestData for build requests based on a project graph.
        /// </summary>
        /// <param name="projectGraph">The graph to build.</param>
        /// <param name="targetsToBuild">The targets to build.</param>
        public GraphBuildRequestData(ProjectGraph projectGraph, ICollection<string> targetsToBuild)
            : this(projectGraph, targetsToBuild, null, BuildRequestDataFlags.None)
        {
        }

        /// <summary>
        /// Constructs a GraphBuildRequestData for build requests based on a project graph.
        /// </summary>
        /// <param name="projectGraph">The graph to build.</param>
        /// <param name="targetsToBuild">The targets to build.</param>
        /// <param name="hostServices">The host services to use, if any.  May be null.</param>
        public GraphBuildRequestData(ProjectGraph projectGraph, ICollection<string> targetsToBuild, HostServices hostServices)
            : this(projectGraph, targetsToBuild, hostServices, BuildRequestDataFlags.None)
        {
        }

        /// <summary>
        /// Constructs a GraphBuildRequestData for build requests based on a project graph.
        /// </summary>
        /// <param name="projectGraph">The graph to build.</param>
        /// <param name="targetsToBuild">The targets to build.</param>
        /// <param name="hostServices">The host services to use, if any.  May be null.</param>
        /// <param name="flags">Flags controlling this build request.</param>
        public GraphBuildRequestData(ProjectGraph projectGraph, ICollection<string> targetsToBuild, HostServices hostServices, BuildRequestDataFlags flags)
            : this(targetsToBuild, hostServices, flags)
        {
            ErrorUtilities.VerifyThrowArgumentNull(projectGraph, nameof(projectGraph));

            ProjectGraph = projectGraph;
        }

        /// <summary>
        /// Constructs a GraphBuildRequestData for build requests based on project files.
        /// </summary>
        /// <param name="projectFullPath">The full path to the project file.</param>
        /// <param name="globalProperties">The global properties which should be used during evaluation of the project.  Cannot be null.</param>
        /// <param name="targetsToBuild">The targets to build.</param>
        /// <param name="hostServices">The host services to use.  May be null.</param>
        public GraphBuildRequestData(string projectFullPath, IDictionary<string, string> globalProperties, ICollection<string> targetsToBuild, HostServices hostServices)
            : this(new ProjectGraphEntryPoint(projectFullPath, globalProperties).AsEnumerable(), targetsToBuild, hostServices, BuildRequestDataFlags.None)
        {
        }

        /// <summary>
        /// Constructs a GraphBuildRequestData for build requests based on project files.
        /// </summary>
        /// <param name="projectFullPath">The full path to the project file.</param>
        /// <param name="globalProperties">The global properties which should be used during evaluation of the project.  Cannot be null.</param>
        /// <param name="targetsToBuild">The targets to build.</param>
        /// <param name="hostServices">The host services to use.  May be null.</param>
        /// <param name="flags">The <see cref="BuildRequestDataFlags"/> to use.</param>
        public GraphBuildRequestData(string projectFullPath, IDictionary<string, string> globalProperties, ICollection<string> targetsToBuild, HostServices hostServices, BuildRequestDataFlags flags)
            : this(new ProjectGraphEntryPoint(projectFullPath, globalProperties).AsEnumerable(), targetsToBuild, hostServices, flags)
        {
        }

        /// <summary>
        /// Constructs a GraphBuildRequestData for build requests based on a project graph entry points.
        /// </summary>
        /// <param name="projectGraphEntryPoint">The entry point to use in the build.</param>
        /// <param name="targetsToBuild">The targets to build.</param>
        public GraphBuildRequestData(ProjectGraphEntryPoint projectGraphEntryPoint, ICollection<string> targetsToBuild)
            : this(projectGraphEntryPoint.AsEnumerable(), targetsToBuild, null, BuildRequestDataFlags.None)
        {
        }

        /// <summary>
        /// Constructs a GraphBuildRequestData for build requests based on a project graph entry points.
        /// </summary>
        /// <param name="projectGraphEntryPoint">The entry point to use in the build.</param>
        /// <param name="targetsToBuild">The targets to build.</param>
        /// <param name="hostServices">The host services to use, if any.  May be null.</param>
        public GraphBuildRequestData(ProjectGraphEntryPoint projectGraphEntryPoint, ICollection<string> targetsToBuild, HostServices hostServices)
            : this(projectGraphEntryPoint.AsEnumerable(), targetsToBuild, hostServices, BuildRequestDataFlags.None)
        {
        }

        /// <summary>
        /// Constructs a GraphBuildRequestData for build requests based on a project graph entry points.
        /// </summary>
        /// <param name="projectGraphEntryPoint">The entry point to use in the build.</param>
        /// <param name="targetsToBuild">The targets to build.</param>
        /// <param name="hostServices">The host services to use, if any.  May be null.</param>
        /// <param name="flags">Flags controlling this build request.</param>
        public GraphBuildRequestData(ProjectGraphEntryPoint projectGraphEntryPoint, ICollection<string> targetsToBuild, HostServices hostServices, BuildRequestDataFlags flags)
            : this(projectGraphEntryPoint.AsEnumerable(), targetsToBuild, hostServices, flags)
        {
        }

        /// <summary>
        /// Constructs a GraphBuildRequestData for build requests based on a project graph entry points.
        /// </summary>
        /// <param name="projectGraphEntryPoints">The entry points to use in the build.</param>
        /// <param name="targetsToBuild">The targets to build.</param>
        public GraphBuildRequestData(IEnumerable<ProjectGraphEntryPoint> projectGraphEntryPoints, ICollection<string> targetsToBuild)
            : this(projectGraphEntryPoints, targetsToBuild, null, BuildRequestDataFlags.None)
        {
        }

        /// <summary>
        /// Constructs a GraphBuildRequestData for build requests based on a project graph entry points.
        /// </summary>
        /// <param name="projectGraphEntryPoints">The entry points to use in the build.</param>
        /// <param name="targetsToBuild">The targets to build.</param>
        /// <param name="hostServices">The host services to use, if any.  May be null.</param>
        public GraphBuildRequestData(IEnumerable<ProjectGraphEntryPoint> projectGraphEntryPoints, ICollection<string> targetsToBuild, HostServices hostServices)
            : this(projectGraphEntryPoints, targetsToBuild, hostServices, BuildRequestDataFlags.None)
        {
        }

        /// <summary>
        /// Constructs a GraphBuildRequestData for build requests based on a project graph entry points.
        /// </summary>
        /// <param name="projectGraphEntryPoints">The entry points to use in the build.</param>
        /// <param name="targetsToBuild">The targets to build.</param>
        /// <param name="hostServices">The host services to use, if any.  May be null.</param>
        /// <param name="flags">Flags controlling this build request.</param>
        public GraphBuildRequestData(IEnumerable<ProjectGraphEntryPoint> projectGraphEntryPoints, ICollection<string> targetsToBuild, HostServices hostServices, BuildRequestDataFlags flags)
            : this(targetsToBuild, hostServices, flags)
        {
            ErrorUtilities.VerifyThrowArgumentNull(projectGraphEntryPoints, nameof(projectGraphEntryPoints));

            ProjectGraphEntryPoints = projectGraphEntryPoints;
        }

        /// <summary>
        /// Common constructor.
        /// </summary>
        private GraphBuildRequestData(ICollection<string> targetsToBuild, HostServices hostServices, BuildRequestDataFlags flags)
        {
            ErrorUtilities.VerifyThrowArgumentNull(targetsToBuild, nameof(targetsToBuild));
            foreach (string targetName in targetsToBuild)
            {
                ErrorUtilities.VerifyThrowArgumentNull(targetName, "target");
            }

            HostServices = hostServices;
            TargetNames = new List<string>(targetsToBuild);
            Flags = flags;
        }

        /// <summary>
        /// The requested project graph to build.
        /// May be null.
        /// </summary>
        /// <value>The project graph.</value>
        public ProjectGraph ProjectGraph { get; }

        /// <summary>
        /// The project graph entry points.
        /// May be null.
        /// </summary>
        /// <value>The project graph entry points.</value>
        public IEnumerable<ProjectGraphEntryPoint> ProjectGraphEntryPoints { get; }

        /// <summary>
        /// The name of the targets to build.
        /// </summary>
        /// <value>An array of targets in the project to be built.</value>
        public ICollection<string> TargetNames { get; }

        /// <summary>
        /// Extra flags for this BuildRequest.
        /// </summary>
        public BuildRequestDataFlags Flags { get; }

        /// <summary>
        /// Gets the HostServices object for this request.
        /// </summary>
        public HostServices HostServices { get; }
    }
}
