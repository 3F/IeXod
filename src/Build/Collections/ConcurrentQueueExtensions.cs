// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod.Collections
{
    /// <summary>
    /// The extensions class for ConcurrentQueue&lt;T&gt;
    /// </summary>
    internal static class ConcurrentQueueExtensions
    {
        /// <summary>
        /// The dequeue method.
        /// </summary>
        /// <typeparam name="T">The type contained within the queue</typeparam>
        public static T Dequeue<T>(this ConcurrentQueue<T> stack) where T : class
        {
            ErrorUtilities.VerifyThrow(stack.TryDequeue(out T result), "Unable to dequeue from queue");
            return result;
        }
    }
}
