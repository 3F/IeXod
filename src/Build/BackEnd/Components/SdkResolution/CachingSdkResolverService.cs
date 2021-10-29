// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using net.r_eg.IeXod.BackEnd.Logging;
using net.r_eg.IeXod.Collections;
using net.r_eg.IeXod.Construction;
using net.r_eg.IeXod.Sdk;
using net.r_eg.IeXod.Shared;
using net.r_eg.IeXod.Utilities;

namespace net.r_eg.IeXod.BackEnd.SdkResolution
{

    internal sealed class CachingSdkResolverService: SdkResolverService
    {
        /// <summary>
        /// Stores the cache in a set of concurrent dictionaries.  The main dictionary is by build submission ID and the inner dictionary contains a case-insensitive SDK name and the cached <see cref="SdkResult"/>.
        /// </summary>
        private readonly ConcurrentDictionary<int, ConcurrentDictionary<string, Lazy<SdkResult>>> _cache = new ConcurrentDictionary<int, ConcurrentDictionary<string, Lazy<SdkResult>>>();

        public override void ClearCache(int submissionId)
        {
            base.ClearCache(submissionId);

            _cache.TryRemove(submissionId, out _);
        }

        public override void ClearCaches()
        {
            base.ClearCaches();

            _cache.Clear();
        }

        public override SdkResult ResolveSdk(int submissionId, SdkReference sdk, LoggingContext loggingContext, ElementLocation sdkReferenceLocation, SdkEnv sdkEnv, bool interactive)
        {
            SdkResult result;

            if (Traits.Instance.EscapeHatches.DisableSdkResolutionCache)
            {
                result = base.ResolveSdk(submissionId, sdk, loggingContext, sdkReferenceLocation, sdkEnv, interactive);
            }
            else
            {
                // Get the dictionary for the specified submission if one is already added otherwise create a new dictionary for the submission.
                ConcurrentDictionary<string, Lazy<SdkResult>> cached = _cache.GetOrAdd(submissionId, new ConcurrentDictionary<string, Lazy<SdkResult>>(MSBuildNameIgnoreCaseComparer.Default));

                /*
                 * Get a Lazy<SdkResult> if available, otherwise create a Lazy<SdkResult> which will resolve the SDK with the SdkResolverService.Instance.  If multiple projects are attempting to resolve
                 * the same SDK, they will all get back the same Lazy<SdkResult> which ensures that a single build submission resolves each unique SDK only one time.
                 */
                Lazy<SdkResult> resultLazy = cached.GetOrAdd(
                    sdk.Name,
                    key => new Lazy<SdkResult>(() => base.ResolveSdk(submissionId, sdk, loggingContext, sdkReferenceLocation, sdkEnv, interactive)));

                // Get the lazy value which will block all waiting threads until the SDK is resolved at least once while subsequent calls get cached results.
                result = resultLazy.Value;
            }

            if (result != null &&
                !SdkResolverService.IsReferenceSameVersion(sdk, result.SdkReference.Version) &&
                !SdkResolverService.IsReferenceSameVersion(sdk, result.Version))
            {
                // MSB4240: Multiple versions of the same SDK "{0}" cannot be specified. The previously resolved SDK version "{1}" from location "{2}" will be used and the version "{3}" will be ignored.
                loggingContext.LogWarning(null, new BuildEventFileInfo(sdkReferenceLocation), "ReferencingMultipleVersionsOfTheSameSdk", sdk.Name, result.Version, result.ElementLocation, sdk.Version);
            }

            return result;
        }
    }
}
