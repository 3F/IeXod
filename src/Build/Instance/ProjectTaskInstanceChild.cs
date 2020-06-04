// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.BackEnd;
using net.r_eg.IeXod.Shared;

using net.r_eg.IeXod.Construction;

namespace net.r_eg.IeXod.Execution
{
    /// <summary>
    /// Type for TaskOutputItem and TaskOutputProperty
    /// allowing them to be used in a single collection
    /// </summary>
    public abstract class ProjectTaskInstanceChild : ITranslatable
    {
        /// <summary>
        /// Condition on the element
        /// </summary>
        public abstract string Condition
        {
            get;
        }

        /// <summary>
        /// Location of the original element
        /// </summary>
        public abstract ElementLocation Location
        {
            get;
        }

        /// <summary>
        /// Location of the TaskParameter attribute
        /// </summary>
        public abstract ElementLocation TaskParameterLocation
        {
            get;
        }

        /// <summary>
        /// Location of the original condition attribute, if any
        /// </summary>
        public abstract ElementLocation ConditionLocation
        {
            get;
        }

        void ITranslatable.Translate(ITranslator translator)
        {
            // all subclasses should be translateable
            ErrorUtilities.ThrowInternalErrorUnreachable();
        }

        internal static ProjectTaskInstanceChild FactoryForDeserialization(ITranslator translator)
        {
            return translator.FactoryForDeserializingTypeWithName<ProjectTaskInstanceChild>();
        }
    }
}
