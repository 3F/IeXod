﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.BackEnd;
using net.r_eg.IeXod.Shared;
using net.r_eg.IeXod.Construction;

namespace net.r_eg.IeXod.Execution
{
    /// <summary>
    /// Represents an output property element beneath a task element
    /// </summary>
    /// <remarks>
    /// Immutable.
    /// </remarks>
    public sealed class ProjectTaskOutputPropertyInstance : ProjectTaskInstanceChild, ITranslatable
    {
        /// <summary>
        /// Name of the property to put the output in
        /// </summary>
        private string _propertyName;

        /// <summary>
        /// Property on the task class to retrieve the output from
        /// </summary>
        private string _taskParameter;

        /// <summary>
        /// Condition on the output element
        /// </summary>
        private string _condition;

        /// <summary>
        /// Location of the original element
        /// </summary>
        private ElementLocation _location;

        /// <summary>
        /// Location of the original property name attribute
        /// </summary>
        private ElementLocation _propertyNameLocation;

        /// <summary>
        /// Location of the original task parameter attribute
        /// </summary>
        private ElementLocation _taskParameterLocation;

        /// <summary>
        /// Location of the original condition attribute
        /// </summary>
        private ElementLocation _conditionLocation;

        /// <summary>
        /// Constructor called by Evaluator
        /// </summary>
        internal ProjectTaskOutputPropertyInstance(string propertyName, string taskParameter, string condition, ElementLocation location, ElementLocation propertyNameLocation, ElementLocation taskParameterLocation, ElementLocation conditionLocation)
        {
            ErrorUtilities.VerifyThrowInternalLength(propertyName, "propertyName");
            ErrorUtilities.VerifyThrowInternalLength(taskParameter, "taskParameter");
            ErrorUtilities.VerifyThrowInternalNull(location, "location");
            ErrorUtilities.VerifyThrowInternalNull(propertyNameLocation, "propertyNameLocation");
            ErrorUtilities.VerifyThrowInternalNull(taskParameterLocation, "taskParameterLocation");

            _propertyName = propertyName;
            _taskParameter = taskParameter;
            _condition = condition;
            _location = location;
            _propertyNameLocation = propertyNameLocation;
            _taskParameterLocation = taskParameterLocation;
            _conditionLocation = conditionLocation;
        }

        private ProjectTaskOutputPropertyInstance()
        {
        }

        /// <summary>
        /// Name of the property to put the output in
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
        }

        /// <summary>
        /// Property on the task class to retrieve the output from
        /// </summary>
        public string TaskParameter
        {
            get { return _taskParameter; }
        }

        /// <summary>
        /// Condition on the output element.
        /// If there is no condition, returns empty string.
        /// </summary>
        public override string Condition
        {
            get { return _condition; }
        }

        /// <summary>
        /// Location of the original PropertyName attribute
        /// </summary>
        public ElementLocation PropertyNameLocation
        {
            get { return _propertyNameLocation; }
        }

        /// <summary>
        /// Location of the original element
        /// </summary>
        public override ElementLocation Location
        {
            get { return _location; }
        }

        /// <summary>
        /// Location of the condition, if any
        /// </summary>
        public override ElementLocation ConditionLocation
        {
            get { return _conditionLocation; }
        }

        /// <summary>
        /// Location of the TaskParameter attribute
        /// </summary>
        public override ElementLocation TaskParameterLocation
        {
            get { return _taskParameterLocation; }
        }

        void ITranslatable.Translate(ITranslator translator)
        {
            if (translator.Mode == TranslationDirection.WriteToStream)
            {
                var typeName = this.GetType().FullName;
                translator.Translate(ref typeName);
            }

            translator.Translate(ref _propertyName);
            translator.Translate(ref _taskParameter);
            translator.Translate(ref _condition);
            translator.Translate(ref _location, ElementLocation.FactoryForDeserialization);
            translator.Translate(ref _conditionLocation, ElementLocation.FactoryForDeserialization);
            translator.Translate(ref _propertyNameLocation, ElementLocation.FactoryForDeserialization);
            translator.Translate(ref _taskParameterLocation, ElementLocation.FactoryForDeserialization);
        }
    }
}
