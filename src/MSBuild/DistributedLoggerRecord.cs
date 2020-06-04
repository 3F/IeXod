// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Shared;
using net.r_eg.IeXod.Logging;

namespace net.r_eg.IeXod.CommandLine
{
    /// <summary>
    /// This class is a container class used to pass around information about distributed logger
    /// </summary>
    internal class DistributedLoggerRecord
    {
        #region Constructors
        /// <summary>
        /// Initialize the container class with the given centralLogger and forwardingLoggerDescription
        /// </summary>
        internal DistributedLoggerRecord(ILogger centralLogger, LoggerDescription forwardingLoggerDescription)
        {
            _centralLogger = centralLogger;
            _forwardingLoggerDescription = forwardingLoggerDescription;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Fully initialized central logger
        /// </summary>
        internal ILogger CentralLogger
        {
            get
            {
                return _centralLogger;
            }
        }

        /// <summary>
        /// Description of the forwarding class
        /// </summary>
        internal LoggerDescription ForwardingLoggerDescription
        {
            get
            {
                return _forwardingLoggerDescription;
            }
        }
        #endregion

        #region Data
        // Central logger
        private ILogger _centralLogger;
        // Description of the forwarding logger
        private LoggerDescription _forwardingLoggerDescription;
        #endregion
    }
}
