﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace net.r_eg.IeXod.Tasks.Hosting
{
    /// <summary>
    /// Defines an interface for the Csc task to communicate with the IDE.  In particular,
    /// the Csc task will delegate the actual compilation to the IDE, rather than shelling
    /// out to the command-line compilers.
    /// </summary>
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(true)]
    [Guid("0DDB496F-C93C-492C-87F1-90B6FDBAA833")]
    public interface ICscHostObject4 : ICscHostObject3
    {
        bool SetPlatformWith32BitPreference(string platformWith32BitPreference);
        bool SetHighEntropyVA(bool highEntropyVA);
        bool SetSubsystemVersion(string subsystemVersion);
    }
}
