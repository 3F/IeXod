// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod.BackEnd.Logging
{
    /// <summary>
    /// This class will throw an exception when it receives any event except for the build started or build finished event
    /// this logger is good to use if a distributed logger is attached but does not want to forward any events
    /// </summary>
    internal class NullCentralLogger : INodeLogger
    {
        #region Data
        private string _parameters;
        private LoggerVerbosity _verbosity;
        #endregion

        #region Properties
        public LoggerVerbosity Verbosity
        {
            get
            {
                return _verbosity;
            }
            set
            {
                _verbosity = value;
            }
        }

        public string Parameters
        {
            get
            {
                return _parameters;
            }
            set
            {
                _parameters = value;
            }
        }
        #endregion

        #region Methods
        public void Initialize(IEventSource eventSource, int nodeCount)
        {
            eventSource.AnyEventRaised += new AnyEventHandler(AnyEventRaisedHandler);
        }

        public void AnyEventRaisedHandler(object sender, BuildEventArgs e)
        {
            if (!(e is BuildStartedEventArgs) && !(e is BuildFinishedEventArgs))
            {
                ErrorUtilities.VerifyThrowInvalidOperation(false, "Should not receive any events other than build started or finished");
            }
        }

        public void Initialize(IEventSource eventSource)
        {
            Initialize(eventSource, 1);
        }

        public void Shutdown()
        {
            // do nothing
        }
        #endregion
    }
}
