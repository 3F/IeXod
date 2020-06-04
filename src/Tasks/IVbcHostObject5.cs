﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

namespace net.r_eg.IeXod.Tasks.Hosting
{
    /// <summary>
    /// Defines an interface that proffers a free threaded host object that
    /// allows for background threads to call directly (avoids marshalling
    /// to the UI thread.
    /// </summary>
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(true)]
    [Guid("5ACF41FF-6F2B-4623-8146-740C89212B21")]
    public interface IVbcHostObject5 : IVbcHostObject4
    {
        IVbcHostObjectFreeThreaded GetFreeThreadedHostObject();
        [PreserveSig]
        int CompileAsync(out IntPtr buildSucceededEvent, out IntPtr buildFailedEvent);
        [PreserveSig]
        int EndCompile(bool buildSuccess);

        bool SetPlatformWith32BitPreference(string platformWith32BitPreference);
        bool SetHighEntropyVA(bool highEntropyVA);
        bool SetSubsystemVersion(string subsystemVersion);
    }
}
