// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using net.r_eg.IeXod.BackEnd;
using net.r_eg.IeXod.Shared;

using net.r_eg.IeXod.Construction;

namespace net.r_eg.IeXod.Execution
{
    /// <summary>
    /// Type for ProjectTaskInstance and ProjectPropertyGroupTaskInstance and ProjectItemGroupTaskInstance
    /// allowing them to be used in a single collection of target children
    /// </summary>
    public abstract class ProjectTargetInstanceChild : ITranslatable
    {
        /// <summary>
        /// Condition on the element
        /// </summary>
        public abstract string Condition { get; }

        /// <summary>
        /// Full path to the file in which the originating element was originally 
        /// defined.
        /// If it originated in a project that was not loaded and has never been 
        /// given a path, returns an empty string.
        /// </summary>
        public string FullPath
        {
            get { return Location.File; }
        }

        /// <summary>
        /// Location of the original element
        /// </summary>
        public abstract ElementLocation Location { get; }

        /// <summary>
        /// Location of the original condition attribute
        /// if any
        /// </summary>
        public abstract ElementLocation ConditionLocation { get; }

        void ITranslatable.Translate(ITranslator translator)
        {
            // all subclasses should be translateable
            ErrorUtilities.ThrowInternalErrorUnreachable();
        }

        internal static ProjectTargetInstanceChild FactoryForDeserialization(ITranslator translator)
        {
            return translator.FactoryForDeserializingTypeWithName<ProjectTargetInstanceChild>();
        }
    }
}
