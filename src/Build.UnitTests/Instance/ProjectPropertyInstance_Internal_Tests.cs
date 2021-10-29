// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using net.r_eg.IeXod.Execution;
using net.r_eg.IeXod.Shared;
using System;
using net.r_eg.IeXod.Evaluation;
using net.r_eg.IeXod.Construction;
using net.r_eg.IeXod.UnitTests.BackEnd;
using Xunit;

namespace net.r_eg.IeXod.UnitTests.OM.Instance
{
    /// <summary>
    /// Tests for ProjectPropertyInstance internal members
    /// </summary>
    public class ProjectPropertyInstance_Internal_Tests
    {
        /// <summary>
        /// Cloning
        /// </summary>
        [Fact]
        public void DeepClone()
        {
            ProjectPropertyInstance property = GetPropertyInstance();

            ProjectPropertyInstance clone = property.DeepClone();

            Assert.False(Object.ReferenceEquals(property, clone));
            Assert.Equal("p", clone.Name);
            Assert.Equal("v1", clone.EvaluatedValue);
        }

        /// <summary>
        /// Serialization test
        /// </summary>
        [Fact]
        public void Serialization()
        {
            ProjectPropertyInstance property = GetPropertyInstance();

            TranslationHelpers.GetWriteTranslator().Translate(ref property, ProjectPropertyInstance.FactoryForDeserialization);
            ProjectPropertyInstance deserializedProperty = null;
            TranslationHelpers.GetReadTranslator().Translate(ref deserializedProperty, ProjectPropertyInstance.FactoryForDeserialization);

            Assert.Equal(property.Name, deserializedProperty.Name);
            Assert.Equal(property.EvaluatedValue, deserializedProperty.EvaluatedValue);
        }

        /// <summary>
        /// Tests serialization.
        /// </summary>
        [Fact]
        public void ProjectPropertyInstanceSerializationTest_Mutable()
        {
            var property = ProjectPropertyInstance.Create("p", "v", false /*mutable*/);
            Assert.False(property.IsImmutable);

            TranslationHelpers.GetWriteTranslator().Translate(ref property, ProjectPropertyInstance.FactoryForDeserialization);
            ProjectPropertyInstance deserializedProperty = null;
            TranslationHelpers.GetReadTranslator().Translate(ref deserializedProperty, ProjectPropertyInstance.FactoryForDeserialization);

            Assert.Equal(property.Name, deserializedProperty.Name);
            Assert.Equal(property.EvaluatedValue, deserializedProperty.EvaluatedValue);
            Assert.Equal(property.IsImmutable, deserializedProperty.IsImmutable);
            Assert.Equal(typeof(ProjectPropertyInstance), property.GetType());
        }

        /// <summary>
        /// Tests serialization.
        /// </summary>
        [Fact]
        public void ProjectPropertyInstanceSerializationTest_Immutable()
        {
            var property = ProjectPropertyInstance.Create("p", "v", mayBeReserved: true, isImmutable: true);
            Assert.True(property.IsImmutable);

            TranslationHelpers.GetWriteTranslator().Translate(ref property, ProjectPropertyInstance.FactoryForDeserialization);
            ProjectPropertyInstance deserializedProperty = null;
            TranslationHelpers.GetReadTranslator().Translate(ref deserializedProperty, ProjectPropertyInstance.FactoryForDeserialization);

            Assert.Equal(property.Name, deserializedProperty.Name);
            Assert.Equal(property.EvaluatedValue, deserializedProperty.EvaluatedValue);
            Assert.Equal(property.IsImmutable, deserializedProperty.IsImmutable);
            Assert.Equal("net.r_eg.IeXod.Execution.ProjectPropertyInstance+ProjectPropertyInstanceImmutable", property.GetType().ToString());
        }

        /// <summary>
        /// Get a ProjectPropertyInstance
        /// </summary>
        private static ProjectPropertyInstance GetPropertyInstance()
        {
            Project project = new Project();
            ProjectInstance projectInstance = project.CreateProjectInstance();
            ProjectPropertyInstance property = projectInstance.SetProperty("p", "v1");

            return property;
        }
    }
}
