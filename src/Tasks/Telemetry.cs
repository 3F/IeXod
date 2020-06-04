// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Shared;
using System;
using System.Collections.Generic;

namespace net.r_eg.IeXod.Tasks
{
    /// <summary>
    /// Task that logs telemetry.
    /// </summary>
    public sealed class Telemetry : TaskExtension
    {
        /// <summary>
        /// Gets or sets a semi-colon delimited list of equal-sign separated key/value pairs.  An example would be &quot;Property1=Value1;Property2=Value2&quot;.
        /// </summary>
        public string EventData { get; set; }

        /// <summary>
        /// Gets or sets the event name.
        /// </summary>
        [Required]
        public string EventName { get; set; }

        /// <summary>
        /// Main task method
        /// </summary>
        public override bool Execute()
        {
            IDictionary<string, string> properties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (!String.IsNullOrEmpty(EventData))
            {
                foreach (string pair in EventData.Split(MSBuildConstants.SemicolonChar, StringSplitOptions.RemoveEmptyEntries))
                {
                    var item = pair.Split(MSBuildConstants.EqualsChar, 2, StringSplitOptions.RemoveEmptyEntries);

                    if (item.Length != 2)
                    {
                        Log.LogMessageFromResources(MessageImportance.Low, "Telemetry.IllegalEventDataString", pair, EventData);
                        return false;
                    }

                    // Last value added wins
                    //
                    properties[item[0]] = item[1];
                }
            }

            Log.LogTelemetry(EventName, properties);

            return !Log.HasLoggedErrors;
        }
    }
}
