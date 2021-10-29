// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod.Logging
{
    /// <summary>
    /// This class descibes a central/forwarding logger pair used in multiproc logging.
    /// </summary>
    public class ForwardingLoggerRecord
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="centralLogger">The central logger</param>
        /// <param name="forwardingLoggerDescription">The description for the forwarding logger.</param>
        public ForwardingLoggerRecord(ILogger centralLogger, LoggerDescription forwardingLoggerDescription)
        {
            // The logging service allows a null central logger, so we don't check for it here.
            ErrorUtilities.VerifyThrowArgumentNull(forwardingLoggerDescription, "forwardingLoggerDescription");

            this.CentralLogger = centralLogger;
            this.ForwardingLoggerDescription = forwardingLoggerDescription;
        }

        /// <summary>
        /// Retrieves the central logger.
        /// </summary>
        public ILogger CentralLogger
        {
            get;
            private set;
        }

        /// <summary>
        /// Retrieves the forwarding logger description.
        /// </summary>
        public LoggerDescription ForwardingLoggerDescription
        {
            get;
            private set;
        }
    }
}
