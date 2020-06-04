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
    [Guid("1186fe8f-8aba-48d6-8ce3-32ca42f53728")]
    public interface IVbcHostObject3 : IVbcHostObject2
    {
        bool SetLanguageVersion(string languageVersion);
    }
}
