// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace net.r_eg.IeXod.Framework
{
    /// <summary>
    /// This interface defines a "parallel aware logger" in the build system. A parallel aware logger 
    /// will accept a cpu count and be aware that any cpu count greater than 1 means the events will
    /// be received from the logger from each cpu as the events are logged. 
    /// </summary>
    [ComVisible(true)]
    public interface INodeLogger : ILogger
    {
        /// <summary>
        /// Initializes the current <see cref="INodeLogger"/> instance.
        /// </summary>
        /// <param name="eventSource"></param>
        /// <param name="nodeCount"></param>
        void Initialize(IEventSource eventSource, int nodeCount);
    }
}
