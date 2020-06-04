// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using net.r_eg.IeXod.BackEnd;
using Xunit;

namespace net.r_eg.IeXod.UnitTests.BackEnd
{
    /// <summary>
    /// Unit Tests for TaskHostTaskCancelled packet.
    /// </summary>
    public class TaskHostTaskCancelled_Tests
    {
        /// <summary>
        /// Basic test of the constructor. 
        /// </summary>
        [Fact]
        public void TestConstructor()
        {
            TaskHostTaskCancelled cancelled = new TaskHostTaskCancelled();
        }

        /// <summary>
        /// Basic test of serialization / deserialization. 
        /// </summary>
        [Fact]
        public void TestTranslation()
        {
            TaskHostTaskCancelled cancelled = new TaskHostTaskCancelled();

            ((ITranslatable)cancelled).Translate(TranslationHelpers.GetWriteTranslator());
            INodePacket packet = TaskHostTaskCancelled.FactoryForDeserialization(TranslationHelpers.GetReadTranslator());

            TaskHostTaskCancelled deserializedCancelled = packet as TaskHostTaskCancelled;
        }
    }
}
