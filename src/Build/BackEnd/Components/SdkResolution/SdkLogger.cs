// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.BackEnd.Logging;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Sdk;

using SdkLoggerBase = net.r_eg.IeXod.Sdk.SdkLogger;

namespace net.r_eg.IeXod.BackEnd.SdkResolution
{
    /// <summary>
    /// An internal implementation of <see cref="Framework.SdkLogger"/>.
    /// </summary>
    internal class SdkLogger : SdkLoggerBase
    {
        private readonly LoggingContext _loggingContext;

        public SdkLogger(LoggingContext loggingContext)
        {
            _loggingContext = loggingContext;
        }

        public override void LogMessage(string message, SdkLogVerbosity verbosity = SdkLogVerbosity.Low)
        {
            _loggingContext.LogCommentFromText(GetImportance(verbosity), message);
        }

        private MessageImportance GetImportance(SdkLogVerbosity verbosity)
        {
            switch(verbosity)
            {
                case SdkLogVerbosity.High: return MessageImportance.High;
                case SdkLogVerbosity.Normal: return MessageImportance.Normal;
                case SdkLogVerbosity.Low: return MessageImportance.Low;
            }
            return MessageImportance.Low;
        }
    }
}
