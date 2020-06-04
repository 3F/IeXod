// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
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
    /// This class is used to store information needed to interpret the response from
    /// the parent engine when it completes the requested evaluation
    /// </summary>
    internal class NodeRequestMapping
    {
        #region Constructors

        internal NodeRequestMapping
            (int handleId, int requestId, CacheScope resultsCache )
        {
            ErrorUtilities.VerifyThrow(resultsCache != null, "Expect a non-null build result");
            this.handleId = handleId;
            this.requestId = requestId;
            this.resultsCache = resultsCache;
        }
        #endregion

        #region Properties
        internal int HandleId
        {
            get
            {
                return this.handleId;
            }
        }

        internal int RequestId
        {
            get
            {
                return this.requestId;
            }
        }
        #endregion

        #region Methods
        internal void AddResultToCache(BuildResult buildResult)
        {
            resultsCache.AddCacheEntryForBuildResults(buildResult);
        }
        #endregion

        #region Member Data
        private int handleId;
        private int requestId;
        private CacheScope resultsCache;
        #endregion
    }
}
