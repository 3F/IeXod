// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Reflection;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Tasks;
using net.r_eg.IeXod.Utilities;
using Xunit;

namespace net.r_eg.IeXod.UnitTests
{
    sealed public class RemoveDir_Tests
    {
        /*
         * Method:   AttributeForwarding
         *
         * Make sure that attributes set on input items are forwarded to output items.
         */
        [Fact]
        public void AttributeForwarding()
        {
            RemoveDir t = new RemoveDir();

            ITaskItem i = new TaskItem("MyNonExistentDirectory");
            i.SetMetadata("Locale", "en-GB");
            t.Directories = new ITaskItem[] { i };
            t.BuildEngine = new MockEngine();

            t.Execute();

            Assert.Equal("en-GB", t.RemovedDirectories[0].GetMetadata("Locale"));

            // Output ItemSpec should not be overwritten.
            Assert.Equal("MyNonExistentDirectory", t.RemovedDirectories[0].ItemSpec);
        }
    }
}



