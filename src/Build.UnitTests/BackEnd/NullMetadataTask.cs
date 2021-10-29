// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Utilities;
using System.Collections;
using System.Collections.Generic;

namespace NullMetadataTask
{
    public class NullMetadataTask : Task
    {
        [Output]
        public ITaskItem[] OutputItems
        {
            get;
            set;
        }

        public override bool Execute()
        {
            OutputItems = new ITaskItem[1];

            IDictionary<string, string> metadata = new Dictionary<string, string>();
            metadata.Add("a", null);

            OutputItems[0] = new TaskItem("foo", (IDictionary)metadata);

            return true;
        }
    }
}
