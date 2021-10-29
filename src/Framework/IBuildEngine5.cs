// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace net.r_eg.IeXod.Framework
{
    /// <summary>
    /// This interface extends IBuildEngine to log telemetry.
    /// </summary>
    public interface IBuildEngine5 : IBuildEngine4
    {
        /// <summary>
        /// Logs telemetry.
        /// </summary>
        /// <param name="eventName">The event name.</param>
        /// <param name="properties">The event properties.</param>
        void LogTelemetry(string eventName, IDictionary<string, string> properties);
    }
}