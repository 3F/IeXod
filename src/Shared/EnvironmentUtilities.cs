// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

namespace net.r_eg.IeXod.Shared
{
    internal static partial class EnvironmentUtilities
    {
        public static bool Is64BitProcess => Marshal.SizeOf<IntPtr>() == 8;

        public static bool Is64BitOperatingSystem =>
#if FEATURE_64BIT_ENVIRONMENT_QUERY
            Environment.Is64BitOperatingSystem;
#else
            RuntimeInformation.OSArchitecture == Architecture.Arm64 ||
            RuntimeInformation.OSArchitecture == Architecture.X64;
#endif
    }
}
