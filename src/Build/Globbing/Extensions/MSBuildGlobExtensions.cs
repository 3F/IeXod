// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using net.r_eg.IeXod.Globbing.Visitor;

namespace net.r_eg.IeXod.Globbing.Extensions
{
    /// <summary>
    /// Extensions for <see cref="IMSBuildGlob"/>
    /// </summary>
    public static class MSBuildGlobExtensions
    {
        /// <summary>
        /// Retrieve all the <see cref="MSBuildGlob"/> objects from the given <paramref name="glob"/> composite.
        /// </summary>
        /// <param name="glob">A glob composite</param>
        /// <returns></returns>
        public static IEnumerable<MSBuildGlob> GetParsedGlobs(this IMSBuildGlob glob)
        {
            var parsedGlobVisitor = new ParsedGlobCollector();
            parsedGlobVisitor.Visit(glob);

            return parsedGlobVisitor.CollectedGlobs;
        }
    }
}