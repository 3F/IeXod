// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace net.r_eg.IeXod.Framework
{
    /// <summary>
    /// This interface extends <see cref="IBuildEngine6" /> to allow tasks to set whether they want to
    /// log an error when a task returns without logging an error.
    /// </summary>
    public interface IBuildEngine7 : IBuildEngine6
    {
        public bool AllowFailureWithoutError { get; set; }
    }
}
