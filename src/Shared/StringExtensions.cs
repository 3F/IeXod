// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;

namespace net.r_eg.IeXod.Shared
{
    internal static class StringExtensions
    {
        public static string Replace(this string aString, string oldValue, string newValue, StringComparison stringComparison)
        {
            ErrorUtilities.VerifyThrowArgumentNull(aString, nameof(aString));
            ErrorUtilities.VerifyThrowArgumentNull(oldValue, nameof(oldValue));
            ErrorUtilities.VerifyThrowArgumentLength(oldValue, nameof(oldValue));

            if (newValue == null)
            {
                newValue = string.Empty;
            }

            var currentOccurrence = aString.IndexOf(oldValue, stringComparison);

            if (currentOccurrence == -1)
            {
                return aString;
            }

            var endOfPreviousOccurrence = 0;

            // Assumes one match. Optimizes for replacing fallback property values (e.g. MSBuildExtensionsPath), where an import usually references the fallback property once.
            // Reduces memory usage by half.
            var builder = new StringBuilder(aString.Length - oldValue.Length + newValue.Length);

            while (currentOccurrence != -1)
            {
                var nonMatchLength = currentOccurrence - endOfPreviousOccurrence;
                builder.Append(aString, endOfPreviousOccurrence, nonMatchLength);
                builder.Append(newValue);

                endOfPreviousOccurrence = currentOccurrence + oldValue.Length;
                currentOccurrence = aString.IndexOf(oldValue, endOfPreviousOccurrence, stringComparison);
            }

            builder.Append(aString, endOfPreviousOccurrence, aString.Length - endOfPreviousOccurrence);

            return builder.ToString();
        }
    }
}
