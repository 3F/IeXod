// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using net.r_eg.IeXod.BackEnd;
using net.r_eg.IeXod.Construction;
using net.r_eg.IeXod.Engine.UnitTests.TestComparers;
using net.r_eg.IeXod.Execution;
using net.r_eg.IeXod.UnitTests;
using net.r_eg.IeXod.UnitTests.BackEnd;
using Xunit;
using static net.r_eg.IeXod.Engine.UnitTests.TestData.ProjectInstanceTestObjects;

namespace net.r_eg.IeXod.Engine.UnitTests.Instance
{
    public class ProjectTaskInstance_Internal_Tests
    {
        public static IEnumerable<object[]> TestData
        {
            get
            {
                yield return new object[]
                {
                    null,
                    null
                };

                yield return new object[]
                {
                    new Dictionary<string, (string, MockElementLocation)>(),
                    new List<ProjectTaskInstanceChild>()
                };

                yield return new object[]
                {
                    new Dictionary<string, (string, MockElementLocation)>
                    {
                        {"p1", ("v1", new MockElementLocation("p1"))}
                    },
                    new List<ProjectTaskInstanceChild>
                    {
                        CreateTaskItemyOutput()
                    }
                };

                yield return new object[]
                {
                    new Dictionary<string, (string, MockElementLocation)>
                    {
                        {"p1", ("v1", new MockElementLocation("p1"))},
                        {"p2", ("v2", new MockElementLocation("p2"))}
                    },
                    new List<ProjectTaskInstanceChild>
                    {
                        CreateTaskItemyOutput(),
                        CreateTaskPropertyOutput()
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(TestData), DisableDiscoveryEnumeration = true)]
        public void ProjectTaskInstanceCanSerializeViaTranslator(
            IDictionary<string, (string, MockElementLocation)> parameters,
            List<ProjectTaskInstanceChild> outputs)
        {
            parameters = parameters ?? new Dictionary<string, (string, MockElementLocation)>();

            var parametersCopy = new Dictionary<string, (string, ElementLocation)>(parameters.Count);
            foreach (var param in parameters)
            {
                parametersCopy[param.Key] = (param.Value.Item1, param.Value.Item2);
            }

            var original = CreateTargetTask(null, parametersCopy, outputs);

            ((ITranslatable) original).Translate(TranslationHelpers.GetWriteTranslator());
            var copy = ProjectTaskInstance.FactoryForDeserialization(TranslationHelpers.GetReadTranslator());

            Assert.Equal(original, copy, new TargetTaskComparer());
        }
    }
}
