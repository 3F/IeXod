// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod.CommandLine
{
    /// <summary>
    /// Class for executing a task in an AppDomain
    /// </summary>
    [Serializable]
    internal class OutOfProcTaskAppDomainWrapper : OutOfProcTaskAppDomainWrapperBase
    {
        /// <summary>
        /// This is a stub for CLR2 in place of the OutOfProcTaskAppDomainWrapper class
        /// as used in CLR4 to support cancellation of ICancelable tasks.
        /// We provide a stub for CancelTask here so that the OutOfProcTaskHostNode
        /// that's shared by both the MSBuild.exe and MSBuildTaskHost.exe,
        /// can safely allow MSBuild.exe CLR4 Out-Of-Proc Task Host to call ICancelableTask.Cancel()
        /// </summary>
        /// <returns>False - Used by the OutOfProcTaskHostNode to determine if the task is ICancelable</returns>
        internal bool CancelTask()
        {
            // This method is a stub we will not do anything here.
            return false;
        }
    }
}
