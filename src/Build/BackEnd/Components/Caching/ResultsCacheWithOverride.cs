// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;
using net.r_eg.IeXod.BackEnd;
using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod.Execution
{
    internal class ResultsCacheWithOverride : IResultsCache
    {
        private readonly IResultsCache _override;
        public ResultsCache CurrentCache { get; }


        public ResultsCacheWithOverride(IResultsCache @override)
        {
            _override = @override;
            CurrentCache = new ResultsCache();
        }

        public void InitializeComponent(IBuildComponentHost host)
        {
            CurrentCache.InitializeComponent(host);
        }

        public void ShutdownComponent()
        {
            CurrentCache.ShutdownComponent();
        }

        public void Translate(ITranslator translator)
        {
            ErrorUtilities.ThrowInternalErrorUnreachable();
        }

        public void AddResult(BuildResult result)
        {
            CurrentCache.AddResult(result);
        }

        public void ClearResults()
        {
            CurrentCache.ClearResults();
        }

        public BuildResult GetResultForRequest(BuildRequest request)
        {
            var overrideResult = _override.GetResultForRequest(request);
            if (overrideResult != null)
            {
#if DEBUG
                ErrorUtilities.VerifyThrow(CurrentCache.GetResultForRequest(request) == null, "caches should not overlap");
#endif
                return overrideResult;
            }

            return CurrentCache.GetResultForRequest(request);
        }

        public BuildResult GetResultsForConfiguration(int configurationId)
        {
            var overrideResult = _override.GetResultsForConfiguration(configurationId);
            if (overrideResult != null)
            {
#if DEBUG
                ErrorUtilities.VerifyThrow(CurrentCache.GetResultsForConfiguration(configurationId) == null, "caches should not overlap");
#endif
                return overrideResult;
            }

            return CurrentCache.GetResultsForConfiguration(configurationId);
        }

        public ResultsCacheResponse SatisfyRequest(
            BuildRequest request,
            List<string> configInitialTargets,
            List<string> configDefaultTargets,
            bool skippedResultsDoNotCauseCacheMiss)
        {
            var overrideRequest = _override.SatisfyRequest(
                request,
                configInitialTargets,
                configDefaultTargets,
                skippedResultsDoNotCauseCacheMiss);

            if (overrideRequest.Type == ResultsCacheResponseType.Satisfied)
            {
#if DEBUG
                ErrorUtilities.VerifyThrow(
                    CurrentCache.SatisfyRequest(
                        request,
                        configInitialTargets,
                        configDefaultTargets,
                        skippedResultsDoNotCauseCacheMiss)
                        .Type == ResultsCacheResponseType.NotSatisfied,
                    "caches should not overlap");
#endif
                return overrideRequest;
            }

            return CurrentCache.SatisfyRequest(
                request,
                configInitialTargets,
                configDefaultTargets,
                skippedResultsDoNotCauseCacheMiss);
        }

        public void ClearResultsForConfiguration(int configurationId)
        {
            CurrentCache.ClearResultsForConfiguration(configurationId);
        }

        public void WriteResultsToDisk()
        {
            CurrentCache.WriteResultsToDisk();
        }

        public IEnumerator<BuildResult> GetEnumerator()
        {
            return CurrentCache.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
