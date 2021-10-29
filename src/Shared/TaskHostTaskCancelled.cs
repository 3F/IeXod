// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace net.r_eg.IeXod.BackEnd
{
    /// <summary>
    /// TaskHostTaskCancelled informs the task host that the task it is 
    /// currently executing has been canceled.
    /// </summary>
    internal class TaskHostTaskCancelled : INodePacket
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TaskHostTaskCancelled()
        {
        }

        /// <summary>
        /// The type of this NodePacket
        /// </summary>
        public NodePacketType Type
        {
            get { return NodePacketType.TaskHostTaskCancelled; }
        }

        /// <summary>
        /// Translates the packet to/from binary form.
        /// </summary>
        /// <param name="translator">The translator to use.</param>
        public void Translate(ITranslator translator)
        {
            // Do nothing -- this packet doesn't contain any parameters. 
        }

        /// <summary>
        /// Factory for deserialization.
        /// </summary>
        internal static INodePacket FactoryForDeserialization(ITranslator translator)
        {
            TaskHostTaskCancelled taskCancelled = new TaskHostTaskCancelled();
            taskCancelled.Translate(translator);
            return taskCancelled;
        }
    }
}
