// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace net.r_eg.IeXod.Execution
{
    #region Enums
    /// <summary>
    /// Reasons for a node to shutdown.
    /// </summary>
    public enum NodeEngineShutdownReason
    {
        /// <summary>
        /// The BuildManager sent a command instructing the node to terminate.
        /// </summary>
        BuildComplete,

        /// <summary>
        /// The BuildManager sent a command instructing the node to terminate, but to restart for reuse.
        /// </summary>
        BuildCompleteReuse,

        /// <summary>
        /// The communication link failed.
        /// </summary>
        ConnectionFailed,

        /// <summary>
        /// The NodeEngine caught an exception which requires the Node to shut down.
        /// </summary>
        Error,
    }
    #endregion
}