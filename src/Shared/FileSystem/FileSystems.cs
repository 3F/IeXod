// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace net.r_eg.IeXod.Shared.FileSystem
{
    /// <summary>
    /// Factory for <see cref="IFileSystem"/>
    /// </summary>
    internal static class FileSystems
    {
        public static IFileSystem Default = GetFileSystem();

        private static IFileSystem GetFileSystem()
        {
#if CLR2COMPATIBILITY
            return MSBuildTaskHostFileSystem.Singleton();
#else
            if (NativeMethodsShared.IsWindows)
            {
                return MSBuildOnWindowsFileSystem.Singleton();
            }
            else
            {
                return ManagedFileSystem.Singleton();
            }
#endif
        }
    }
}
