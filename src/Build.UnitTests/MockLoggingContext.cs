// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.BackEnd.Logging;
using net.r_eg.IeXod.Framework;

namespace net.r_eg.IeXod.Engine.UnitTests
{
    /// <summary>
    /// Logging context and helpers for evaluation logging
    /// </summary>
    internal class MockLoggingContext : LoggingContext
    {
        public MockLoggingContext(ILoggingService loggingService, BuildEventContext eventContext) : base(loggingService, eventContext)
        {
            IsValid = true;
        }
    }
}
