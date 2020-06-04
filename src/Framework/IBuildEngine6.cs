// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace net.r_eg.IeXod.Framework
{
    /// <summary>
    /// This interface extends <see cref="IBuildEngine5" /> to allow tasks to get the current project's global properties.
    /// </summary>
    public interface IBuildEngine6 : IBuildEngine5
    {
        /// <summary>
        /// Gets the global properties for the current project.
        /// </summary>
        /// <returns>An <see cref="IReadOnlyDictionary{String, String}" /> containing the global properties of the current project.</returns>
        IReadOnlyDictionary<string, string> GetGlobalProperties();
    }
}
