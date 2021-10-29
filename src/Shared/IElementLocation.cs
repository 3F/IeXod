// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.BackEnd;

namespace net.r_eg.IeXod.Shared
{
    /// <summary>
    /// Represents the location information for error reporting purposes.  This is normally used to
    /// associate a run-time error with the original XML.
    /// This is not used for arbitrary errors from tasks, which store location in a BuildXXXXEventArgs.
    /// All implementations should be IMMUTABLE.
    /// This is not public because the current implementation only provides correct data for unedited projects.
    /// DO NOT make it public without considering a solution to this problem.
    /// </summary>
    internal interface IElementLocation : ITranslatable
    {
        /// <summary>
        /// The file from which this particular element originated.  It may
        /// differ from the ProjectFile if, for instance, it was part of
        /// an import or originated in a targets file.
        /// Should always have a value.
        /// If not known, returns empty string.
        /// </summary>
        string File
        {
            get;
        }

        /// <summary>
        /// The line number where this element exists in its file.
        /// The first line is numbered 1.
        /// Zero indicates "unknown location".
        /// </summary>
        int Line
        {
            get;
        }

        /// <summary>
        /// The column number where this element exists in its file.
        /// The first column is numbered 1.
        /// Zero indicates "unknown location".
        /// </summary>
        int Column
        {
            get;
        }

        /// <summary>
        /// The location in a form suitable for replacement
        /// into a message.
        /// </summary>
        string LocationString
        {
            get;
        }
    }
}
