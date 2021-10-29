// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod.Tasks
{
    /// <summary>
    /// A place the resolver tried to look for an assembly along with some information
    /// that can be used to provide a good error message.
    /// </summary>
    internal class ResolutionSearchLocation
    {
        /// <summary>
        /// The name of the file that was attempted to match.
        /// </summary>
        internal string FileNameAttempted { get; set; }

        /// <summary>
        /// The literal searchpath element that was used to discover this location.
        /// </summary>
        internal string SearchPath { get; set; }

        /// <summary>
        /// The name of the assembly found at that location. Will be null if there was no assembly there.
        /// </summary>
        internal AssemblyNameExtension AssemblyName { get; set; }

        /// <summary>
        /// The reason there was no macth.
        /// </summary>
        internal NoMatchReason Reason { get; set; } = NoMatchReason.Unknown;
    }
}
