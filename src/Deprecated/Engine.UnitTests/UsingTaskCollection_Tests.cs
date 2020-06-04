// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using NUnit.Framework;
using net.r_eg.IeXod.BuildEngine;

namespace net.r_eg.IeXod.UnitTests
{
    [TestFixture]
    public class UsingTaskCollection_Tests
    {
        [Test]
        public void TestICollectionMethods()
        {
            Project project = ObjectModelHelpers.CreateInMemoryProject(string.Format(@"

                    <Project xmlns=`msbuildnamespace`>

                        <UsingTask TaskName=`net.r_eg.IeXod.UnitTests.Project_Tests.WashCar` AssemblyFile=`{0}` Condition=` true == true `/>
                        <UsingTask TaskName=`net.r_eg.IeXod.UnitTests.Project_Tests.Message` AssemblyName=`{1}` Condition=` false == true `/>

                        <Target Name=`Build`>
                            <WashCar/>
                        </Target>

                    </Project>

                ", new Uri(Assembly.GetExecutingAssembly().EscapedCodeBase).LocalPath, Assembly.GetExecutingAssembly().FullName));

            UsingTaskCollection usingTasks = project.UsingTasks;

            Assertion.AssertEquals(2, usingTasks.Count);

            UsingTask[] array = new UsingTask[2];
            usingTasks.CopyTo(array, 0);

            Assertion.AssertEquals(usingTasks[0], array[0]);
            Assertion.AssertEquals(usingTasks[1], array[1]);

            object[] arrayObjects = new object[2];
            usingTasks.CopyTo(arrayObjects, 0);

            Assertion.AssertEquals(usingTasks[0], arrayObjects[0]);
            Assertion.AssertEquals(usingTasks[1], arrayObjects[1]);

            Assertion.AssertEquals("net.r_eg.IeXod.UnitTests.Project_Tests.WashCar", usingTasks[0].TaskName);
            Assertion.AssertEquals("net.r_eg.IeXod.UnitTests.Project_Tests.Message", usingTasks[1].TaskName);

            Assert.IsFalse(usingTasks.IsSynchronized, "Expected IsSynchronized to be false");
            Assert.IsNotNull(usingTasks.SyncRoot, "Expected SynchRoot to not be null");
        }
    }
}
