// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

using net.r_eg.IeXod.Framework;

namespace net.r_eg.IeXod.BuildEngine
{
    internal interface IEngineCallback
    {
        /// <summary>
        /// This method is called by a child engine or node provider to request the parent engine
        /// to build a certain part of the tree which is needed to complete an earlier request
        /// received from the parent engine. The parent engine is expected to
        /// pass back buildResult once the build is completed
        /// </summary>
        void PostBuildRequestsToHost(BuildRequest[] buildRequests);

        /// <summary>
        /// This method is called to send results to the parent engine in response to an earlier 
        /// build request.
        /// </summary>
        /// <param name="buildResult"></param>
        void PostBuildResultToHost(BuildResult buildResult);

        /// <summary>
        /// This method is used to send logging events to the parent engine
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="nodeLoggingEventArray"></param>
        void PostLoggingMessagesToHost(int nodeId, NodeLoggingEvent[] nodeLoggingEventArray);

        /// <summary>
        /// Posts the given set of cache entries to the parent engine.
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="entries"></param>
        /// <param name="scopeName"></param>
        /// <param name="scopeProperties"></param>
        /// <param name="scopeToolsVersion"></param>
        Exception PostCacheEntriesToHost(int nodeId, CacheEntry[] entries, string scopeName, BuildPropertyGroup scopeProperties, string scopeToolsVersion, CacheContentType cacheContentType);

        /// <summary>
        /// Retrieves the requested set of cache entries from the engine.
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="names"></param>
        /// <param name="scopeName"></param>
        /// <param name="scopeProperties"></param>
        /// <param name="scopeToolsVersion"></param>
        /// <returns></returns>
        CacheEntry[] GetCachedEntriesFromHost(int nodeId, string[] names, string scopeName, BuildPropertyGroup scopeProperties, string scopeToolsVersion, CacheContentType cacheContentType);

        /// <summary>
        /// This method is called to post current status to the parent
        /// </summary>
        /// <param name="nodeId"> The identifer for the node </param>
        /// <param name="nodeStatus">The filled out status structure</param>
        /// <param name="blockUntilSent">If true the call will not return until the data has been
        ///                              written out.</param>
        void PostStatus(int nodeId, NodeStatus nodeStatus, bool blockUntilSent);
    }
}
