﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace net.r_eg.IeXod.BackEnd
{
    /// <summary>
    /// The NodeBuildComplete packet is used to indicate to a node that it should clean up its current build and 
    /// possibly prepare for node reuse.
    /// </summary>
    internal class NodeBuildComplete : INodePacket
    {
        /// <summary>
        /// Flag indicating if the node should prepare for reuse after cleanup.
        /// </summary>
        private bool _prepareForReuse;

        /// <summary>
        /// Constructor.
        /// </summary>
        public NodeBuildComplete(bool prepareForReuse)
        {
            _prepareForReuse = prepareForReuse;
        }

        /// <summary>
        /// Private constructor for translation
        /// </summary>
        private NodeBuildComplete()
        {
        }

        /// <summary>
        /// Flag indicating if the node should prepare for reuse.
        /// </summary>
        public bool PrepareForReuse
        {
            [DebuggerStepThrough]
            get
            { return _prepareForReuse; }
        }

        #region INodePacket Members

        /// <summary>
        /// The packet type
        /// </summary>
        public NodePacketType Type
        {
            [DebuggerStepThrough]
            get
            { return NodePacketType.NodeBuildComplete; }
        }

        #endregion

        #region INodePacketTranslatable Members

        /// <summary>
        /// Translates the packet to/from binary form.
        /// </summary>
        /// <param name="translator">The translator to use.</param>
        public void Translate(ITranslator translator)
        {
            translator.Translate(ref _prepareForReuse);
        }

        /// <summary>
        /// Factory for deserialization.
        /// </summary>
        internal static NodeBuildComplete FactoryForDeserialization(ITranslator translator)
        {
            NodeBuildComplete packet = new NodeBuildComplete();
            packet.Translate(translator);
            return packet;
        }

        #endregion
    }
}
