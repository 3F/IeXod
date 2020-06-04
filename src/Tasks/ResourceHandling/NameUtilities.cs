// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace net.r_eg.IeXod.Tasks.ResourceHandling
{
    static class NameUtilities
    {
        /// <summary>
        /// Extract the full name of a type from an assembly-qualified name string.
        /// </summary>
        /// <param name="assemblyQualifiedName"></param>
        /// <returns></returns>
        internal static string FullNameFromAssemblyQualifiedName(string assemblyQualifiedName)
        {
            var commaIndex = assemblyQualifiedName.IndexOf(',');

            if (commaIndex == -1)
            {
                throw new ArgumentException(nameof(assemblyQualifiedName));
            }

            return assemblyQualifiedName.Substring(0, commaIndex);
        }
    }
}
