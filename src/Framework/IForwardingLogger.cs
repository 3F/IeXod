// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace net.r_eg.IeXod.Framework
{
    /// <summary>
    /// This interface extends the ILogger interface to provide a property which can be used to forward events
    /// to a logger running in a different process. It can also be used create filtering loggers.
    /// </summary>
    public interface IForwardingLogger : INodeLogger
    {
        /// <summary>
        /// This property is set by the build engine to allow a node loggers to forward messages to the
        /// central logger
        /// </summary>
        IEventRedirector BuildEventRedirector
        {
            get;

            set;
        }

        /// <summary>
        /// This property is set by the build engine or node to inform the forwarding logger which node it is running on
        /// </summary>
        int NodeId
        {
            get;

            set;
        }
    }
}
