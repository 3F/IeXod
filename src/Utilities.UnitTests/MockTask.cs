// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using System.Resources;
using net.r_eg.IeXod.Utilities;

namespace net.r_eg.IeXod.UnitTests
{
    internal sealed class MockTask : Task
    {
        internal MockTask()
            : this(true)
        {
        }

        internal MockTask(bool registerResources)
        {
            if (registerResources)
            {
                RegisterResources();
            }
        }

        private void RegisterResources() => Log.TaskResources = new ResourceManager("net.r_eg.IeXod.Utilities.UnitTests.strings", typeof(MockTask).GetTypeInfo().Assembly);

        public override bool Execute() => true;
    }
}
