// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using net.r_eg.IeXod.BackEnd;
using net.r_eg.IeXod.Execution;
using net.r_eg.IeXod.UnitTests.BackEnd;
using Xunit;

using static net.r_eg.IeXod.Engine.UnitTests.TestComparers.ProjectInstanceModelTestComparers;
using static net.r_eg.IeXod.Engine.UnitTests.TestData.ProjectInstanceTestObjects;

namespace net.r_eg.IeXod.Engine.UnitTests.Instance
{
    public class ProjectItemGroupTaskItemInstance_Internal_Tests
    {
        public static IEnumerable<object[]> MetadataTestData
        {
            get
            {
                yield return new object[]
                {
                    new List<ProjectItemGroupTaskMetadataInstance>()
                };

                yield return new object[]
                {
                    new List<ProjectItemGroupTaskMetadataInstance>
                    {
                        CreateTargetItemMetadata(1),
                        CreateTargetItemMetadata(2)
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(MetadataTestData))]
        public void ProjectItemGroupTaskItemInstanceCanSerializeViaTranslator(List<ProjectItemGroupTaskMetadataInstance> metadata)
        {
            var original = CreateTargetItem(null, metadata);

            ((ITranslatable) original).Translate(TranslationHelpers.GetWriteTranslator());
            var clone = ProjectItemGroupTaskItemInstance.FactoryForDeserialization(TranslationHelpers.GetReadTranslator());

            Assert.Equal(original, clone, new TargetItemComparer());
        }
    }
}
