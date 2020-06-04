// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Collections.Generic;
using net.r_eg.IeXod.Execution;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Tasks;
using net.r_eg.IeXod.Utilities;
using Xunit;
using Xunit.Abstractions;
using Shouldly;

namespace net.r_eg.IeXod.UnitTests
{
    sealed public class CreateItem_Tests
    {
        private readonly ITestOutputHelper _testOutput;

        public CreateItem_Tests(ITestOutputHelper output)
        {
            _testOutput = output;
        }

        /// <summary>
        /// CreateIteming identical lists results in empty list.
        /// </summary>
        [Fact]
        public void OneFromOneIsZero()
        {
            CreateItem t = new CreateItem();
            t.BuildEngine = new MockEngine();

            t.Include = new ITaskItem[] { new TaskItem("MyFile.txt") };
            t.Exclude = new ITaskItem[] { new TaskItem("MyFile.txt") };

            bool success = t.Execute();

            Assert.True(success);
            Assert.Empty(t.Include);
        }

        /// <summary>
        /// CreateIteming completely different lists results in left list.
        /// </summary>
        [Fact]
        public void OneFromOneMismatchIsOne()
        {
            CreateItem t = new CreateItem();
            t.BuildEngine = new MockEngine();

            t.Include = new ITaskItem[] { new TaskItem("MyFile.txt") };
            t.Exclude = new ITaskItem[] { new TaskItem("MyFileOther.txt") };

            bool success = t.Execute();

            Assert.True(success);
            Assert.Single(t.Include);
            Assert.Equal("MyFile.txt", t.Include[0].ItemSpec);
        }

        /// <summary>
        /// If 'Exclude' is unspecified, then 'Include' is the result.
        /// </summary>
        [Fact]
        public void UnspecifiedFromOneIsOne()
        {
            CreateItem t = new CreateItem();
            t.BuildEngine = new MockEngine();

            t.Include = new ITaskItem[] { new TaskItem("MyFile.txt") };

            bool success = t.Execute();

            Assert.True(success);
            Assert.Single(t.Include);
            Assert.Equal(t.Include[0].ItemSpec, t.Include[0].ItemSpec);
        }


        /// <summary>
        /// If 'Include' is unspecified, then empty is the result.
        /// </summary>
        [Fact]
        public void OneFromUnspecifiedIsEmpty()
        {
            CreateItem t = new CreateItem();
            t.BuildEngine = new MockEngine();

            t.Exclude = new ITaskItem[] { new TaskItem("MyFile.txt") };

            bool success = t.Execute();

            Assert.True(success);
            Assert.Empty(t.Include);
        }

        /// <summary>
        /// If 'Include' and 'Exclude' are unspecified, then empty is the result.
        /// </summary>
        [Fact]
        public void UnspecifiedFromUnspecifiedIsEmpty()
        {
            CreateItem t = new CreateItem();
            t.BuildEngine = new MockEngine();

            bool success = t.Execute();

            Assert.True(success);
            Assert.Empty(t.Include);
        }


        /// <summary>
        /// CreateItem is case insensitive.
        /// </summary>
        [Fact]
        public void CaseDoesntMatter()
        {
            CreateItem t = new CreateItem();
            t.BuildEngine = new MockEngine();

            t.Include = new ITaskItem[] { new TaskItem("MyFile.txt") };
            t.Exclude = new ITaskItem[] { new TaskItem("myfile.tXt") };

            bool success = t.Execute();

            Assert.True(success);
            Assert.Empty(t.Include);
        }

        /// <summary>
        /// Using the CreateItem task to expand wildcards, and then try accessing the RecursiveDir 
        /// metadata to force batching.
        /// </summary>
        [Fact]
        public void WildcardsWithRecursiveDir()
        {
            ObjectModelHelpers.DeleteTempProjectDirectory();

            ObjectModelHelpers.CreateFileInTempProjectDirectory("Myapp.proj", @"
                <Project ToolsVersion=`msbuilddefaulttoolsversion` xmlns=`msbuildnamespace`>
                  <Target Name =`Repro`>
                    <CreateItem Include=`**\*.txt`>
                      <Output TaskParameter=`Include` ItemName=`Text`/>
                    </CreateItem>
                    <Copy SourceFiles=`@(Text)` DestinationFiles=`Destination\%(RecursiveDir)%(Filename)%(Extension)`/>
                  </Target>
                </Project>
                ");

            ObjectModelHelpers.CreateFileInTempProjectDirectory("Foo.txt", "foo");
            ObjectModelHelpers.CreateFileInTempProjectDirectory(Path.Combine("Subdir", "Bar.txt"), "bar");

            MockLogger logger = new MockLogger(_testOutput);
            ObjectModelHelpers.BuildTempProjectFileExpectSuccess("Myapp.proj", logger);

            ObjectModelHelpers.AssertFileExistsInTempProjectDirectory(Path.Combine("Destination", "Foo.txt"));
            ObjectModelHelpers.AssertFileExistsInTempProjectDirectory(Path.Combine("Destination", "Subdir", "Bar.txt"));
        }

        /// <summary>
        /// Using the CreateItem task to expand wildcards and verifying that the RecursiveDir metadatum is successfully
        /// serialized/deserialized cross process.
        /// </summary>
        [Fact]
        public void RecursiveDirOutOfProc()
        {
            using var env = TestEnvironment.Create(_testOutput);

            ObjectModelHelpers.DeleteTempProjectDirectory();

            string projectFileFullPath = ObjectModelHelpers.CreateFileInTempProjectDirectory("Myapp.proj", @"
                <Project ToolsVersion=`msbuilddefaulttoolsversion` xmlns=`msbuildnamespace`>
                  <Target Name =`Repro` Returns=`@(Text)`>
                    <CreateItem Include=`**\*.txt`>
                      <Output TaskParameter=`Include` ItemName=`Text`/>
                    </CreateItem>
                  </Target>
                </Project>
                ");

            ObjectModelHelpers.CreateFileInTempProjectDirectory(Path.Combine("Subdir", "Bar.txt"), "bar");

            env.SetEnvironmentVariable("MSBUILDTARGETRESULTCOMPRESSIONTHRESHOLD", "0");

            BuildRequestData data = new BuildRequestData(projectFileFullPath, new Dictionary<string, string>(), null, new string[] { "Repro" }, null);
            BuildParameters parameters = new BuildParameters
            {
                DisableInProcNode = true,
                EnableNodeReuse = false,
                Loggers = new ILogger[] { new MockLogger(_testOutput) },
            };
            BuildResult result = BuildManager.DefaultBuildManager.Build(parameters, data);
            result.OverallResult.ShouldBe(BuildResultCode.Success);
            result.ResultsByTarget["Repro"].Items[0].GetMetadata("RecursiveDir").ShouldBe("Subdir" + Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// CreateItem should add additional metadata when instructed
        /// </summary>
        [Fact]
        public void AdditionalMetaData()
        {
            CreateItem t = new CreateItem();
            t.BuildEngine = new MockEngine();

            t.Include = new ITaskItem[] { new TaskItem("MyFile.txt") };
            t.AdditionalMetadata = new string[] { "MyMetaData=SomeValue" };

            bool success = t.Execute();

            Assert.True(success);
            Assert.Equal("SomeValue", t.Include[0].GetMetadata("MyMetaData"));
        }

        /// <summary>
        /// We should be able to preserve the existing metadata on items
        /// </summary>
        [Fact]
        public void AdditionalMetaDataPreserveExisting()
        {
            CreateItem t = new CreateItem();
            t.BuildEngine = new MockEngine();

            TaskItem item = new TaskItem("MyFile.txt");
            item.SetMetadata("MyMetaData", "SomePreserveMeValue");

            t.Include = new ITaskItem[] { item };
            t.PreserveExistingMetadata = true;

            t.AdditionalMetadata = new string[] { "MyMetaData=SomeValue" };

            bool success = t.Execute();

            Assert.True(success);
            Assert.Equal("SomePreserveMeValue", t.Include[0].GetMetadata("MyMetaData"));
        }

        /// <summary>
        /// The default is to overwrite existing metadata on items
        /// </summary>
        [Fact]
        public void AdditionalMetaDataOverwriteExisting()
        {
            CreateItem t = new CreateItem();
            t.BuildEngine = new MockEngine();

            TaskItem item = new TaskItem("MyFile.txt");
            item.SetMetadata("MyMetaData", "SomePreserveMeValue");

            t.Include = new ITaskItem[] { item };

            // The default for CreateItem is to overwrite any existing metadata
            // t.PreserveExistingMetadata = false;

            t.AdditionalMetadata = new string[] { "MyMetaData=SomeOverwriteValue" };

            bool success = t.Execute();

            Assert.True(success);
            Assert.Equal("SomeOverwriteValue", t.Include[0].GetMetadata("MyMetaData"));
        }
    }
}



