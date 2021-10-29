// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.BackEnd.Logging;
using net.r_eg.IeXod.BackEnd;
using net.r_eg.IeXod.Shared;
using TaskItem = net.r_eg.IeXod.Execution.ProjectItemInstance.TaskItem;
using System.Collections.Generic;
using Xunit;

namespace net.r_eg.IeXod.UnitTests.BackEnd
{
    /// <summary>
    /// Each packet is split up into a region, the region contains the tests for 
    /// a given packet type.
    /// </summary>
    public class NodePackets_Tests
    {
        #region LogMessagePacket Tests

        /// <summary>
        /// Verify a null build event throws an exception
        /// </summary>
        [Fact]
        public void LogMessageConstructorNullBuildEvent()
        {
            Assert.Throws<InternalErrorException>(() =>
            {
                LogMessagePacket packet = new LogMessagePacket(null);
            }
           );
        }
        /// <summary>
        /// Verify when creating a LogMessagePacket
        /// that the correct Event Type is set.
        /// </summary>
        [Fact]
        public void VerifyEventType()
        {
            BuildFinishedEventArgs buildFinished = new BuildFinishedEventArgs("Message", "Keyword", true);
            BuildStartedEventArgs buildStarted = new BuildStartedEventArgs("Message", "Help");
            BuildMessageEventArgs lowMessage = new BuildMessageEventArgs("Message", "help", "sender", MessageImportance.Low);
            TaskStartedEventArgs taskStarted = new TaskStartedEventArgs("message", "help", "projectFile", "taskFile", "taskName");
            TaskFinishedEventArgs taskFinished = new TaskFinishedEventArgs("message", "help", "projectFile", "taskFile", "taskName", true);
            TaskCommandLineEventArgs commandLine = new TaskCommandLineEventArgs("commandLine", "taskName", MessageImportance.Low);
            BuildWarningEventArgs warning = new BuildWarningEventArgs("SubCategoryForSchemaValidationErrors", "MSB4000", "file", 1, 2, 3, 4, "message", "help", "sender");
            BuildErrorEventArgs error = new BuildErrorEventArgs("SubCategoryForSchemaValidationErrors", "MSB4000", "file", 1, 2, 3, 4, "message", "help", "sender");
            TargetStartedEventArgs targetStarted = new TargetStartedEventArgs("message", "help", "targetName", "ProjectFile", "targetFile");
            TargetFinishedEventArgs targetFinished = new TargetFinishedEventArgs("message", "help", "targetName", "ProjectFile", "targetFile", true);
            ProjectStartedEventArgs projectStarted = new ProjectStartedEventArgs(-1, "message", "help", "ProjectFile", "targetNames", null, null, null);
            ProjectFinishedEventArgs projectFinished = new ProjectFinishedEventArgs("message", "help", "ProjectFile", true);
            ExternalProjectStartedEventArgs externalStartedEvent = new ExternalProjectStartedEventArgs("message", "help", "senderName", "projectFile", "targetNames");

            VerifyLoggingPacket(buildFinished, LoggingEventType.BuildFinishedEvent);
            VerifyLoggingPacket(buildStarted, LoggingEventType.BuildStartedEvent);
            VerifyLoggingPacket(lowMessage, LoggingEventType.BuildMessageEvent);
            VerifyLoggingPacket(taskStarted, LoggingEventType.TaskStartedEvent);
            VerifyLoggingPacket(taskFinished, LoggingEventType.TaskFinishedEvent);
            VerifyLoggingPacket(commandLine, LoggingEventType.TaskCommandLineEvent);
            VerifyLoggingPacket(warning, LoggingEventType.BuildWarningEvent);
            VerifyLoggingPacket(error, LoggingEventType.BuildErrorEvent);
            VerifyLoggingPacket(targetStarted, LoggingEventType.TargetStartedEvent);
            VerifyLoggingPacket(targetFinished, LoggingEventType.TargetFinishedEvent);
            VerifyLoggingPacket(projectStarted, LoggingEventType.ProjectStartedEvent);
            VerifyLoggingPacket(projectFinished, LoggingEventType.ProjectFinishedEvent);
            VerifyLoggingPacket(externalStartedEvent, LoggingEventType.CustomEvent);
        }

        /// <summary>
        /// Tests serialization of LogMessagePacket with each kind of event type.
        /// </summary>
        [Fact]
        public void TestTranslation()
        {
            TaskItem item = new TaskItem("Hello", "my.proj");
            List<TaskItem> targetOutputs = new List<TaskItem>();
            targetOutputs.Add(item);

            string _initialTargetOutputLogging = Environment.GetEnvironmentVariable("MSBUILDTARGETOUTPUTLOGGING");
            Environment.SetEnvironmentVariable("MSBUILDTARGETOUTPUTLOGGING", "1");
            try {
                BuildEventArgs[] testArgs = new BuildEventArgs[]
                {
                    new BuildFinishedEventArgs("Message", "Keyword", true),
                    new BuildStartedEventArgs("Message", "Help"),
                    new BuildMessageEventArgs("Message", "help", "sender", MessageImportance.Low),
                    new TaskStartedEventArgs("message", "help", "projectFile", "taskFile", "taskName"),
                    new TaskFinishedEventArgs("message", "help", "projectFile", "taskFile", "taskName", true),
                    new TaskCommandLineEventArgs("commandLine", "taskName", MessageImportance.Low),
                    new BuildWarningEventArgs("SubCategoryForSchemaValidationErrors", "MSB4000", "file", 1, 2, 3, 4, "message", "help", "sender"),
                    new BuildErrorEventArgs("SubCategoryForSchemaValidationErrors", "MSB4000", "file", 1, 2, 3, 4, "message", "help", "sender"),
                    new TargetStartedEventArgs("message", "help", "targetName", "ProjectFile", "targetFile"),
                    new TargetFinishedEventArgs("message", "help", "targetName", "ProjectFile", "targetFile", true, targetOutputs),
                    new ProjectStartedEventArgs(-1, "message", "help", "ProjectFile", "targetNames", null, null, null),
                    new ProjectFinishedEventArgs("message", "help", "ProjectFile", true),
                    new ExternalProjectStartedEventArgs("message", "help", "senderName", "projectFile", "targetNames")
                };

                foreach (BuildEventArgs arg in testArgs)
                {
                    LogMessagePacket packet = new LogMessagePacket(new KeyValuePair<int, BuildEventArgs>(0, arg));

                    ((ITranslatable)packet).Translate(TranslationHelpers.GetWriteTranslator());
                    INodePacket tempPacket = LogMessagePacket.FactoryForDeserialization(TranslationHelpers.GetReadTranslator()) as LogMessagePacket;

                    LogMessagePacket deserializedPacket = tempPacket as LogMessagePacket;

                    CompareLogMessagePackets(packet, deserializedPacket);
                }
            } finally {
                Environment.SetEnvironmentVariable("MSBUILDTARGETOUTPUTLOGGING", _initialTargetOutputLogging);
            }
        }

        /// <summary>
        /// Verify the LoggingMessagePacket is properly created from a build event. 
        /// This includes the packet type and the event type depending on which build event arg is passed in.
        /// </summary>
        /// <param name="buildEvent">Build event to put into a packet, and verify after packet creation</param>
        /// <param name="logEventType">What is the expected logging event type</param>
        private static void VerifyLoggingPacket(BuildEventArgs buildEvent, LoggingEventType logEventType)
        {
            LogMessagePacket packet = new LogMessagePacket(new KeyValuePair<int, BuildEventArgs>(0, buildEvent));
            Assert.Equal(logEventType, packet.EventType);
            Assert.Equal(NodePacketType.LogMessage, packet.Type);
            Assert.True(Object.ReferenceEquals(buildEvent, packet.NodeBuildEvent.Value.Value)); // "Expected buildEvent to have the same object reference as packet.BuildEvent"
        }

        /// <summary>
        /// Compares two BuildEventArgs objects for equivalence.
        /// </summary>
        private void CompareNodeBuildEventArgs(KeyValuePair<int, BuildEventArgs> leftTuple, KeyValuePair<int, BuildEventArgs> rightTuple, bool expectInvalidBuildEventContext)
        {
            BuildEventArgs left = leftTuple.Value;
            BuildEventArgs right = rightTuple.Value;

            if (expectInvalidBuildEventContext)
            {
                Assert.Equal(BuildEventContext.Invalid, right.BuildEventContext);
            }
            else
            {
                Assert.Equal(left.BuildEventContext, right.BuildEventContext);
            }

            Assert.Equal(leftTuple.Key, rightTuple.Key);
            Assert.Equal(left.HelpKeyword, right.HelpKeyword);
            Assert.Equal(left.Message, right.Message);
            Assert.Equal(left.SenderName, right.SenderName);
            Assert.Equal(left.ThreadId, right.ThreadId);
            Assert.Equal(left.Timestamp, right.Timestamp);
        }

        /// <summary>
        /// Compares two LogMessagePacket objects for equivalence.
        /// </summary>
        private void CompareLogMessagePackets(LogMessagePacket left, LogMessagePacket right)
        {
            Assert.Equal(left.EventType, right.EventType);
            Assert.Equal(left.NodeBuildEvent.Value.Value.GetType(), right.NodeBuildEvent.Value.Value.GetType());

            CompareNodeBuildEventArgs(left.NodeBuildEvent.Value, right.NodeBuildEvent.Value, left.EventType == LoggingEventType.CustomEvent /* expectInvalidBuildEventContext */);

            switch (left.EventType)
            {
                case LoggingEventType.BuildErrorEvent:
                    BuildErrorEventArgs leftError = left.NodeBuildEvent.Value.Value as BuildErrorEventArgs;
                    BuildErrorEventArgs rightError = right.NodeBuildEvent.Value.Value as BuildErrorEventArgs;
                    Assert.NotNull(leftError);
                    Assert.NotNull(rightError);
                    Assert.Equal(leftError.Code, rightError.Code);
                    Assert.Equal(leftError.ColumnNumber, rightError.ColumnNumber);
                    Assert.Equal(leftError.EndColumnNumber, rightError.EndColumnNumber);
                    Assert.Equal(leftError.EndLineNumber, rightError.EndLineNumber);
                    Assert.Equal(leftError.File, rightError.File);
                    Assert.Equal(leftError.LineNumber, rightError.LineNumber);
                    Assert.Equal(leftError.Message, rightError.Message);
                    Assert.Equal(leftError.Subcategory, rightError.Subcategory);
                    break;

                case LoggingEventType.BuildFinishedEvent:
                    BuildFinishedEventArgs leftFinished = left.NodeBuildEvent.Value.Value as BuildFinishedEventArgs;
                    BuildFinishedEventArgs rightFinished = right.NodeBuildEvent.Value.Value as BuildFinishedEventArgs;
                    Assert.NotNull(leftFinished);
                    Assert.NotNull(rightFinished);
                    Assert.Equal(leftFinished.Succeeded, rightFinished.Succeeded);
                    break;

                case LoggingEventType.BuildMessageEvent:
                    BuildMessageEventArgs leftMessage = left.NodeBuildEvent.Value.Value as BuildMessageEventArgs;
                    BuildMessageEventArgs rightMessage = right.NodeBuildEvent.Value.Value as BuildMessageEventArgs;
                    Assert.NotNull(leftMessage);
                    Assert.NotNull(rightMessage);
                    Assert.Equal(leftMessage.Importance, rightMessage.Importance);
                    break;

                case LoggingEventType.BuildStartedEvent:
                    BuildStartedEventArgs leftBuildStart = left.NodeBuildEvent.Value.Value as BuildStartedEventArgs;
                    BuildStartedEventArgs rightBuildStart = right.NodeBuildEvent.Value.Value as BuildStartedEventArgs;
                    Assert.NotNull(leftBuildStart);
                    Assert.NotNull(rightBuildStart);
                    break;

                case LoggingEventType.BuildWarningEvent:
                    BuildWarningEventArgs leftBuildWarn = left.NodeBuildEvent.Value.Value as BuildWarningEventArgs;
                    BuildWarningEventArgs rightBuildWarn = right.NodeBuildEvent.Value.Value as BuildWarningEventArgs;
                    Assert.NotNull(leftBuildWarn);
                    Assert.NotNull(rightBuildWarn);
                    Assert.Equal(leftBuildWarn.Code, rightBuildWarn.Code);
                    Assert.Equal(leftBuildWarn.ColumnNumber, rightBuildWarn.ColumnNumber);
                    Assert.Equal(leftBuildWarn.EndColumnNumber, rightBuildWarn.EndColumnNumber);
                    Assert.Equal(leftBuildWarn.EndLineNumber, rightBuildWarn.EndLineNumber);
                    Assert.Equal(leftBuildWarn.File, rightBuildWarn.File);
                    Assert.Equal(leftBuildWarn.LineNumber, rightBuildWarn.LineNumber);
                    Assert.Equal(leftBuildWarn.Subcategory, rightBuildWarn.Subcategory);
                    break;

                case LoggingEventType.CustomEvent:
                    ExternalProjectStartedEventArgs leftCustom = left.NodeBuildEvent.Value.Value as ExternalProjectStartedEventArgs;
                    ExternalProjectStartedEventArgs rightCustom = right.NodeBuildEvent.Value.Value as ExternalProjectStartedEventArgs;
                    Assert.NotNull(leftCustom);
                    Assert.NotNull(rightCustom);
                    Assert.Equal(leftCustom.ProjectFile, rightCustom.ProjectFile);
                    Assert.Equal(leftCustom.TargetNames, rightCustom.TargetNames);
                    break;

                case LoggingEventType.ProjectFinishedEvent:
                    ProjectFinishedEventArgs leftProjectFinished = left.NodeBuildEvent.Value.Value as ProjectFinishedEventArgs;
                    ProjectFinishedEventArgs rightProjectFinished = right.NodeBuildEvent.Value.Value as ProjectFinishedEventArgs;
                    Assert.NotNull(leftProjectFinished);
                    Assert.NotNull(rightProjectFinished);
                    Assert.Equal(leftProjectFinished.ProjectFile, rightProjectFinished.ProjectFile);
                    Assert.Equal(leftProjectFinished.Succeeded, rightProjectFinished.Succeeded);
                    break;

                case LoggingEventType.ProjectStartedEvent:
                    ProjectStartedEventArgs leftProjectStarted = left.NodeBuildEvent.Value.Value as ProjectStartedEventArgs;
                    ProjectStartedEventArgs rightProjectStarted = right.NodeBuildEvent.Value.Value as ProjectStartedEventArgs;
                    Assert.NotNull(leftProjectStarted);
                    Assert.NotNull(rightProjectStarted);
                    Assert.Equal(leftProjectStarted.ParentProjectBuildEventContext, rightProjectStarted.ParentProjectBuildEventContext);
                    Assert.Equal(leftProjectStarted.ProjectFile, rightProjectStarted.ProjectFile);
                    Assert.Equal(leftProjectStarted.ProjectId, rightProjectStarted.ProjectId);
                    Assert.Equal(leftProjectStarted.TargetNames, rightProjectStarted.TargetNames);

                    // UNDONE: (Serialization.) We don't actually serialize the items at this time.
                    // Assert.AreEqual(leftProjectStarted.Items, rightProjectStarted.Items);
                    // UNDONE: (Serialization.) We don't actually serialize properties at this time.
                    // Assert.AreEqual(leftProjectStarted.Properties, rightProjectStarted.Properties);
                    break;

                case LoggingEventType.TargetFinishedEvent:
                    TargetFinishedEventArgs leftTargetFinished = left.NodeBuildEvent.Value.Value as TargetFinishedEventArgs;
                    TargetFinishedEventArgs rightTargetFinished = right.NodeBuildEvent.Value.Value as TargetFinishedEventArgs;
                    Assert.NotNull(leftTargetFinished);
                    Assert.NotNull(rightTargetFinished);
                    Assert.Equal(leftTargetFinished.ProjectFile, rightTargetFinished.ProjectFile);
                    Assert.Equal(leftTargetFinished.Succeeded, rightTargetFinished.Succeeded);
                    Assert.Equal(leftTargetFinished.TargetFile, rightTargetFinished.TargetFile);
                    Assert.Equal(leftTargetFinished.TargetName, rightTargetFinished.TargetName);
                    break;

                case LoggingEventType.TargetStartedEvent:
                    TargetStartedEventArgs leftTargetStarted = left.NodeBuildEvent.Value.Value as TargetStartedEventArgs;
                    TargetStartedEventArgs rightTargetStarted = right.NodeBuildEvent.Value.Value as TargetStartedEventArgs;
                    Assert.NotNull(leftTargetStarted);
                    Assert.NotNull(rightTargetStarted);
                    Assert.Equal(leftTargetStarted.ProjectFile, rightTargetStarted.ProjectFile);
                    Assert.Equal(leftTargetStarted.TargetFile, rightTargetStarted.TargetFile);
                    Assert.Equal(leftTargetStarted.TargetName, rightTargetStarted.TargetName);
                    break;

                case LoggingEventType.TaskCommandLineEvent:
                    TaskCommandLineEventArgs leftCommand = left.NodeBuildEvent.Value.Value as TaskCommandLineEventArgs;
                    TaskCommandLineEventArgs rightCommand = right.NodeBuildEvent.Value.Value as TaskCommandLineEventArgs;
                    Assert.NotNull(leftCommand);
                    Assert.NotNull(rightCommand);
                    Assert.Equal(leftCommand.CommandLine, rightCommand.CommandLine);
                    Assert.Equal(leftCommand.Importance, rightCommand.Importance);
                    Assert.Equal(leftCommand.TaskName, rightCommand.TaskName);
                    break;

                case LoggingEventType.TaskFinishedEvent:
                    TaskFinishedEventArgs leftTaskFinished = left.NodeBuildEvent.Value.Value as TaskFinishedEventArgs;
                    TaskFinishedEventArgs rightTaskFinished = right.NodeBuildEvent.Value.Value as TaskFinishedEventArgs;
                    Assert.NotNull(leftTaskFinished);
                    Assert.NotNull(rightTaskFinished);
                    Assert.Equal(leftTaskFinished.ProjectFile, rightTaskFinished.ProjectFile);
                    Assert.Equal(leftTaskFinished.Succeeded, rightTaskFinished.Succeeded);
                    Assert.Equal(leftTaskFinished.TaskFile, rightTaskFinished.TaskFile);
                    Assert.Equal(leftTaskFinished.TaskName, rightTaskFinished.TaskName);
                    break;

                case LoggingEventType.TaskStartedEvent:
                    TaskStartedEventArgs leftTaskStarted = left.NodeBuildEvent.Value.Value as TaskStartedEventArgs;
                    TaskStartedEventArgs rightTaskStarted = right.NodeBuildEvent.Value.Value as TaskStartedEventArgs;
                    Assert.NotNull(leftTaskStarted);
                    Assert.NotNull(rightTaskStarted);
                    Assert.Equal(leftTaskStarted.ProjectFile, rightTaskStarted.ProjectFile);
                    Assert.Equal(leftTaskStarted.TaskFile, rightTaskStarted.TaskFile);
                    Assert.Equal(leftTaskStarted.TaskName, rightTaskStarted.TaskName);
                    break;

                default:
                    Assert.True(false, string.Format("Unexpected logging event type {0}", left.EventType));
                    break;
            }
        }

        #endregion
    }
}
