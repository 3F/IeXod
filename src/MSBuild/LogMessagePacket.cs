// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

using net.r_eg.IeXod.BackEnd;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Shared;
#if FEATURE_APPDOMAIN
using TaskEngineAssemblyResolver = net.r_eg.IeXod.BackEnd.Logging.TaskEngineAssemblyResolver;
#endif

namespace net.r_eg.IeXod.CommandLine
{
    /// <summary>
    /// A packet to encapsulate a BuildEventArg logging message.
    /// Contents:
    /// Build Event Type
    /// Build Event Args
    /// </summary>
    internal class LogMessagePacket : LogMessagePacketBase
    {
        /// <summary>
        /// Encapsulates the buildEventArg in this packet.
        /// </summary>
        internal LogMessagePacket(KeyValuePair<int, BuildEventArgs>? nodeBuildEvent)
            : base(nodeBuildEvent, null)
        {
        }

        /// <summary>
        /// Constructor for deserialization
        /// </summary>
        private LogMessagePacket(ITranslator translator)
            : base(translator)
        {
            Translate(translator);
        }

        /// <summary>
        /// Factory for serialization
        /// </summary>
        static internal INodePacket FactoryForDeserialization(ITranslator translator)
        {
            return new LogMessagePacket(translator);
        }
    }
}