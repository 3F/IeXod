// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using net.r_eg.IeXod.BackEnd;
using net.r_eg.IeXod.Execution;
using net.r_eg.IeXod.UnitTests.BackEnd;
using Xunit;
using static net.r_eg.IeXod.Engine.UnitTests.TestData.ProjectInstanceTestObjects;
using static net.r_eg.IeXod.Engine.UnitTests.TestComparers.ProjectInstanceModelTestComparers;

namespace net.r_eg.IeXod.Engine.UnitTests.Instance
{
    public class ProjectTargetInstance_Internal_Tests
    {
        public static IEnumerable<object[]> TargetChildrenTestData
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
                    new ReadOnlyCollection<ProjectTargetInstanceChild>(new ProjectTargetInstanceChild[0]),
                    new ReadOnlyCollection<ProjectOnErrorInstance>(new ProjectOnErrorInstance[0])
                };

                yield return new object[]
                {
                    new ReadOnlyCollection<ProjectTargetInstanceChild>(
                        new ProjectTargetInstanceChild[]
                        {
                            CreateTargetPropertyGroup(),
                            CreateTargetItemGroup(),
                            CreateTargetOnError(),
                            CreateTargetTask()
                        }
                    ),
                    new ReadOnlyCollection<ProjectOnErrorInstance>(new[] {CreateTargetOnError()})
                };

                yield return new object[]
                {
                    new ReadOnlyCollection<ProjectTargetInstanceChild>(
                        new ProjectTargetInstanceChild[]
                        {
                            CreateTargetPropertyGroup(),
                            CreateTargetItemGroup(),
                            CreateTargetPropertyGroup(),
                            CreateTargetItemGroup(),
                            CreateTargetOnError(),
                            CreateTargetTask(),
                            CreateTargetOnError(),
                            CreateTargetTask()
                        }
                    ),
                    new ReadOnlyCollection<ProjectOnErrorInstance>(new[]
                    {
                        CreateTargetOnError(),
                        CreateTargetOnError()
                    })
                };
            }
        }

        [Theory]
        [MemberData(nameof(TargetChildrenTestData), DisableDiscoveryEnumeration = true)]
        public void ProjectTargetInstanceCanSerializeViaTranslator(
            ReadOnlyCollection<ProjectTargetInstanceChild> children,
            ReadOnlyCollection<ProjectOnErrorInstance> errorChildren)
        {
            var original = CreateTarget(null, children, errorChildren);

            ((ITranslatable) original).Translate(TranslationHelpers.GetWriteTranslator());
            var copy = ProjectTargetInstance.FactoryForDeserialization(TranslationHelpers.GetReadTranslator());

            Assert.Equal(original, copy, new TargetComparer());
        }
    }
}
