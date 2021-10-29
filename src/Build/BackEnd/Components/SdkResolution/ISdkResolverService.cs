// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.BackEnd.Logging;
using net.r_eg.IeXod.Construction;
using net.r_eg.IeXod.Sdk;
using System;

namespace net.r_eg.IeXod.BackEnd.SdkResolution
{
    /// <summary>
    /// An interface for services which resolve SDKs.
    /// </summary>
    internal interface ISdkResolverService
    {
        /// <summary>
        /// A method to use when sending packets to a remote host.
        /// </summary>
        Action<INodePacket> SendPacket { get; }

        /// <summary>
        /// Clears the cache for the specified build submission ID.
        /// </summary>
        /// <param name="submissionId">The build submission ID to clear from the cache.</param>
        void ClearCache(int submissionId);

        /// <summary>
        /// Clear the entire cache
        /// </summary>
        void ClearCaches();

        /// <summary>
        ///  Resolves the full path to the specified SDK.
        /// </summary>
        /// <param name="submissionId">The build submission ID that the resolution request is for.</param>
        /// <param name="sdk">The <see cref="SdkReference"/> containing information about the referenced SDK.</param>
        /// <param name="loggingContext">The <see cref="LoggingContext"/> to use when logging messages during resolution.</param>
        /// <param name="sdkReferenceLocation">The <see cref="ElementLocation"/> of the element which referenced the SDK.</param>
        /// <param name="sdkEnv">An accompanying environment during resolution.</param>
        /// <param name="interactive">Indicates whether or not the resolver is allowed to be interactive.</param>
        /// <returns>An <see cref="SdkResult"/> containing information about the resolved SDK. If no resolver was able to resolve it, then <see cref="Framework.SdkResult.Success"/> == false. </returns>
        SdkResult ResolveSdk(int submissionId, SdkReference sdk, LoggingContext loggingContext, ElementLocation sdkReferenceLocation, SdkEnv sdkEnv, bool interactive);
    }
}
