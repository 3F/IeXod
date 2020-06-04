// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using net.r_eg.IeXod.BackEnd;
using net.r_eg.IeXod.Unittest;
using net.r_eg.IeXod.Evaluation;
using net.r_eg.IeXod.Exceptions;
using net.r_eg.IeXod.Execution;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Shared;
using TaskItem = net.r_eg.IeXod.Execution.ProjectItemInstance.TaskItem;
using Xunit;

namespace net.r_eg.IeXod.UnitTests.BackEnd
{
    /// <summary>
    /// Tests for the target result test.
    /// </summary>
    public class TargetResult_Tests
    {
        /// <summary>
        /// Tests a constructor with no items.
        /// </summary>
        [Fact]
        public void TestConstructorNoItems()
        {
            TargetResult result = new TargetResult(new TaskItem[] { }, BuildResultUtilities.GetStopWithErrorResult());
            Assert.Empty(result.Items);
            Assert.Null(result.Exception);
            Assert.Equal(TargetResultCode.Failure, result.ResultCode);
        }

        /// <summary>
        /// Tests a constructor with items.
        /// </summary>
        [Fact]
        public void TestConstructorWithItems()
        {
            TaskItem item = new TaskItem("foo", "bar.proj");
            TargetResult result = new TargetResult(new TaskItem[] { item }, BuildResultUtilities.GetStopWithErrorResult());
            Assert.Single(result.Items);
            Assert.Equal(item.ItemSpec, result.Items[0].ItemSpec);
            Assert.Equal(TargetResultCode.Failure, result.ResultCode);
        }

        /// <summary>
        /// Tests a constructor with a null item array passed.
        /// </summary>
        [Fact]
        public void TestConstructorNullItems()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                TargetResult result = new TargetResult(null, BuildResultUtilities.GetStopWithErrorResult());
            }
           );
        }
        /// <summary>
        /// Tests a constructor with an exception passed.
        /// </summary>
        [Fact]
        public void TestConstructorWithException()
        {
            TaskItem item = new TaskItem("foo", "bar.proj");
            TargetResult result = new TargetResult(new TaskItem[] { item }, BuildResultUtilities.GetStopWithErrorResult(new ArgumentException()));
            Assert.Single(result.Items);
            Assert.NotNull(result.Exception);
            Assert.Equal(typeof(ArgumentException), result.Exception.GetType());
            Assert.Equal(TargetResultCode.Failure, result.ResultCode);
        }

        /// <summary>
        /// Tests a constructor with a null exception passed.
        /// </summary>
        [Fact]
        public void TestConstructorWithExceptionNull()
        {
            TaskItem item = new TaskItem("foo", "bar.proj");
            TargetResult result = new TargetResult(new TaskItem[] { item }, BuildResultUtilities.GetStopWithErrorResult());
            Assert.Single(result.Items);
            Assert.Null(result.Exception);
            Assert.Equal(TargetResultCode.Failure, result.ResultCode);
        }

        /// <summary>
        /// Tests serialization with no exception in the result.
        /// </summary>
        [Fact]
        public void TestTranslationNoException()
        {
            TaskItem item = new TaskItem("foo", "bar.proj");
            item.SetMetadata("a", "b");

            TargetResult result = new TargetResult(new TaskItem[] { item }, BuildResultUtilities.GetStopWithErrorResult());

            ((ITranslatable)result).Translate(TranslationHelpers.GetWriteTranslator());
            TargetResult deserializedResult = TargetResult.FactoryForDeserialization(TranslationHelpers.GetReadTranslator());

            Assert.Equal(result.ResultCode, deserializedResult.ResultCode);
            Assert.True(TranslationHelpers.CompareCollections(result.Items, deserializedResult.Items, TaskItemComparer.Instance));
            Assert.True(TranslationHelpers.CompareExceptions(result.Exception, deserializedResult.Exception));
        }

        /// <summary>
        /// Tests serialization with an exception in the result.
        /// </summary>
        [Fact]
        public void TestTranslationWithException()
        {
            TaskItem item = new TaskItem("foo", "bar.proj");
            item.SetMetadata("a", "b");

            TargetResult result = new TargetResult(new TaskItem[] { item }, BuildResultUtilities.GetStopWithErrorResult(new BuildAbortedException()));

            ((ITranslatable)result).Translate(TranslationHelpers.GetWriteTranslator());
            TargetResult deserializedResult = TargetResult.FactoryForDeserialization(TranslationHelpers.GetReadTranslator());

            Assert.Equal(result.ResultCode, deserializedResult.ResultCode);
            Assert.True(TranslationHelpers.CompareCollections(result.Items, deserializedResult.Items, TaskItemComparer.Instance));
            Assert.True(TranslationHelpers.CompareExceptions(result.Exception, deserializedResult.Exception));
        }

        /// <summary>
        /// Test GetCacheDirectory is resilient to paths with strings that would normally make string.format to throw a FormatException
        /// </summary>
        [Fact]
        public void TestGetCacheDirectory()
        {
            string oldTmp = Environment.GetEnvironmentVariable("TMP");

            try
            {
                Environment.SetEnvironmentVariable("TMP", "C:\\}");
                string path1 = TargetResult.GetCacheDirectory(2, "Blah");

                Environment.SetEnvironmentVariable("TMP", "C:\\{");
                string path2 = TargetResult.GetCacheDirectory(2, "Blah");
            }
            finally
            {
                Environment.SetEnvironmentVariable("TMP", oldTmp);
            }
        }
    }
}
