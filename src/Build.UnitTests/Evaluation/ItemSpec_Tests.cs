// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using net.r_eg.IeXod.Collections;
using net.r_eg.IeXod.Execution;
using net.r_eg.IeXod.Shared;
using net.r_eg.IeXod.Shared.FileSystem;
using net.r_eg.IeXod.UnitTests.BackEnd;
using Shouldly;
using Xunit;
using ProjectInstanceItemSpec =
    net.r_eg.IeXod.Evaluation.ItemSpec<net.r_eg.IeXod.Execution.ProjectPropertyInstance, net.r_eg.IeXod.Execution.ProjectItemInstance>;
using ProjectInstanceExpander =
    net.r_eg.IeXod.Evaluation.Expander<net.r_eg.IeXod.Execution.ProjectPropertyInstance, net.r_eg.IeXod.Execution.ProjectItemInstance>;


namespace net.r_eg.IeXod.UnitTests.OM.Evaluation
{
    public class ItemSpec_Tests
    {
        [Fact]
        public void EachFragmentTypeShouldContributeToItemSpecGlob()
        {
            var itemSpec = CreateItemSpecFrom("a;b*;c*;@(foo)", CreateExpander(new Dictionary<string, string[]> {{"foo", new[] {"d", "e"}}}));

            var itemSpecGlob = itemSpec.ToMSBuildGlob();

            Assert.True(itemSpecGlob.IsMatch("a"));
            Assert.True(itemSpecGlob.IsMatch("bar"));
            Assert.True(itemSpecGlob.IsMatch("car"));
            Assert.True(itemSpecGlob.IsMatch("d"));
            Assert.True(itemSpecGlob.IsMatch("e"));
        }

        [Fact]
        public void AbsolutePathsShouldMatch()
        {
            var absoluteRootPath = NativeMethodsShared.IsWindows
                ? @"c:\a\b"
                : "/a/b";

            var projectFile = Path.Combine(absoluteRootPath, "build.proj");
            var absoluteSpec = Path.Combine(absoluteRootPath, "s.cs");

            var itemSpecFromAbsolute = CreateItemSpecFrom(absoluteSpec, CreateExpander(new Dictionary<string, string[]>()), new MockElementLocation(projectFile));
            var itemSpecFromRelative = CreateItemSpecFrom("s.cs", CreateExpander(new Dictionary<string, string[]>()), new MockElementLocation(projectFile));

            itemSpecFromRelative.ToMSBuildGlob().IsMatch("s.cs").ShouldBeTrue();
            itemSpecFromRelative.ToMSBuildGlob().IsMatch(absoluteSpec).ShouldBeTrue();

            itemSpecFromAbsolute.ToMSBuildGlob().IsMatch("s.cs").ShouldBeTrue();
            itemSpecFromAbsolute.ToMSBuildGlob().IsMatch(absoluteSpec).ShouldBeTrue();
        }

        [Fact]
        public void FragmentGlobsWorkAfterStateIsPartiallyInitializedByOtherOperations()
        {
            var itemSpec = CreateItemSpecFrom("a;b*;c*;@(foo)", CreateExpander(new Dictionary<string, string[]> {{"foo", new[] {"d", "e"}}}));

            int matches;
            // cause partial Lazy state to initialize in the ItemExpressionFragment
            itemSpec.FragmentsMatchingItem("e", out matches);

            Assert.Equal(1, matches);

            var itemSpecGlob = itemSpec.ToMSBuildGlob();

            Assert.True(itemSpecGlob.IsMatch("a"));
            Assert.True(itemSpecGlob.IsMatch("bar"));
            Assert.True(itemSpecGlob.IsMatch("car"));
            Assert.True(itemSpecGlob.IsMatch("d"));
            Assert.True(itemSpecGlob.IsMatch("e"));
        }

        private ProjectInstanceItemSpec CreateItemSpecFrom(string itemSpec, ProjectInstanceExpander expander, IElementLocation location = null)
        {
            location = location ?? MockElementLocation.Instance;

            return new ProjectInstanceItemSpec(itemSpec, expander, location, Path.GetDirectoryName(location.File));
        }

        private ProjectInstanceExpander CreateExpander(Dictionary<string, string[]> items)
        {
            var itemDictionary = ToItemDictionary(items);

            return new ProjectInstanceExpander(new PropertyDictionary<ProjectPropertyInstance>(), itemDictionary, (IFileSystem) FileSystems.Default);
        }

        private static ItemDictionary<ProjectItemInstance> ToItemDictionary(Dictionary<string, string[]> itemTypes)
        {
            var itemDictionary = new ItemDictionary<ProjectItemInstance>();

            var dummyProject = ProjectHelpers.CreateEmptyProjectInstance();

            foreach (var itemType in itemTypes)
            {
                foreach (var item in itemType.Value)
                {
                    itemDictionary.Add(new ProjectItemInstance(dummyProject, itemType.Key, item, dummyProject.FullPath));
                }
            }

            return itemDictionary;
        }
    }
}
