﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using net.r_eg.IeXod.Execution;
using net.r_eg.IeXod.Collections;
using System.Threading.Tasks;

namespace net.r_eg.IeXod.BackEnd
{
    /// <summary>
    /// This interface is passed to objects which might need to call back into the request builder, such as the Target and Task builders
    /// </summary>
    internal interface IRequestBuilderCallback
    {
        /// <summary>
        /// This method instructs the request builder to build the specified projects using the specified parameters.  This is
        /// what is ultimately used by something like an MSBuild task which needs to invoke a project-to-project reference.  IBuildEngine
        /// and IBuildEngine2 have BuildProjectFile methods which boil down to an invocation of this method as well.
        /// </summary>
        /// <param name="projectFiles">An array of projects to be built.</param>
        /// <param name="properties">The property groups to use for each project.  Must be the same number as there are project files.</param>
        /// <param name="toolsVersions">The tools version to use for each project.  Must be the same number as there are project files.</param>
        /// <param name="targets">The targets to be built.  Each project will be built with the same targets.</param>
        /// <param name="waitForResults">True to wait for the results </param>
        /// <param name="skipNonexistentTargets">If set, skip targets that are not defined in the projects to be built.</param>
        /// <returns>An Task representing the work which will be done.</returns>
        Task<BuildResult[]> BuildProjects(string[] projectFiles, PropertyDictionary<ProjectPropertyInstance>[] properties, string[] toolsVersions, string[] targets, bool waitForResults, bool skipNonexistentTargets = false);

        /// <summary>
        /// This method instructs the request builder that the target builder is blocked on a target which is already in progress on the
        /// configuration due to another request.
        /// </summary>
        /// <param name="blockingRequestId">The request on which we are blocked.</param>
        /// <param name="blockingTarget">The target on which we are blocked.</param>
        /// <param name="partialBuildResult">Results so far from the target builder that's blocking</param>
        Task BlockOnTargetInProgress(int blockingRequestId, string blockingTarget, BuildResult partialBuildResult);

        /// <summary>
        /// Instructs the RequestBuilder that it may yield its control of the node.
        /// </summary>
        void Yield();

        /// <summary>
        /// Instructs the RequestBuilder to suspend until the node is reacquired.
        /// </summary>
        void Reacquire();

        /// <summary>
        /// Instructs the RequestBuilder that next Build request from a task should post its request
        /// and immediately return so that the thread may be freed up.  May not be nested.
        /// </summary>
        void EnterMSBuildCallbackState();

        /// <summary>
        /// Exits the previous MSBuild callback state.
        /// </summary>
        void ExitMSBuildCallbackState();
    }
}
