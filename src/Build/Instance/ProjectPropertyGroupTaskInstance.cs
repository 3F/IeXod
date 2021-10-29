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
    /// Wraps an unevaluated propertygroup under a target.
    /// Immutable.
    /// </summary>
    [DebuggerDisplay("Condition={_condition}")]
    public class ProjectPropertyGroupTaskInstance : ProjectTargetInstanceChild, ITranslatable
    {
        /// <summary>
        /// Condition, if any
        /// </summary>
        private string _condition;

        /// <summary>
        /// Child properties.
        /// Not ProjectPropertyInstances, as these are evaluated during the build.
        /// </summary>
        private List<ProjectPropertyGroupTaskPropertyInstance> _properties;

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
        /// Assumes ProjectPropertyGroupTaskPropertyInstance is an immutable type.
        /// </summary>
        internal ProjectPropertyGroupTaskInstance
            (
            string condition,
            ElementLocation location,
            ElementLocation conditionLocation,
            List<ProjectPropertyGroupTaskPropertyInstance> properties
            )
        {
            ErrorUtilities.VerifyThrowInternalNull(condition, "condition");
            ErrorUtilities.VerifyThrowInternalNull(location, "location");
            ErrorUtilities.VerifyThrowInternalNull(properties, "properties");

            _condition = condition;
            _location = location;
            _conditionLocation = conditionLocation;
            _properties = properties;
        }

        private ProjectPropertyGroupTaskInstance()
        {
        }

        /// <summary>
        /// Cloning constructor
        /// </summary>
        private ProjectPropertyGroupTaskInstance(ProjectPropertyGroupTaskInstance that)
        {
            // All members are immutable
            _condition = that._condition;
            _properties = that._properties;
        }

        /// <summary>
        /// Condition, if any.
        /// May be empty string.
        /// </summary>
        public override string Condition
        {
            [DebuggerStepThrough]
            get
            { return _condition; }
        }

        /// <summary>
        /// Child properties
        /// </summary>
        public ICollection<ProjectPropertyGroupTaskPropertyInstance> Properties
        {
            [DebuggerStepThrough]
            get
            {
                return (_properties == null) ?
                    (ICollection<ProjectPropertyGroupTaskPropertyInstance>)ReadOnlyEmptyCollection<ProjectPropertyGroupTaskPropertyInstance>.Instance :
                    new ReadOnlyCollection<ProjectPropertyGroupTaskPropertyInstance>(_properties);
            }
        }

        /// <summary>
        /// Location of the element itself
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
        /// Deep clone
        /// </summary>
        internal ProjectPropertyGroupTaskInstance DeepClone()
        {
            return new ProjectPropertyGroupTaskInstance(this);
        }
        void ITranslatable.Translate(ITranslator translator)
        {
            if (translator.Mode == TranslationDirection.WriteToStream)
            {
                var typeName = this.GetType().FullName;
                translator.Translate(ref typeName);
            }

            translator.Translate(ref _condition);
            translator.Translate(ref _properties, ProjectPropertyGroupTaskPropertyInstance.FactoryForDeserialization);
            translator.Translate(ref _location, ElementLocation.FactoryForDeserialization);
            translator.Translate(ref _conditionLocation, ElementLocation.FactoryForDeserialization);
        }
    }
}
