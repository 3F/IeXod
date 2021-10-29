﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using net.r_eg.IeXod.BackEnd;
using net.r_eg.IeXod.Collections;
using net.r_eg.IeXod.Shared;
using net.r_eg.IeXod.Evaluation;

using net.r_eg.IeXod.Construction;

namespace net.r_eg.IeXod.Execution
{
    /// <summary>
    /// Wraps an unevaluated metadatum under an item in an itemgroup in a target
    /// Immutable.
    /// </summary>
    [DebuggerDisplay("{_name} Value={_value} Condition={_condition}")]
    public class ProjectItemGroupTaskMetadataInstance : ITranslatable
    {
        /// <summary>
        /// Name of the metadatum
        /// </summary>
        private string _name;

        /// <summary>
        /// Unevaluated value
        /// </summary>
        private string _value;

        /// <summary>
        /// Unevaluated condition
        /// </summary>
        private string _condition;

        /// <summary>
        /// Location of this element
        /// </summary>
        private ElementLocation _location;

        /// <summary>
        /// Location of the condition, if any
        /// </summary>
        private ElementLocation _conditionLocation;

        /// <summary>
        /// Constructor called by the Evaluator.
        /// </summary>
        internal ProjectItemGroupTaskMetadataInstance(string name, string value, string condition, ElementLocation location, ElementLocation conditionLocation)
        {
            ErrorUtilities.VerifyThrowInternalNull(name, "name");
            ErrorUtilities.VerifyThrowInternalNull(value, "value");
            ErrorUtilities.VerifyThrowInternalNull(condition, "condition");
            ErrorUtilities.VerifyThrowInternalNull(location, "location");

            _name = name;
            _value = value;
            _condition = condition;
            _location = location;
            _conditionLocation = conditionLocation;
        }

        /// <summary>
        /// Cloning constructor
        /// </summary>
        private ProjectItemGroupTaskMetadataInstance(ProjectItemGroupTaskMetadataInstance that)
        {
            // All fields are immutable
            _name = that._name;
            _value = that._value;
            _condition = that._condition;
        }

        private ProjectItemGroupTaskMetadataInstance()
        {
        }

        /// <summary>
        /// Name of the metadatum
        /// </summary>
        public string Name
        {
            [DebuggerStepThrough]
            get
            { return _name; }
        }

        /// <summary>
        /// Unevaluated value
        /// </summary>
        public string Value
        {
            [DebuggerStepThrough]
            get
            { return _value; }
        }

        /// <summary>
        /// Unevaluated condition value
        /// </summary>
        public string Condition
        {
            [DebuggerStepThrough]
            get
            { return _condition; }
        }

        /// <summary>
        /// Location of the element
        /// </summary>
        public ElementLocation Location
        {
            [DebuggerStepThrough]
            get
            { return _location; }
        }

        /// <summary>
        /// Location of the condition attribute if any
        /// </summary>
        public ElementLocation ConditionLocation
        {
            [DebuggerStepThrough]
            get
            { return _conditionLocation; }
        }

        /// <summary>
        /// Deep clone
        /// </summary>
        internal ProjectItemGroupTaskMetadataInstance DeepClone()
        {
            return new ProjectItemGroupTaskMetadataInstance(this);
        }

        void ITranslatable.Translate(ITranslator translator)
        {
            translator.Translate(ref _name);
            translator.Translate(ref _value);
            translator.Translate(ref _condition);
            translator.Translate(ref _location, ElementLocation.FactoryForDeserialization);
            translator.Translate(ref _conditionLocation, ElementLocation.FactoryForDeserialization);
        }

        internal static ProjectItemGroupTaskMetadataInstance FactoryForDeserialization(ITranslator translator)
        {
            var instance = new ProjectItemGroupTaskMetadataInstance();
            ((ITranslatable) instance).Translate(translator);

            return instance;
        }
    }
}
