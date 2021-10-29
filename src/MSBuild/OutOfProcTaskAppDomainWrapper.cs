// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using net.r_eg.IeXod.Shared;
using net.r_eg.IeXod.Internal;
using net.r_eg.IeXod.Framework;

namespace net.r_eg.IeXod.CommandLine
{
    /// <summary>
    /// Class for executing a task in an AppDomain
    /// </summary>
    [Serializable]
    internal class OutOfProcTaskAppDomainWrapper : OutOfProcTaskAppDomainWrapperBase
    {
        /// <summary>
        /// This is an extension of the OutOfProcTaskAppDomainWrapper that is responsible 
        /// for activating and executing the user task.
        /// This extension provides support for ICancellable Out-Of-Proc tasks.
        /// </summary>
        /// <returns>True if the task is ICancellable</returns>
        internal bool CancelTask()
        {
            // If the cancel was issued even before WrappedTask has been created then set a flag so that we can
            // skip execution
            CancelPending = true;

            // Store in a local to avoid a race
            var wrappedTask = WrappedTask;
            if (wrappedTask == null)
            {
                return true;
            }

            ICancelableTask cancelableTask = wrappedTask as ICancelableTask;
            if (cancelableTask != null)
            {
                cancelableTask.Cancel();
                return true;
            }

            return false;
        }
    }
}
