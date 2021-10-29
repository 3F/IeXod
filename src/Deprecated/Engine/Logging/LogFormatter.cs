// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Text;
using System.IO;
using net.r_eg.IeXod.Framework;
using System.Globalization;

namespace net.r_eg.IeXod.BuildEngine
{
    /// <summary>
    /// Utility helper functions for formatting logger output.
    /// </summary>
    /// <owner>JomoF</owner>
    static internal class LogFormatter 
    {
        /// <summary>
        /// Formats the timestamp in the log as  Hours:Minutes:Seconds.Milliseconds
        /// </summary>
        internal static string FormatLogTimeStamp(DateTime timeStamp)
        {
            // From http://msdn2.microsoft.com/en-us/library/8kb3ddd4.aspx
            // Custom DateTime Format Strings  
            //
            // HH Represents the hour as a number from 00 through 23, that is,
            //    the hour as represented by a zero-based 24-hour clock that counts the hours since midnight.
            //    A single-digit hour is formatted with a leading zero.
            //
            // mm Represents the minute as a number from 00 through 59. The minute represents whole minutes
            //    passed since the last hour. A single-digit minute is formatted with a leading zero.
            //
            // ss Represents the seconds as a number from 00 through 59. The second represents whole seconds passed
            //    since the last minute. A single-digit second is formatted with a leading zero.
            //
            // fff Represents the three most significant digits of the seconds fraction. Trailing zeros are displayed.
            //     Since milliseconds are 1 / 1000 of a second we need to display 3 digits.

            return timeStamp.ToString("HH:mm:ss.fff", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Formats a timespan for logger output.
        /// </summary>
        /// <owner>JomoF</owner>
        /// <param name="t"></param>
        /// <returns>String representation of time-span.</returns>
        internal static string FormatTimeSpan(TimeSpan t) 
        {
            string rawTime = t.ToString(); // Timespan is a value type and can't be null.
            int rawTimeLength = rawTime.Length;
            int prettyLength = System.Math.Min(11, rawTimeLength);
            return t.ToString().Substring(0, prettyLength);
        }
    }
}
