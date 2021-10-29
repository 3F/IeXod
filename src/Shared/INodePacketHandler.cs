// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Text;

namespace net.r_eg.IeXod.BackEnd
{
    /// <summary>
    /// Objects which wish to receive packets from the NodePacketRouter must implement this interface.
    /// </summary>
    internal interface INodePacketHandler
    {
        /// <summary>
        /// This method is invoked by the NodePacketRouter when a packet is received and is intended for
        /// this recipient.
        /// </summary>
        /// <param name="node">The node from which the packet was received.</param>
        /// <param name="packet">The packet.</param>
        void PacketReceived(int node, INodePacket packet);
    }
}
