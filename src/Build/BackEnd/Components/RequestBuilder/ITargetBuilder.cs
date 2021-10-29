// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using net.r_eg.IeXod.BackEnd.Logging;
using BuildResult = net.r_eg.IeXod.Execution.BuildResult;

namespace net.r_eg.IeXod.BackEnd
{
    /// <summary>
    /// Represents an object which can build targets for a project.
    /// </summary>
    internal interface ITargetBuilder
    {
        /// <summary>
        /// Builds the specified targets.
        /// </summary>
        /// <param name="projectLoggingContext">The logging context for the project.</param>
        /// <param name="entry">The BuildRequestEntry for which we are building targets.</param>
        /// <param name="callback">The callback to be used to handle new project build requests.</param>
        /// <param name="targets">The targets to build.</param>
        /// <param name="baseLookup">The Lookup containing all current items and properties for this target.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel processing of targets.</param>
        /// <returns>A Task representing the work to be done.</returns>
        Task<BuildResult> BuildTargets(ProjectLoggingContext projectLoggingContext, BuildRequestEntry entry, IRequestBuilderCallback callback, string[] targets, Lookup baseLookup, CancellationToken cancellationToken);
    }
}
