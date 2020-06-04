// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

namespace net.r_eg.IeXod.Tasks.Deployment.Bootstrapper
{
    internal static class NativeMethods
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr BeginUpdateResourceW(String fileName, bool deleteExistingResource);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool UpdateResourceW(IntPtr hUpdate, IntPtr lpType, String lpName, short wLanguage, byte[] data, int cbData);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool EndUpdateResource(IntPtr hUpdate, bool fDiscard);
    }
}
