// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.DotNet.Cli.Utils
{
    public static class PerfTrace
    {
        private static ConcurrentBag<PerfTraceThreadContext> _threads = new ConcurrentBag<PerfTraceThreadContext>();

        [ThreadStatic]
        private static PerfTraceThreadContext _current;

        public static bool Enabled { get; set; }

        public static PerfTraceThreadContext Current => _current ?? (_current = InitializeCurrent());

        private static PerfTraceThreadContext InitializeCurrent()
        {
            var context = new PerfTraceThreadContext(Thread.CurrentThread.ManagedThreadId);
            _threads.Add(context);
            return context;
        }

        public static IEnumerable<PerfTraceThreadContext> GetEvents()
        {
            return _threads;
        }
    }
}
