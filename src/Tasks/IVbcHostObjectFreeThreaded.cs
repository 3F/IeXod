// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace net.r_eg.IeXod.Tasks.Hosting
{
    /// <summary>
    /// Defines a free threaded interface for the Vbc task to communicate with the IDE.  In particular,
    /// the Vbc task will delegate the actual compilation to the IDE, rather than shelling
    /// out to the command-line compilers. 
    /// This particular version of Compile (unlike the IVbcHostObject::Compile) is not marshalled back to the UI
    /// thread. The implementor of the interface is responsible for any marshalling.
    /// This was added to allow some of the implementors code to run on the BG thread from which VBC Task is being 
    /// called from.
    /// </summary>
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(true)]
    [Guid("ECCF972F-8C2D-4F51-9746-9288661DE2CB")]
    public interface IVbcHostObjectFreeThreaded
    {
        bool Compile();
    }
}
