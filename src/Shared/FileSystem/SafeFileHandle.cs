// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Win32.SafeHandles;

namespace net.r_eg.IeXod.Shared.FileSystem
{
    /// <summary>
    /// Handle for a volume iteration as returned by WindowsNative.FindFirstVolumeW />
    /// </summary>
    internal sealed class SafeFindFileHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        /// <summary>
        /// Private constructor for the PInvoke marshaller.
        /// </summary>
        private SafeFindFileHandle()
            : base(ownsHandle: true)
        {
        }

        /// <nodoc/>
        protected override bool ReleaseHandle()
        {
            return WindowsNative.FindClose(handle);
        }
    }
}
