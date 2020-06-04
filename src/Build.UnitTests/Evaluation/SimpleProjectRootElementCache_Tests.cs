﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using net.r_eg.IeXod.Construction;
using net.r_eg.IeXod.Evaluation;
using net.r_eg.IeXod.Shared;
using Shouldly;
using Xunit;

namespace net.r_eg.IeXod.Engine.UnitTests.Evaluation
{
    public class SimpleProjectRootElementCache_Tests : IDisposable
    {
        public SimpleProjectRootElementCache_Tests()
        {
            // Empty the cache
            ProjectCollection.GlobalProjectCollection.UnloadAllProjects();
        }

        public void Dispose()
        {
            // Empty the cache
            ProjectCollection.GlobalProjectCollection.UnloadAllProjects();
        }

        [Fact]
        public void Get_GivenCachedProjectFile_ReturnsRootElement()
        {
            string projectFile = NativeMethodsShared.IsUnixLike ? "/foo" : "c:\\foo";
            string projectFileToCache = NativeMethodsShared.IsUnixLike ? "/foo" : "c:\\foo";
            ProjectRootElement rootElementToCache = ProjectRootElement.Create(projectFileToCache);

            var cache = new SimpleProjectRootElementCache();
            cache.AddEntry(rootElementToCache);

            ProjectRootElement actualRootElement = cache.Get(projectFile, null, false, null);
            actualRootElement.ShouldBe(rootElementToCache);
        }

        [Fact]
        public void Get_GivenCachedProjectFileWithDifferentCasing_ReturnsRootElement()
        {
            string projectFile = NativeMethodsShared.IsUnixLike ? "/foo" : "c:\\foo";
            string projectFileToCache = NativeMethodsShared.IsUnixLike ? "/Foo" : "c:\\Foo";
            ProjectRootElement rootElementToCache = ProjectRootElement.Create(projectFileToCache);

            var cache = new SimpleProjectRootElementCache();
            cache.AddEntry(rootElementToCache);

            ProjectRootElement actualRootElement = cache.Get(projectFile, null, false, null);
            actualRootElement.ShouldBe(rootElementToCache);
        }

        [Fact]
        public void Get_GivenOpenFuncWhichAddsRootElement_ReturnsRootElement()
        {
            string projectFile = NativeMethodsShared.IsUnixLike ? "/foo" : "c:\\foo";
            string projectFileToCache = NativeMethodsShared.IsUnixLike ? "/foo" : "c:\\foo";
            ProjectRootElement rootElementToCache = ProjectRootElement.Create(projectFileToCache);
            ProjectRootElement OpenFunc(string pathArg, ProjectRootElementCacheBase cacheArg)
            {
                cacheArg.AddEntry(rootElementToCache);
                return rootElementToCache;
            }

            var cache = new SimpleProjectRootElementCache();
            cache.AddEntry(rootElementToCache);

            ProjectRootElement actualRootElement = cache.Get(projectFile, OpenFunc, false, null);
            actualRootElement.ShouldBe(rootElementToCache);
        }

        [Fact]
        public void Get_GivenOpenFuncWhichAddsRootElementWithDifferentCasing_ReturnsRootElement()
        {
            string projectFile = NativeMethodsShared.IsUnixLike ? "/foo" : "c:\\foo";
            string projectFileToCache = NativeMethodsShared.IsUnixLike ? "/Foo" : "c:\\Foo";
            ProjectRootElement rootElementToCache = ProjectRootElement.Create(projectFileToCache);
            ProjectRootElement OpenFunc(string pathArg, ProjectRootElementCacheBase cacheArg)
            {
                cacheArg.AddEntry(rootElementToCache);
                return rootElementToCache;
            }

            var cache = new SimpleProjectRootElementCache();
            cache.AddEntry(rootElementToCache);

            ProjectRootElement actualRootElement = cache.Get(projectFile, OpenFunc, false, null);
            actualRootElement.ShouldBe(rootElementToCache);
        }

        [Fact]
        public void Get_GivenOpenFuncWhichReturnsNull_ThrowsInternalErrorException()
        {
            string projectFile = NativeMethodsShared.IsUnixLike ? "/foo" : "c:\\foo";
            ProjectRootElement OpenFunc(string pathArg, ProjectRootElementCacheBase cacheArg) => null;

            var cache = new SimpleProjectRootElementCache();

            Should.Throw<InternalErrorException>(() =>
            {
                cache.Get(projectFile, OpenFunc, false, null);
            });
        }

        [Fact]
        public void Get_GivenOpenFuncWhichReturnsIncorrectProject_ThrowsInternalErrorException()
        {
            string projectFile = NativeMethodsShared.IsUnixLike ? "/foo" : "c:\\foo";
            string projectFileToCache = NativeMethodsShared.IsUnixLike ? "/bar" : "c:\\bar";
            ProjectRootElement rootElementToCache = ProjectRootElement.Create(projectFileToCache);
            ProjectRootElement OpenFunc(string pathArg, ProjectRootElementCacheBase cacheArg)
            {
                cacheArg.AddEntry(rootElementToCache);
                return rootElementToCache;
            }

            var cache = new SimpleProjectRootElementCache();

            Should.Throw<InternalErrorException>(() =>
            {
                cache.Get(projectFile, OpenFunc, false, null);
            });
        }

        [Fact]
        public void Get_GivenOpenFuncWhichDoesNotAddToCache_ThrowsInternalErrorException()
        {
            string projectFile = NativeMethodsShared.IsUnixLike ? "/foo" : "c:\\foo";
            string openFuncPath = NativeMethodsShared.IsUnixLike ? "/foo" : "c:\\foo";
            ProjectRootElement openFuncElement = ProjectRootElement.Create(openFuncPath);
            ProjectRootElement OpenFunc(string pathArg, ProjectRootElementCacheBase cacheArg) => openFuncElement;

            var cache = new SimpleProjectRootElementCache();

            Should.Throw<InternalErrorException>(() =>
            {
                cache.Get(projectFile, OpenFunc, false, null);
            });
        }
    }
}
