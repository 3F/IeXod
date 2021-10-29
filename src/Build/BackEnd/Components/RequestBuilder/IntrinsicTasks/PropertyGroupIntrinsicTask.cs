// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using net.r_eg.IeXod.Evaluation;
using net.r_eg.IeXod.Execution;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Shared;
using net.r_eg.IeXod.Shared.FileSystem;
using ReservedPropertyNames = net.r_eg.IeXod.Internal.ReservedPropertyNames;
using TargetLoggingContext = net.r_eg.IeXod.BackEnd.Logging.TargetLoggingContext;

namespace net.r_eg.IeXod.BackEnd
{
    /// <summary>
    /// This class represents a PropertyGroup intrinsic task.
    /// </summary>
    internal class PropertyGroupIntrinsicTask : IntrinsicTask
    {
        /// <summary>
        /// The original task instance data.
        /// </summary>
        private ProjectPropertyGroupTaskInstance _taskInstance;

        /// <summary>
        /// Create a new PropertyGroup task.
        /// </summary>
        /// <param name="taskInstance">The task instance data</param>
        /// <param name="loggingContext">The logging context</param>
        /// <param name="projectInstance">The project instance</param>
        /// <param name="logTaskInputs">Flag to determine whether or not to log task inputs.</param>
        public PropertyGroupIntrinsicTask(ProjectPropertyGroupTaskInstance taskInstance, TargetLoggingContext loggingContext, ProjectInstance projectInstance, bool logTaskInputs)
            : base(loggingContext, projectInstance, logTaskInputs)
        {
            _taskInstance = taskInstance;
        }

        /// <summary>
        /// Execute a PropertyGroup element, including each child property
        /// </summary>
        /// <param name="lookup">The lookup use for evaluation and as a destination for these properties.</param>
        internal override void ExecuteTask(Lookup lookup)
        {
            foreach (ProjectPropertyGroupTaskPropertyInstance property in _taskInstance.Properties)
            {
                List<ItemBucket> buckets = null;

                try
                {
                    // Find all the metadata references in order to create buckets
                    List<string> parameterValues = new List<string>();
                    GetBatchableValuesFromProperty(parameterValues, property);
                    buckets = BatchingEngine.PrepareBatchingBuckets(parameterValues, lookup, property.Location);

                    // "Execute" each bucket
                    foreach (ItemBucket bucket in buckets)
                    {
                        bool condition = ConditionEvaluator.EvaluateCondition
                            (
                            property.Condition,
                            ParserOptions.AllowAll,
                            bucket.Expander,
                            ExpanderOptions.ExpandAll,
                            Project.Directory,
                            property.ConditionLocation,
                            LoggingContext.LoggingService,
                            LoggingContext.BuildEventContext,
                            FileSystems.Default);

                        if (condition)
                        {
                            // Check for a reserved name now, so it fails right here instead of later when the property eventually reaches
                            // the outer scope.
                            ProjectErrorUtilities.VerifyThrowInvalidProject
                                (
                                !ReservedPropertyNames.IsReservedProperty(property.Name),
                                property.Location,
                                "CannotModifyReservedProperty",
                                property.Name
                                );

                            string evaluatedValue = bucket.Expander.ExpandIntoStringLeaveEscaped(property.Value, ExpanderOptions.ExpandAll, property.Location);

                            if (LogTaskInputs && !LoggingContext.LoggingService.OnlyLogCriticalEvents)
                            {
                                LoggingContext.LogComment(MessageImportance.Low, "PropertyGroupLogMessage", property.Name, evaluatedValue);
                            }

                            bucket.Lookup.SetProperty(ProjectPropertyInstance.Create(property.Name, evaluatedValue, property.Location, Project.IsImmutable));
                        }
                    }
                }
                finally
                {
                    if (buckets != null)
                    {
                        // Propagate the property changes to the bucket above
                        foreach (ItemBucket bucket in buckets)
                        {
                            bucket.LeaveScope();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds batchable parameters from a property element into the list. If the property element was
        /// a task, these would be its raw parameter values.
        /// </summary>
        /// <param name="parameterValues">The list which will contain the batchable values.</param>
        /// <param name="property">The property from which to take the values.</param>
        private void GetBatchableValuesFromProperty(List<string> parameterValues, ProjectPropertyGroupTaskPropertyInstance property)
        {
            AddIfNotEmptyString(parameterValues, property.Value);
            AddIfNotEmptyString(parameterValues, property.Condition);
        }
    }
}
