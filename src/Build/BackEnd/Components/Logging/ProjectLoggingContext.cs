﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Execution;
using System.Collections;
using net.r_eg.IeXod.Shared;
using TaskItem = net.r_eg.IeXod.Execution.ProjectItemInstance.TaskItem;
using net.r_eg.IeXod.Collections;

namespace net.r_eg.IeXod.BackEnd.Logging
{
    /// <summary>
    /// A logging context for a project.
    /// </summary>
    internal class ProjectLoggingContext : BuildLoggingContext
    {
        /// <summary>
        /// The project's full path
        /// </summary>
        private string _projectFullPath;

        /// <summary>
        /// The parent node logging context this context was derived from.
        /// </summary>
        private NodeLoggingContext _nodeLoggingContext;

        /// <summary>
        /// Constructs a project logging context.
        /// </summary>
        internal ProjectLoggingContext(NodeLoggingContext nodeLoggingContext, BuildRequestEntry requestEntry, BuildEventContext parentBuildEventContext)
            : this
            (
            nodeLoggingContext,
            requestEntry.Request.SubmissionId,
            requestEntry.Request.ConfigurationId,
            requestEntry.RequestConfiguration.ProjectFullPath,
            requestEntry.Request.Targets,
            requestEntry.RequestConfiguration.ToolsVersion,
            requestEntry.RequestConfiguration.Project.PropertiesToBuildWith,
            requestEntry.RequestConfiguration.Project.ItemsToBuildWith,
            parentBuildEventContext
            )
        {
        }

        /// <summary>
        /// Constructs a project logging context.
        /// </summary>
        internal ProjectLoggingContext(NodeLoggingContext nodeLoggingContext, BuildRequest request, string projectFullPath, string toolsVersion, BuildEventContext parentBuildEventContext)
            : this
            (
            nodeLoggingContext,
            request.SubmissionId,
            request.ConfigurationId,
            projectFullPath,
            request.Targets,
            toolsVersion,
            null,
            null,
            parentBuildEventContext
            )
        {
        }

        /// <summary>
        /// Constructs a project logging contexts.
        /// </summary>
        private ProjectLoggingContext(NodeLoggingContext nodeLoggingContext, int submissionId, int configurationId, string projectFullPath, List<string> targets, string toolsVersion, PropertyDictionary<ProjectPropertyInstance> projectProperties, ItemDictionary<ProjectItemInstance> projectItems, BuildEventContext parentBuildEventContext)
            : base(nodeLoggingContext)
        {
            _nodeLoggingContext = nodeLoggingContext;
            _projectFullPath = projectFullPath;

            ProjectPropertyInstanceEnumeratorProxy properties = null;
            ProjectItemInstanceEnumeratorProxy items = null;

            IEnumerable<ProjectPropertyInstance> projectPropertiesEnumerator = projectProperties == null ? Array.Empty<ProjectPropertyInstance>() : null;
            IEnumerable<ProjectItemInstance> projectItemsEnumerator = projectItems == null ? Array.Empty<ProjectItemInstance>() : null;

            string[] propertiesToSerialize = LoggingService.PropertiesToSerialize;

            // If we are only logging critical events lets not pass back the items or properties
            if (!LoggingService.OnlyLogCriticalEvents && (!LoggingService.RunningOnRemoteNode || LoggingService.SerializeAllProperties))
            {
                if (projectProperties != null)
                {
                    projectPropertiesEnumerator = projectProperties.GetCopyOnReadEnumerable();
                }

                if (projectItems != null)
                {
                    projectItemsEnumerator = projectItems.GetCopyOnReadEnumerable();
                }

                properties = new ProjectPropertyInstanceEnumeratorProxy(projectPropertiesEnumerator);
                items = new ProjectItemInstanceEnumeratorProxy(projectItemsEnumerator);
            }

            if (projectProperties != null && propertiesToSerialize != null && propertiesToSerialize.Length > 0 && !LoggingService.SerializeAllProperties)
            {
                PropertyDictionary<ProjectPropertyInstance> projectPropertiesToSerialize = new PropertyDictionary<ProjectPropertyInstance>();
                foreach (string propertyToGet in propertiesToSerialize)
                {
                    ProjectPropertyInstance instance = projectProperties[propertyToGet];
                    {
                        if (instance != null)
                        {
                            projectPropertiesToSerialize.Set(instance);
                        }
                    }
                }

                properties = new ProjectPropertyInstanceEnumeratorProxy(projectPropertiesToSerialize);
            }

            this.BuildEventContext = LoggingService.LogProjectStarted
                (
                nodeLoggingContext.BuildEventContext,
                submissionId,
                configurationId,
                parentBuildEventContext,
                projectFullPath,
                String.Join(";", targets),
                properties,
                items
                );
            LoggingService.LogComment(this.BuildEventContext, MessageImportance.Low, "ToolsVersionInEffectForBuild", toolsVersion);

            this.IsValid = true;
        }

        /// <summary>
        /// Retrieves the node logging context.
        /// </summary>
        internal NodeLoggingContext NodeLoggingContext
        {
            get
            {
                return _nodeLoggingContext;
            }
        }

        /// <summary>
        /// Log that the project has finished
        /// </summary>
        /// <param name="success">Did the build succeede or not</param>
        internal void LogProjectFinished(bool success)
        {
            ErrorUtilities.VerifyThrow(this.IsValid, "invalid");
            LoggingService.LogProjectFinished(BuildEventContext, _projectFullPath, success);
            this.IsValid = false;
        }

        /// <summary>
        /// Log that a target has started
        /// </summary>
        internal TargetLoggingContext LogTargetBatchStarted(string projectFullPath, ProjectTargetInstance target, string parentTargetName, TargetBuiltReason buildReason)
        {
            ErrorUtilities.VerifyThrow(this.IsValid, "invalid");
            return new TargetLoggingContext(this, projectFullPath, target, parentTargetName, buildReason);
        }

        /// <summary>
        /// An enumerable wrapper for items that clones items as they are requested,
        /// so that writes have no effect on the items.
        /// </summary>
        /// <remarks>
        /// This class is designed to be passed to loggers.
        /// The expense of copying items is only incurred if and when 
        /// a logger chooses to enumerate over it.
        /// The type of the items enumerated over is imposed by backwards compatibility for ProjectStartedEvent.
        /// </remarks>
        private class ProjectItemInstanceEnumeratorProxy : IEnumerable<DictionaryEntry>
        {
            /// <summary>
            /// Enumerable that this proxies
            /// </summary>
            private IEnumerable<ProjectItemInstance> _backingItems;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="backingItems">Enumerator this class should proxy</param>
            internal ProjectItemInstanceEnumeratorProxy(IEnumerable<ProjectItemInstance> backingItems)
            {
                _backingItems = backingItems;
            }

            /// <summary>
            /// Returns an enumerator that provides copies of the items
            /// in the backing store.
            /// Each dictionary entry has key of the item type and value of an ITaskItem.
            /// Type of the enumerator is imposed by backwards compatibility for ProjectStartedEvent.
            /// </summary>
            public IEnumerator<DictionaryEntry> GetEnumerator()
            {
                foreach (ProjectItemInstance item in _backingItems)
                {
                    yield return new DictionaryEntry(item.ItemType, new TaskItem(item));
                }
            }

            /// <summary>
            /// Returns an enumerator that provides copies of the items
            /// in the backing store.
            /// </summary>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return (IEnumerator)GetEnumerator();
            }
        }

        /// <summary>
        /// An enumerable wrapper for properties that clones properties as they are requested,
        /// so that writes have no effect on the properties.
        /// </summary>
        /// <remarks>
        /// This class is designed to be passed to loggers.
        /// The expense of copying items is only incurred if and when 
        /// a logger chooses to enumerate over it.
        /// The type of the items enumerated over is imposed by backwards compatibility for ProjectStartedEvent.
        /// </remarks>
        private class ProjectPropertyInstanceEnumeratorProxy : IEnumerable<DictionaryEntry>
        {
            /// <summary>
            /// Enumerable that this proxies
            /// </summary>
            private IEnumerable<ProjectPropertyInstance> _backingProperties;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="backingProperties">Enumerator this class should proxy</param>
            internal ProjectPropertyInstanceEnumeratorProxy(IEnumerable<ProjectPropertyInstance> backingProperties)
            {
                _backingProperties = backingProperties;
            }

            /// <summary>
            /// Returns an enumerator that provides copies of the properties
            /// in the backing store.
            /// Each DictionaryEntry has key of the property name and value of the property value.
            /// Type of the enumerator is imposed by backwards compatibility for ProjectStartedEvent.
            /// </summary>
            public IEnumerator<DictionaryEntry> GetEnumerator()
            {
                foreach (ProjectPropertyInstance property in _backingProperties)
                {
                    yield return new DictionaryEntry(property.Name, property.EvaluatedValue);
                }
            }

            /// <summary>
            /// Returns an enumerator that provides copies of the properties
            /// in the backing store.
            /// </summary>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return (IEnumerator)GetEnumerator();
            }
        }
    }
}
