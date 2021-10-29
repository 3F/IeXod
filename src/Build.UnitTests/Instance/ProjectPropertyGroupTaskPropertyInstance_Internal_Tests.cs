// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using net.r_eg.IeXod.BackEnd;
using net.r_eg.IeXod.Engine.UnitTests.TestComparers;
using net.r_eg.IeXod.Execution;
using net.r_eg.IeXod.UnitTests;
using net.r_eg.IeXod.UnitTests.BackEnd;
using Xunit;

using static net.r_eg.IeXod.Engine.UnitTests.TestData.ProjectInstanceTestObjects;

namespace net.r_eg.IeXod.Engine.UnitTests.Instance
{
    public class ProjectPropertyGroupTaskPropertyInstance_Internal_Tests
    {
        [Fact]
        public void ProjectPropertyGroupTaskPropertyInstanceCanSerializeViaTranslator()
        {
            var original = CreateTargetProperty();

            ((ITranslatable) original).Translate(TranslationHelpers.GetWriteTranslator());
            var copy = ProjectPropertyGroupTaskPropertyInstance.FactoryForDeserialization(TranslationHelpers.GetReadTranslator());

            Assert.Equal(original, copy, new TargetPropertyComparer());
        }
    }
}
