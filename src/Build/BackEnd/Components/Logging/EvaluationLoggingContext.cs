// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.BackEnd.Logging;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod.BackEnd.Components.Logging
{
    /// <summary>
    ///     Logging context and helpers for evaluation logging
    /// </summary>
    internal class EvaluationLoggingContext : LoggingContext
    {
        private readonly string _projectFile;

        public EvaluationLoggingContext(ILoggingService loggingService, BuildEventContext buildEventContext, string projectFile) :
            base(
                loggingService,
                loggingService.CreateEvaluationBuildEventContext(buildEventContext.NodeId, buildEventContext.SubmissionId))
        {
            _projectFile = projectFile;
            IsValid = true;
        }

        public void LogProjectEvaluationStarted()
        {
            LoggingService.LogProjectEvaluationStarted(BuildEventContext, _projectFile);
        }

        /// <summary>
        /// Log that the project has finished
        /// </summary>
        internal void LogProjectEvaluationFinished()
        {
            ErrorUtilities.VerifyThrow(IsValid, "invalid");
            LoggingService.LogProjectEvaluationFinished(BuildEventContext, _projectFile);
            IsValid = false;
        }
    }
}
