// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod.Shared.EscapingStringExtensions
{
    internal static class EscapingStringExtensions
    {
        internal static string Unescape(this string escapedString)
        {
            return EscapingUtilities.UnescapeAll(escapedString);
        }

        internal static string Unescape
        (
            this string escapedString,
            out bool escapingWasNecessary
        )
        {
            return EscapingUtilities.UnescapeAll(escapedString, out escapingWasNecessary);
        }

        internal static string Escape(this string unescapedString)
        {
            return EscapingUtilities.Escape(unescapedString);
        }

        internal static bool ContainsEscapedWildcards(this string escapedString)
        {
            return EscapingUtilities.ContainsEscapedWildcards(escapedString);
        }
    }
}