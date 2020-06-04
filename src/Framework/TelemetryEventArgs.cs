// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace net.r_eg.IeXod.Framework
{
    /// <summary>
    /// Arguments for telemetry events.
    /// </summary>
    [Serializable]
    public sealed class TelemetryEventArgs : BuildEventArgs
    {
        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// Gets or sets a list of properties associated with the event.
        /// </summary>
        public IDictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
    }
}
