// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace net.r_eg.IeXod.Framework
{
    /// <summary>
    /// This interface is used to forward events to another loggers
    /// </summary>
    public interface IEventRedirector
    {
        /// <summary>
        /// This method is called by the node loggers to forward the events to central logger
        /// </summary>
        void ForwardEvent(BuildEventArgs buildEvent);
    }
}
