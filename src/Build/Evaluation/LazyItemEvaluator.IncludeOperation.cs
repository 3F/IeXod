﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.Construction;
using net.r_eg.IeXod.Eventing;
using net.r_eg.IeXod.Internal;
using net.r_eg.IeXod.Shared;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace net.r_eg.IeXod.Evaluation
{
    internal partial class LazyItemEvaluator<P, I, M, D>
    {
        class IncludeOperation : LazyItemOperation
        {
            readonly int _elementOrder;
            
            readonly string _rootDirectory;

            readonly ImmutableList<string> _excludes;

            readonly ImmutableList<ProjectMetadataElement> _metadata;

            public IncludeOperation(IncludeOperationBuilder builder, LazyItemEvaluator<P, I, M, D> lazyEvaluator)
                : base(builder, lazyEvaluator)
            {
                _elementOrder = builder.ElementOrder;
                _rootDirectory = builder.RootDirectory;

                _excludes = builder.Excludes.ToImmutable();
                _metadata = builder.Metadata.ToImmutable();
            }

            protected override ImmutableList<I> SelectItems(ImmutableList<ItemData>.Builder listBuilder, ImmutableHashSet<string> globsToIgnore)
            {
                var itemsToAdd = ImmutableList.CreateBuilder<I>();

                Lazy<Func<string, bool>> excludeTester = null;
                ImmutableList<string>.Builder excludePatterns = ImmutableList.CreateBuilder<string>();
                if (_excludes != null)
                {
                    // STEP 4: Evaluate, split, expand and subtract any Exclude
                    foreach (string exclude in _excludes)
                    {
                        string excludeExpanded = _expander.ExpandIntoStringLeaveEscaped(exclude, ExpanderOptions.ExpandPropertiesAndItems, _itemElement.ExcludeLocation);
                        var excludeSplits = ExpressionShredder.SplitSemiColonSeparatedList(excludeExpanded);
                        excludePatterns.AddRange(excludeSplits);
                    }

                    if (excludePatterns.Count > 0)
                    {
                        excludeTester = new Lazy<Func<string, bool>>(() => EngineFileUtilities.GetFileSpecMatchTester(excludePatterns, _rootDirectory));
                    }
                }

                ISet<string> excludePatternsForGlobs = null;

                foreach (var fragment in _itemSpec.Fragments)
                {
                    if (fragment is ItemSpec<P, I>.ItemExpressionFragment itemReferenceFragment)
                    {
                        // STEP 3: If expression is "@(x)" copy specified list with its metadata, otherwise just treat as string
                        var itemsFromExpression = _expander.ExpandExpressionCaptureIntoItems(
                            itemReferenceFragment.Capture,
                            _evaluatorData,
                            _itemFactory,
                            ExpanderOptions.ExpandItems,
                            includeNullEntries: false,
                            isTransformExpression: out _,
                            elementLocation: _itemElement.IncludeLocation);

                        itemsToAdd.AddRange(
                            excludeTester != null
                                ? itemsFromExpression.Where(item => !excludeTester.Value(item.EvaluatedInclude))
                                : itemsFromExpression);
                    }
                    else if (fragment is ValueFragment valueFragment)
                    {
                        string value = valueFragment.TextFragment;

                        if (excludeTester == null ||
                            !excludeTester.Value(EscapingUtilities.UnescapeAll(value)))
                        {
                            var item = _itemFactory.CreateItem(value, value, _itemElement.ContainingProject.FullPath);
                            itemsToAdd.Add(item);
                        }
                    }
                    else if (fragment is GlobFragment globFragment)
                    {
                        string glob = globFragment.TextFragment;

                        if (excludePatternsForGlobs == null)
                        {
                            excludePatternsForGlobs = BuildExcludePatternsForGlobs(globsToIgnore, excludePatterns);
                        }

                        string[] includeSplitFilesEscaped;
                        if (MSBuildEventSource.Log.IsEnabled())
                        {
                            MSBuildEventSource.Log.ExpandGlobStart(_rootDirectory, glob, string.Join(", ", excludePatternsForGlobs));
                        }
                        using (_lazyEvaluator._evaluationProfiler.TrackGlob(_rootDirectory, glob, excludePatternsForGlobs))
                        {
                            includeSplitFilesEscaped = EngineFileUtilities.GetFileListEscaped(
                                _rootDirectory,
                                glob,
                                excludePatternsForGlobs
                            );
                        }
                        if (MSBuildEventSource.Log.IsEnabled())
                        {
                            MSBuildEventSource.Log.ExpandGlobStop(_rootDirectory, glob, string.Join(", ", excludePatternsForGlobs));
                        }

                        foreach (string includeSplitFileEscaped in includeSplitFilesEscaped)
                        {
                            itemsToAdd.Add(_itemFactory.CreateItem(includeSplitFileEscaped, glob, _itemElement.ContainingProject.FullPath));
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException(fragment.GetType().ToString());
                    }
                }

                return itemsToAdd.ToImmutable();
            }

            private static ISet<string> BuildExcludePatternsForGlobs(ImmutableHashSet<string> globsToIgnore, ImmutableList<string>.Builder excludePatterns)
            {
                var anyExcludes = excludePatterns.Count > 0;
                var anyGlobsToIgnore = globsToIgnore.Count > 0;

                if (anyGlobsToIgnore && anyExcludes)
                {
                    return excludePatterns.Concat(globsToIgnore).ToImmutableHashSet();
                }

                return anyExcludes ? excludePatterns.ToImmutableHashSet() : globsToIgnore;
            }

            protected override void MutateItems(ImmutableList<I> items)
            {
                DecorateItemsWithMetadata(items.Select(i => new ItemBatchingContext(i)), _metadata);
            }

            protected override void SaveItems(ImmutableList<I> items, ImmutableList<ItemData>.Builder listBuilder)
            {
                foreach (var item in items)
                {
                    listBuilder.Add(new ItemData(item, _itemElement, _elementOrder, _conditionResult));
                }
            }
        }

        class IncludeOperationBuilder : OperationBuilderWithMetadata
        {
            public int ElementOrder { get; set; }
            public string RootDirectory { get; set; }

            public ImmutableList<string>.Builder Excludes { get; } = ImmutableList.CreateBuilder<string>();

            public IncludeOperationBuilder(ProjectItemElement itemElement, bool conditionResult) : base(itemElement, conditionResult)
            {
            }
        }
    }
}
