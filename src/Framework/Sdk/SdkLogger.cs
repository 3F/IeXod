// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace net.r_eg.IeXod.Sdk
{
    /// <summary>
    ///     An abstract interface class to providing real-time logging and status while resolving
    ///     an SDK.
    /// </summary>
    public abstract class SdkLogger
    {
        /// <summary>
        ///     Log a build message to MSBuild.
        /// </summary>
        /// <param name="message">Message string.</param>
        /// <param name="verbosity">Optional message importances. Default to low.</param>
        public abstract void LogMessage(string message, SdkLogVerbosity verbosity = SdkLogVerbosity.Low);
    }
}
