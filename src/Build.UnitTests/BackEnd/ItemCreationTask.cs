// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Utilities;
using System.Collections;
using System.Collections.Generic;

namespace ItemCreationTask
{
    public class ItemCreationTask : Task
    {
        public ITaskItem[] InputItemsToPassThrough
        {
            get;
            set;
        }

        public ITaskItem[] InputItemsToCopy
        {
            get;
            set;
        }

        [Output]
        public ITaskItem[] PassedThroughOutputItems
        {
            get;
            set;
        }

        [Output]
        public ITaskItem[] CreatedOutputItems
        {
            get;
            set;
        }

        [Output]
        public ITaskItem[] CopiedOutputItems
        {
            get;
            set;
        }

        [Output]
        public string OutputString
        {
            get;
            set;
        }

        public override bool Execute()
        {
            PassedThroughOutputItems = InputItemsToPassThrough;

            CopiedOutputItems = new ITaskItem[InputItemsToCopy.Length];

            for (int i = 0; i < InputItemsToCopy.Length; i++)
            {
                CopiedOutputItems[i] = new TaskItem(InputItemsToCopy[i]);
            }

            CreatedOutputItems = new ITaskItem[2];
            CreatedOutputItems[0] = new TaskItem("Foo");
            CreatedOutputItems[1] = new TaskItem("Bar");

            OutputString = "abc; def; ghi";

            return true;
        }
    }
}
