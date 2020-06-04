// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod.Collections
{
    /// <summary>
    /// The extensions class for ConcurrentStack&lt;T&gt;
    /// </summary>
    internal static class ConcurrentStackExtensions
    {
        /// <summary>
        /// The peek method.
        /// </summary>
        /// <typeparam name="T">The type contained within the stack.</typeparam>
        public static T Peek<T>(this ConcurrentStack<T> stack) where T : class
        {
            ErrorUtilities.VerifyThrow(stack.TryPeek(out T result), "Unable to peek from stack");
            return result;
        }

        /// <summary>
        /// The pop method.
        /// </summary>
        /// <typeparam name="T">The type contained within the stack.</typeparam>
        public static T Pop<T>(this ConcurrentStack<T> stack) where T : class
        {
            ErrorUtilities.VerifyThrow(stack.TryPop(out T result), "Unable to pop from stack");
            return result;
        }
    }
}
