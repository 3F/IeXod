﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Threading;

namespace net.r_eg.IeXod.Framework.Profiler
{
    /// <summary>
    /// Assigns unique evaluation ids. Thread safe.
    /// </summary>
    internal static class EvaluationIdProvider
    {
        private static long _sAssignedId = -1;
        private static readonly long ProcessId = Process.GetCurrentProcess().Id;

        /// <summary>
        /// Returns a unique evaluation id
        /// </summary>
        /// <remarks>
        /// The id is guaranteed to be unique across all running processes.
        /// Additionally, it is monotonically increasing for callers on the same process id
        /// </remarks>
        public static long GetNextId()
        {
            checked
            {
                var nextId = Interlocked.Increment(ref _sAssignedId);
                // Returns a unique number based on nextId (a unique number for this process) and the current process Id
                // Uses the Cantor pairing function (https://en.wikipedia.org/wiki/Pairing_function) to guarantee uniqueness
                return (((nextId + ProcessId) * (nextId + ProcessId + 1)) / 2) + ProcessId;
            }
        }
    }
}
