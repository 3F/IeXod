// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Shared;
using TaskItem = net.r_eg.IeXod.Execution.ProjectItemInstance.TaskItem;

namespace net.r_eg.IeXod.BackEnd
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
            : base(nodeBuildEvent, new TargetFinishedTranslator(TranslateTargetFinishedEvent))
        {
        }

        /// <summary>
        /// Constructor for deserialization
        /// </summary>
        private LogMessagePacket(ITranslator translator)
            : base(translator)
        {
        }

        /// <summary>
        /// Factory for serialization
        /// </summary>
        static internal INodePacket FactoryForDeserialization(ITranslator translator)
        {
            return new LogMessagePacket(translator);
        }

        /// <summary>
        /// Translate the TargetOutputs for the target finished event.
        /// </summary>
        private static void TranslateTargetFinishedEvent(ITranslator translator, TargetFinishedEventArgs finishedEvent)
        {
            List<TaskItem> targetOutputs = null;
            if (translator.Mode == TranslationDirection.WriteToStream)
            {
                if (finishedEvent.TargetOutputs != null)
                {
                    targetOutputs = new List<TaskItem>();
                    foreach (TaskItem item in finishedEvent.TargetOutputs)
                    {
                        targetOutputs.Add(item);
                    }
                }
            }

            translator.Translate<TaskItem>(ref targetOutputs, TaskItem.FactoryForDeserialization);

            if (translator.Mode == TranslationDirection.ReadFromStream)
            {
                finishedEvent.TargetOutputs = targetOutputs;
            }
        }
    }
}
