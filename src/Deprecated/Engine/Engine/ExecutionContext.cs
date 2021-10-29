// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.BuildEngine.Shared;

namespace net.r_eg.IeXod.BuildEngine
{
    /// <summary>
    /// Base class for data container shared between the Engine data domain and the TaskExecutionModule (TEM)
    /// data domain
    /// </summary>
    internal class ExecutionContext
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        internal ExecutionContext(int handleId, int nodeIndex, BuildEventContext buildEventContext)
        {
            this.handleId  = handleId;
            this.nodeIndex = nodeIndex;
            this.buildEventContext = buildEventContext;
        }
        #endregion

        #region Properties

        /// <summary>
        /// The token id corresponding to this context
        /// </summary>
        internal int HandleId
        {
            get
            {
                return this.handleId;
            }
        }

        /// <summary>
        /// The node on which this context is being executed 
        /// </summary>
        internal int NodeIndex
        {
            get
            {
                return this.nodeIndex;
            }
        }

        /// <summary>
        /// The logging context
        /// </summary>
        internal BuildEventContext BuildEventContext
        {
            get
            {
                return this.buildEventContext;
            }
        }

        #endregion

        #region Data
        // The handle for the context
        private int handleId;
        // The node of execution
        private int nodeIndex;
        // The logging context
        private BuildEventContext buildEventContext;
        #endregion
    }
}
