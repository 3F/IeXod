// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace net.r_eg.IeXod.Tasks.Hosting
{
    /// <summary>
    /// Defines an interface for the Vbc task to communicate with the IDE.  In particular,
    /// the Vbc task will delegate the actual compilation to the IDE, rather than shelling
    /// out to the command-line compilers.
    /// </summary>
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(true)]
    [Guid("f59afc84-d102-48b1-a090-1b90c79d3e09")]
    public interface IVbcHostObject2 : IVbcHostObject
    {
        bool SetOptionInfer(bool optionInfer);
        bool SetModuleAssemblyName(string moduleAssemblyName);
        bool SetWin32Manifest(string win32Manifest);
    }
}
