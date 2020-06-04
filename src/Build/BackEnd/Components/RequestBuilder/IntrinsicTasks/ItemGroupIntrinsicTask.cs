﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;
using net.r_eg.IeXod.Collections;
using ElementLocation = net.r_eg.IeXod.Construction.ElementLocation;
using net.r_eg.IeXod.Execution;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Shared;
using net.r_eg.IeXod.Evaluation;
using net.r_eg.IeXod.Shared.FileSystem;
using TaskItem = net.r_eg.IeXod.Execution.ProjectItemInstance.TaskItem;
using ProjectItemInstanceFactory = net.r_eg.IeXod.Execution.ProjectItemInstance.TaskItem.ProjectItemInstanceFactory;
using EngineFileUtilities = net.r_eg.IeXod.Internal.EngineFileUtilities;
using TargetLoggingContext = net.r_eg.IeXod.BackEnd.Logging.TargetLoggingContext;

namespace net.r_eg.IeXod.BackEnd
{
    /// <summary>
    /// Implementation of the ItemGroup intrinsic task
    /// </summary>
    internal class ItemGroupIntrinsicTask : IntrinsicTask
    {
        /// <summary>
        /// The task instance data
        /// </summary>
        private ProjectItemGroupTaskInstance _taskInstance;

        private EngineFileUtilities _engineFileUtilities;

        /// <summary>
        /// Instantiates an ItemGroup task
        /// </summary>
        /// <param name="taskInstance">The original task instance data</param>
        /// <param name="loggingContext">The logging context</param>
        /// <param name="projectInstance">The project instance</param>
        /// <param name="logTaskInputs">Flag to determine whether or not to log task inputs.</param>
        public ItemGroupIntrinsicTask(ProjectItemGroupTaskInstance taskInstance, TargetLoggingContext loggingContext, ProjectInstance projectInstance, bool logTaskInputs)
            : base(loggingContext, projectInstance, logTaskInputs)
        {
            _taskInstance = taskInstance;
            _engineFileUtilities = EngineFileUtilities.Default;
        }

        /// <summary>
        /// Execute an ItemGroup element, including each child item expression
        /// </summary>
        /// <param name="lookup">The lookup used for evaluation and as a destination for these items.</param>
        internal override void ExecuteTask(Lookup lookup)
        {
            foreach (ProjectItemGroupTaskItemInstance child in _taskInstance.Items)
            {
                List<ItemBucket> buckets = null;

                try
                {
                    List<string> parameterValues = new List<string>();
                    GetBatchableValuesFromBuildItemGroupChild(parameterValues, child);
                    buckets = BatchingEngine.PrepareBatchingBuckets(parameterValues, lookup, child.ItemType, _taskInstance.Location);

                    // "Execute" each bucket
                    foreach (ItemBucket bucket in buckets)
                    {
                        bool condition = ConditionEvaluator.EvaluateCondition
                            (
                            child.Condition,
                            ParserOptions.AllowAll,
                            bucket.Expander,
                            ExpanderOptions.ExpandAll,
                            Project.Directory,
                            child.ConditionLocation,
                            LoggingContext.LoggingService,
                            LoggingContext.BuildEventContext,
                            FileSystems.Default);

                        if (condition)
                        {
                            HashSet<string> keepMetadata = null;
                            HashSet<string> removeMetadata = null;
                            HashSet<string> matchOnMetadata = null;
                            MatchOnMetadataOptions matchOnMetadataOptions = MatchOnMetadataConstants.MatchOnMetadataOptionsDefaultValue;

                            if (!String.IsNullOrEmpty(child.KeepMetadata))
                            {
                                var keepMetadataEvaluated = bucket.Expander.ExpandIntoStringListLeaveEscaped(child.KeepMetadata, ExpanderOptions.ExpandAll, child.KeepMetadataLocation).ToList();
                                if (keepMetadataEvaluated.Count > 0)
                                {
                                    keepMetadata = new HashSet<string>(keepMetadataEvaluated);
                                }
                            }

                            if (!String.IsNullOrEmpty(child.RemoveMetadata))
                            {
                                var removeMetadataEvaluated = bucket.Expander.ExpandIntoStringListLeaveEscaped(child.RemoveMetadata, ExpanderOptions.ExpandAll, child.RemoveMetadataLocation).ToList();
                                if (removeMetadataEvaluated.Count > 0)
                                {
                                    removeMetadata = new HashSet<string>(removeMetadataEvaluated);
                                }
                            }

                            if (!String.IsNullOrEmpty(child.MatchOnMetadata))
                            {
                                var matchOnMetadataEvaluated = bucket.Expander.ExpandIntoStringListLeaveEscaped(child.MatchOnMetadata, ExpanderOptions.ExpandAll, child.MatchOnMetadataLocation).ToList();
                                if (matchOnMetadataEvaluated.Count > 0)
                                {
                                    matchOnMetadata = new HashSet<string>(matchOnMetadataEvaluated);
                                }

                                Enum.TryParse(child.MatchOnMetadataOptions, out matchOnMetadataOptions);
                            }

                            if ((child.Include.Length != 0) ||
                                (child.Exclude.Length != 0))
                            {
                                // It's an item -- we're "adding" items to the world
                                ExecuteAdd(child, bucket, keepMetadata, removeMetadata);
                            }
                            else if (child.Remove.Length != 0)
                            {
                                // It's a remove -- we're "removing" items from the world
                                ExecuteRemove(child, bucket, matchOnMetadata, matchOnMetadataOptions);
                            }
                            else
                            {
                                // It's a modify -- changing existing items
                                ExecuteModify(child, bucket, keepMetadata, removeMetadata);
                            }
                        }
                    }
                }
                finally
                {
                    if (buckets != null)
                    {
                        // Propagate the item changes to the bucket above
                        foreach (ItemBucket bucket in buckets)
                        {
                            bucket.LeaveScope();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Add items to the world. This is the in-target equivalent of an item include expression outside of a target.
        /// </summary>
        /// <param name="child">The item specification to evaluate and add.</param>
        /// <param name="bucket">The batching bucket.</param>
        /// <param name="keepMetadata">An <see cref="ISet{String}"/> of metadata names to keep.</param>
        /// <param name="removeMetadata">An <see cref="ISet{String}"/> of metadata names to remove.</param>
        private void ExecuteAdd(ProjectItemGroupTaskItemInstance child, ItemBucket bucket, ISet<string> keepMetadata, ISet<string> removeMetadata)
        {
            // First, collect up the appropriate metadata collections.  We need the one from the item definition, if any, and
            // the one we are using for this batching bucket.
            ProjectItemDefinitionInstance itemDefinition = null;
            Project.ItemDefinitions.TryGetValue(child.ItemType, out itemDefinition);

            // The NestedMetadataTable will handle the aggregation of the different metadata collections
            NestedMetadataTable metadataTable = new NestedMetadataTable(child.ItemType, bucket.Expander.Metadata, itemDefinition);
            IMetadataTable originalMetadataTable = bucket.Expander.Metadata;

            bucket.Expander.Metadata = metadataTable;

            // Second, expand the item include and exclude, and filter existing metadata as appropriate.
            List<ProjectItemInstance> itemsToAdd = ExpandItemIntoItems(child, bucket.Expander, keepMetadata, removeMetadata);

            // Third, expand the metadata.           
            foreach (ProjectItemGroupTaskMetadataInstance metadataInstance in child.Metadata)
            {
                bool condition = ConditionEvaluator.EvaluateCondition
                    (
                    metadataInstance.Condition,
                    ParserOptions.AllowAll,
                    bucket.Expander,
                    ExpanderOptions.ExpandAll,
                    Project.Directory,
                    metadataInstance.Location,
                    LoggingContext.LoggingService,
                    LoggingContext.BuildEventContext,
                    FileSystems.Default);

                if (condition)
                {
                    string evaluatedValue = bucket.Expander.ExpandIntoStringLeaveEscaped(metadataInstance.Value, ExpanderOptions.ExpandAll, metadataInstance.Location);

                    // This both stores the metadata so we can add it to all the items we just created later, and 
                    // exposes this metadata to further metadata evaluations in subsequent loop iterations.
                    metadataTable.SetValue(metadataInstance.Name, evaluatedValue);
                }
            }

            // Finally, copy the added metadata onto the new items.  The set call is additive.
            ProjectItemInstance.SetMetadata(metadataTable.AddedMetadata, itemsToAdd); // Add in one operation for potential copy-on-write

            // Restore the original metadata table.
            bucket.Expander.Metadata = originalMetadataTable;

            // Determine if we should NOT add duplicate entries
            bool keepDuplicates = ConditionEvaluator.EvaluateCondition
                (
                child.KeepDuplicates,
                ParserOptions.AllowAll,
                bucket.Expander,
                ExpanderOptions.ExpandAll,
                Project.Directory,
                child.KeepDuplicatesLocation,
                LoggingContext.LoggingService,
                LoggingContext.BuildEventContext,
                FileSystems.Default);

            if (LogTaskInputs && !LoggingContext.LoggingService.OnlyLogCriticalEvents && itemsToAdd != null && itemsToAdd.Count > 0)
            {
                var itemGroupText = ItemGroupLoggingHelper.GetParameterText(
                    ItemGroupLoggingHelper.ItemGroupIncludeLogMessagePrefix,
                    child.ItemType,
                    itemsToAdd,
                    logItemMetadata: true);
                LoggingContext.LogCommentFromText(MessageImportance.Low, itemGroupText);
            }

            // Now add the items we created to the lookup.
            bucket.Lookup.AddNewItemsOfItemType(child.ItemType, itemsToAdd, !keepDuplicates); // Add in one operation for potential copy-on-write
        }

        /// <summary>
        /// Remove items from the world. Removes to items that are part of the project manifest are backed up, so 
        /// they can be reverted when the project is reset after the end of the build.
        /// </summary>
        /// <param name="child">The item specification to evaluate and remove.</param>
        /// <param name="bucket">The batching bucket.</param>
        private void ExecuteRemove(ProjectItemGroupTaskItemInstance child, ItemBucket bucket, HashSet<string> matchOnMetadata, MatchOnMetadataOptions matchingOptions)
        {
            ICollection<ProjectItemInstance> group = bucket.Lookup.GetItems(child.ItemType);
            if (group == null)
            {
                // No items of this type to remove
                return;
            }

            List<ProjectItemInstance> itemsToRemove = null;

            if (matchOnMetadata == null)
            {
                itemsToRemove = FindItemsMatchingSpecification(group, child.Remove, child.RemoveLocation, bucket.Expander);
            }
            else
            {
                itemsToRemove = FindItemsUsingMatchOnMetadata(group, child, bucket, matchOnMetadata, matchingOptions);
            }

            if (itemsToRemove != null)
            {
                if (LogTaskInputs && !LoggingContext.LoggingService.OnlyLogCriticalEvents && itemsToRemove.Count > 0)
                {
                    var itemGroupText = ItemGroupLoggingHelper.GetParameterText(
                        ItemGroupLoggingHelper.ItemGroupRemoveLogMessage,
                        child.ItemType,
                        itemsToRemove,
                        logItemMetadata: true);
                    LoggingContext.LogCommentFromText(MessageImportance.Low, itemGroupText);
                }

                bucket.Lookup.RemoveItems(itemsToRemove);
            }
        }

        private List<ProjectItemInstance> FindItemsUsingMatchOnMetadata(
            ICollection<ProjectItemInstance> items,
            ProjectItemGroupTaskItemInstance child,
            ItemBucket bucket,
            HashSet<string> matchOnMetadata,
            MatchOnMetadataOptions options)
        {
            ErrorUtilities.VerifyThrowArgumentNull(matchOnMetadata, nameof(matchOnMetadata));

            var itemSpec = new ItemSpec<ProjectPropertyInstance, ProjectItemInstance>(child.Remove, bucket.Expander, child.RemoveLocation, Project.Directory, true);

            ProjectFileErrorUtilities.VerifyThrowInvalidProjectFile(
                itemSpec.Fragments.Count == 1
                && itemSpec.Fragments.First() is ItemSpec<ProjectPropertyInstance, ProjectItemInstance>.ItemExpressionFragment
                && matchOnMetadata.Count == 1,
                new BuildEventFileInfo(string.Empty),
                "OM_MatchOnMetadataIsRestrictedToOnlyOneReferencedItem",
                child.RemoveLocation,
                child.Remove);

            return items.Where(item => itemSpec.MatchesItemOnMetadata(item, matchOnMetadata, options)).ToList();
        }

        /// <summary>
        /// Modifies items in the world - specifically, changes their metadata. Changes to items that are part of the project manifest are backed up, so 
        /// they can be reverted when the project is reset after the end of the build.
        /// </summary>
        /// <param name="child">The item specification to evaluate and modify.</param>
        /// <param name="bucket">The batching bucket.</param>
        /// <param name="keepMetadata">An <see cref="ISet{String}"/> of metadata names to keep.</param>
        /// <param name="removeMetadata">An <see cref="ISet{String}"/> of metadata names to remove.</param>
        private void ExecuteModify(ProjectItemGroupTaskItemInstance child, ItemBucket bucket, ISet<string> keepMetadata, ISet<string> removeMetadata)
        {
            ICollection<ProjectItemInstance> group = bucket.Lookup.GetItems(child.ItemType);
            if (group == null || group.Count == 0)
            {
                // No items of this type to modify
                return;
            }

            // Figure out what metadata names and values we need to set
            var metadataToSet = new Lookup.MetadataModifications(keepMetadata != null);

            // Filter the metadata as appropriate
            if (keepMetadata != null)
            {
                foreach (var metadataName in keepMetadata)
                {
                    metadataToSet[metadataName] = Lookup.MetadataModification.CreateFromNoChange();
                }
            }
            else if (removeMetadata != null)
            {
                foreach (var metadataName in removeMetadata)
                {
                    metadataToSet[metadataName] = Lookup.MetadataModification.CreateFromRemove();
                }
            }

            foreach (ProjectItemGroupTaskMetadataInstance metadataInstance in child.Metadata)
            {
                bool condition = ConditionEvaluator.EvaluateCondition
                    (
                    metadataInstance.Condition,
                    ParserOptions.AllowAll,
                    bucket.Expander,
                    ExpanderOptions.ExpandAll,
                    Project.Directory,
                    metadataInstance.ConditionLocation,
                    LoggingContext.LoggingService,
                    LoggingContext.BuildEventContext,
                    FileSystems.Default);

                if (condition)
                {
                    string evaluatedValue = bucket.Expander.ExpandIntoStringLeaveEscaped(metadataInstance.Value, ExpanderOptions.ExpandAll, metadataInstance.Location);
                    metadataToSet[metadataInstance.Name] = Lookup.MetadataModification.CreateFromNewValue(evaluatedValue);
                }
            }

            // Now apply the changes.  This must be done after filtering, since explicitly set metadata overrides filters.
            bucket.Lookup.ModifyItems(child.ItemType, group, metadataToSet);
        }

        /// <summary>
        /// Adds batchable parameters from an item element into the list. If the item element was a task, these
        /// would be its raw parameter values.
        /// </summary>
        /// <param name="parameterValues">The list of batchable values</param>
        /// <param name="child">The item from which to find batchable values</param>
        private void GetBatchableValuesFromBuildItemGroupChild(List<string> parameterValues, ProjectItemGroupTaskItemInstance child)
        {
            AddIfNotEmptyString(parameterValues, child.Include);
            AddIfNotEmptyString(parameterValues, child.Exclude);
            AddIfNotEmptyString(parameterValues, child.Remove);
            AddIfNotEmptyString(parameterValues, child.Condition);

            foreach (ProjectItemGroupTaskMetadataInstance metadataElement in child.Metadata)
            {
                AddIfNotEmptyString(parameterValues, metadataElement.Value);
                AddIfNotEmptyString(parameterValues, metadataElement.Condition);
            }
        }

        /// <summary>
        /// Takes an item specification, evaluates it and expands it into a list of items
        /// </summary>
        /// <param name="originalItem">The original item data</param>
        /// <param name="expander">The expander to use.</param>
        /// <param name="keepMetadata">An <see cref="ISet{String}"/> of metadata names to keep.</param>
        /// <param name="removeMetadata">An <see cref="ISet{String}"/> of metadata names to remove.</param>
        /// <remarks>
        /// This code is very close to that which exists in the Evaluator.EvaluateItemXml method.  However, because
        /// it invokes type constructors, and those constructors take arguments of fundamentally different types, it has not
        /// been refactored.
        /// </remarks>
        /// <returns>A list of items.</returns>
        private List<ProjectItemInstance> ExpandItemIntoItems
        (
            ProjectItemGroupTaskItemInstance originalItem,
            Expander<ProjectPropertyInstance, ProjectItemInstance> expander,
            ISet<string> keepMetadata,
            ISet<string> removeMetadata
        )
        {
            //todo this is duplicated logic with the item computation logic from evaluation (in LazyIncludeOperation.SelectItems)

            ProjectErrorUtilities.VerifyThrowInvalidProject(!(keepMetadata != null && removeMetadata != null), originalItem.KeepMetadataLocation, "KeepAndRemoveMetadataMutuallyExclusive");
            List<ProjectItemInstance> items = new List<ProjectItemInstance>();

            // Expand properties and metadata in Include
            string evaluatedInclude = expander.ExpandIntoStringLeaveEscaped(originalItem.Include, ExpanderOptions.ExpandPropertiesAndMetadata, originalItem.IncludeLocation);

            if (evaluatedInclude.Length == 0)
            {
                return items;
            }

            // Compute exclude fragments, without expanding wildcards
            var excludes = ImmutableList<string>.Empty.ToBuilder();
            if (originalItem.Exclude.Length > 0)
            {
                string evaluatedExclude = expander.ExpandIntoStringLeaveEscaped(originalItem.Exclude, ExpanderOptions.ExpandAll, originalItem.ExcludeLocation);

                if (evaluatedExclude.Length > 0)
                {
                    var excludeSplits = ExpressionShredder.SplitSemiColonSeparatedList(evaluatedExclude);

                    foreach (string excludeSplit in excludeSplits)
                    {
                        excludes.Add(excludeSplit);
                    }
                }
            }

            // Split Include on any semicolons, and take each split in turn
            var includeSplits = ExpressionShredder.SplitSemiColonSeparatedList(evaluatedInclude);
            ProjectItemInstanceFactory itemFactory = new ProjectItemInstanceFactory(this.Project, originalItem.ItemType);

            foreach (string includeSplit in includeSplits)
            {
                // If expression is "@(x)" copy specified list with its metadata, otherwise just treat as string
                bool throwaway;

                IList<ProjectItemInstance> itemsFromSplit = expander.ExpandSingleItemVectorExpressionIntoItems(includeSplit,
                    itemFactory,
                    ExpanderOptions.ExpandItems,
                    false /* do not include null expansion results */,
                    out throwaway,
                    originalItem.IncludeLocation);

                if (itemsFromSplit != null)
                {
                    // Expression is in form "@(X)", so add these items directly.
                    foreach (ProjectItemInstance item in itemsFromSplit)
                    {
                        items.Add(item);
                    }
                }
                else
                {
                    // The expression is not of the form "@(X)". Treat as string

                    // Pass the non wildcard expanded excludes here to fix https://github.com/Microsoft/msbuild/issues/2621
                    string[] includeSplitFiles = _engineFileUtilities.GetFileListEscaped(
                        Project.Directory,
                        includeSplit,
                        excludes);

                    foreach (string includeSplitFile in includeSplitFiles)
                    {
                        items.Add(new ProjectItemInstance(
                            Project,
                            originalItem.ItemType,
                            includeSplitFile,
                            includeSplit /* before wildcard expansion */,
                            null,
                            null,
                            originalItem.Location.File));
                    }
                }
            }

            // Evaluate, split, expand and subtract any Exclude
            HashSet<string> excludesUnescapedForComparison = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (string excludeSplit in excludes)
            {
                string[] excludeSplitFiles = _engineFileUtilities.GetFileListUnescaped(Project.Directory, excludeSplit);

                foreach (string excludeSplitFile in excludeSplitFiles)
                {
                    excludesUnescapedForComparison.Add(excludeSplitFile);
                }
            }

            List<ProjectItemInstance> remainingItems = new List<ProjectItemInstance>();

            for (int i = 0; i < items.Count; i++)
            {
                if (!excludesUnescapedForComparison.Contains(((IItem)items[i]).EvaluatedInclude))
                {
                    remainingItems.Add(items[i]);
                }
            }

            items = remainingItems;

            // Filter the metadata as appropriate
            if (keepMetadata != null)
            {
                foreach (var item in items)
                {
                    var metadataToRemove = item.MetadataNames.Where(name => !keepMetadata.Contains(name));
                    foreach (var metadataName in metadataToRemove)
                    {
                        item.RemoveMetadata(metadataName);
                    }
                }
            }
            else if (removeMetadata != null)
            {
                foreach (var item in items)
                {
                    var metadataToRemove = item.MetadataNames.Where(name => removeMetadata.Contains(name));
                    foreach (var metadataName in metadataToRemove)
                    {
                        item.RemoveMetadata(metadataName);
                    }
                }
            }

            return items;
        }

        /// <summary>
        /// Returns a list of all items in the provided item group whose itemspecs match the specification, after it is split and any wildcards are expanded.
        /// If no items match, returns null.
        /// </summary>
        /// <param name="items">The items to match</param>
        /// <param name="specification">The specification to match against the items.</param>
        /// <param name="specificationLocation">The specification to match against the provided items</param>
        /// <param name="expander">The expander to use</param>
        /// <returns>A list of matching items</returns>
        private List<ProjectItemInstance> FindItemsMatchingSpecification
            (
            ICollection<ProjectItemInstance> items,
            string specification,
            ElementLocation specificationLocation,
            Expander<ProjectPropertyInstance, ProjectItemInstance> expander
            )
        {
            if (items.Count == 0 || specification.Length == 0)
            {
                return null;
            }

            // This is a hashtable whose key is the filename for the individual items
            // in the Exclude list, after wildcard expansion.
            HashSet<string> specificationsToFind = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Split by semicolons
            var specificationPieces = expander.ExpandIntoStringListLeaveEscaped(specification, ExpanderOptions.ExpandAll, specificationLocation);

            foreach (string piece in specificationPieces)
            {
                // Take each individual path or file expression, and expand any
                // wildcards.  Then loop through each file returned, and add it
                // to our hashtable.

                // Don't unescape wildcards just yet - if there were any escaped, the caller wants to treat them
                // as literals. Everything else is safe to unescape at this point, since we're only matching
                // against the file system.
                string[] fileList = _engineFileUtilities.GetFileListEscaped(Project.Directory, piece);

                foreach (string file in fileList)
                {
                    // Now unescape everything, because this is the end of the road for this filename.
                    // We're just going to compare it to the unescaped include path to filter out the
                    // file excludes.
                    specificationsToFind.Add(EscapingUtilities.UnescapeAll(file));
                }
            }

            if (specificationsToFind.Count == 0)
            {
                return null;
            }

            // Now loop through our list and filter out any that match a
            // filename in the remove list.
            List<ProjectItemInstance> itemsRemoved = new List<ProjectItemInstance>();

            foreach (ProjectItemInstance item in items)
            {
                // Even if the case for the excluded files is different, they
                // will still get excluded, as expected.  However, if the excluded path
                // references the same file in a different way, such as by relative
                // path instead of absolute path, we will not realize that they refer
                // to the same file, and thus we will not exclude it.
                if (specificationsToFind.Contains(item.EvaluatedInclude))
                {
                    itemsRemoved.Add(item);
                }
            }

            return itemsRemoved;
        }

        /// <summary>
        /// This class is used during ItemGroup intrinsic tasks to resolve metadata references.  It consists of three tables:
        /// 1. The metadata added during evaluation.
        /// 1. The metadata table created for the bucket, may be null.
        /// 2. The metadata table derived from the item definition group, may be null.
        /// </summary>
        private class NestedMetadataTable : IMetadataTable
        {
            /// <summary>
            /// The table for all metadata added during expansion
            /// </summary>
            private Dictionary<string, string> _addTable;

            /// <summary>
            /// The table for metadata which was generated for this batch bucket.
            /// May be null.
            /// </summary>
            private IMetadataTable _bucketTable;

            /// <summary>
            /// The table for metadata from the item definition
            /// May be null.
            /// </summary>
            private IMetadataTable _itemDefinitionTable;

            /// <summary>
            /// The item type to which this metadata applies.
            /// </summary>
            private string _itemType;

            /// <summary>
            /// Creates a new metadata table aggregating the bucket and item definition tables.
            /// </summary>
            /// <param name="itemType">The type of item for which we are doing evaluation.</param>
            /// <param name="bucketTable">The metadata table created for this batch bucket.  May be null.</param>
            /// <param name="itemDefinitionTable">The metadata table for the item definition representing this item.  May be null.</param>
            internal NestedMetadataTable(string itemType, IMetadataTable bucketTable, IMetadataTable itemDefinitionTable)
            {
                _itemType = itemType;
                _addTable = new Dictionary<string, string>(MSBuildNameIgnoreCaseComparer.Default);
                _bucketTable = bucketTable;
                _itemDefinitionTable = itemDefinitionTable;
            }

            /// <summary>
            /// Retrieves the metadata table used to collect additions.
            /// </summary>
            internal Dictionary<string, string> AddedMetadata
            {
                get { return _addTable; }
            }

            #region IMetadataTable Members
            // NOTE:  Leaving these methods public so as to avoid having to explicitly define them
            // through the IMetadataTable interface and then cast everywhere they're used.  This class
            // is private, so it ultimately doesn't matter. 

            /// <summary>
            /// Gets the specified metadata value.  Returns an empty string if none is set.
            /// </summary>
            public string GetEscapedValue(string name)
            {
                return GetEscapedValue(null, name);
            }

            /// <summary>
            /// Gets the specified metadata value for the qualified item type.  Returns an empty string if none is set.
            /// </summary>
            public string GetEscapedValue(string specifiedItemType, string name)
            {
                return GetEscapedValueIfPresent(specifiedItemType, name) ?? String.Empty;
            }

            /// <summary>
            /// Gets the specified metadata value for the qualified item type.  Returns null if none is set.
            /// </summary>
            public string GetEscapedValueIfPresent(string specifiedItemType, string name)
            {
                string value = null;
                if (null == specifiedItemType || specifiedItemType == _itemType)
                {
                    // Look in the addTable
                    if (_addTable.TryGetValue(name, out value))
                    {
                        return value;
                    }
                }

                // Look in the bucket table
                if (null != _bucketTable)
                {
                    value = _bucketTable.GetEscapedValueIfPresent(specifiedItemType, name);
                    if (null != value)
                    {
                        return value;
                    }
                }

                // Look in the item definition table
                if (null != _itemDefinitionTable)
                {
                    value = _itemDefinitionTable.GetEscapedValueIfPresent(specifiedItemType, name);
                }

                return value;
            }

            #endregion

            /// <summary>
            /// Sets the metadata value.
            /// </summary>
            internal void SetValue(string name, string value)
            {
                _addTable[name] = value;
            }
        }
    }
}
