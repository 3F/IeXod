// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Security;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace net.r_eg.IeXod.BuildEngine.Shared
{
    /// <summary>
    /// This class contains utility methods for file IO.
    /// Separate from FileUtilities because some assemblies may only need the patterns.
    /// PERF\COVERAGE NOTE: Try to keep classes in 'shared' as granular as possible. All the methods in 
    /// each class get pulled into the resulting assembly.
    /// </summary>
    /// <owner>SumedhK, JomoF</owner>
    internal static class FileUtilitiesRegex
    {
        // regular expression used to match file-specs beginning with "<drive letter>:" 
        internal static readonly Regex DrivePattern = new Regex(@"^[A-Za-z]:");

        // regular expression used to match UNC paths beginning with "\\<server>\<share>"
        internal static readonly Regex UNCPattern = new Regex(String.Format(CultureInfo.InvariantCulture,
            @"^[\{0}\{1}][\{0}\{1}][^\{0}\{1}]+[\{0}\{1}][^\{0}\{1}]+", Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
    }
}
