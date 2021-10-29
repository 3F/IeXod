// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using net.r_eg.IeXod.BackEnd;
using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod.BackEnd.Logging
{
    /// <summary>
    /// Factory to create components of the type LoggingService
    /// </summary>
    internal class LoggingServiceFactory
    {
        #region Data

        /// <summary>
        /// What kind of LoggerMode are the logging services when created.
        /// They could be Synchronous or Asynchronous
        /// </summary>
        private LoggerMode _logMode = LoggerMode.Synchronous;

        /// <summary>
        /// What node is this logging service being created on.
        /// </summary>
        private int _nodeId = 0;
        #endregion

        #region Constructor

        /// <summary>
        /// Tell the factory what kind of logging services is should create
        /// </summary>
        /// <param name="mode">Synchronous or Asynchronous</param>
        /// <param name="nodeId">The node identifier.</param>
        internal LoggingServiceFactory(LoggerMode mode, int nodeId)
        {
            _logMode = mode;
            _nodeId = nodeId;
        }

        #endregion
        #region Members

        /// <summary>
        /// Create an instance of a LoggingService and returns is as an IBuildComponent
        /// </summary>
        /// <returns>An instance of a LoggingService as a IBuildComponent</returns>
        public IBuildComponent CreateInstance(BuildComponentType type)
        {
            ErrorUtilities.VerifyThrow(type == BuildComponentType.LoggingService, "Cannot create components of type {0}", type);
            IBuildComponent loggingService = (IBuildComponent)LoggingService.CreateLoggingService(_logMode, _nodeId);
            return loggingService;
        }

        #endregion
    }
}
