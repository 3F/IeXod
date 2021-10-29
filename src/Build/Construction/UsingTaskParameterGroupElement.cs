﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using net.r_eg.IeXod.Collections;
using net.r_eg.IeXod.ObjectModelRemoting;
using net.r_eg.IeXod.Shared;

using ProjectXmlUtilities = net.r_eg.IeXod.Internal.ProjectXmlUtilities;

namespace net.r_eg.IeXod.Construction
{
    /// <summary>
    /// UsingTaskParameterGroupElement represents a ParameterGroup under the using task.
    /// </summary>
    [DebuggerDisplay("#Parameters={Count}")]
    public class UsingTaskParameterGroupElement : ProjectElementContainer
    {
        /// <summary>
        /// External projects support
        /// </summary>
        internal UsingTaskParameterGroupElement(UsingTaskParameterGroupElementLink link)
            : base(link)
        {
        }

        /// <summary>
        /// Initialize a parented UsingTaskParameterGroupElement
        /// </summary>
        internal UsingTaskParameterGroupElement(XmlElementWithLocation xmlElement, ProjectElementContainer parent, ProjectRootElement containingProject)
            : base(xmlElement, parent, containingProject)
        {
            ErrorUtilities.VerifyThrowArgumentNull(parent, nameof(parent));
            VerifyCorrectParent(parent);
        }

        /// <summary>
        /// Initialize an unparented UsingTaskParameterGroupElement
        /// </summary>
        private UsingTaskParameterGroupElement(XmlElementWithLocation xmlElement, ProjectRootElement containingProject)
            : base(xmlElement, null, containingProject)
        {
        }

        /// <summary>
        /// Condition should never be set, but the getter returns null instead of throwing 
        /// because a nonexistent condition is implicitly true
        /// </summary>
        public override string Condition
        {
            get => null;
            set => ErrorUtilities.ThrowInvalidOperation("OM_CannotGetSetCondition");
        }

        /// <summary>
        /// Get any contained parameters.
        /// </summary>
        public ICollection<ProjectUsingTaskParameterElement> Parameters => new ReadOnlyCollection<ProjectUsingTaskParameterElement>(Children.OfType<ProjectUsingTaskParameterElement>());

        /// <summary>
        /// This does not allow conditions, so it should not be called.
        /// </summary>
        public override ElementLocation ConditionLocation
        {
            get
            {
                ErrorUtilities.ThrowInternalError("Should not evaluate this");
                return null;
            }
        }

        #region Add parameters
        /// <summary>
        /// Convenience method that picks a location based on a heuristic:
        /// </summary>
        public ProjectUsingTaskParameterElement AddParameter(string name, string output, string required, string parameterType)
        {
            ErrorUtilities.VerifyThrowArgumentLength(name, nameof(name));

            ProjectUsingTaskParameterElement newParameter = ContainingProject.CreateUsingTaskParameterElement(name, output, required, parameterType);
            AppendChild(newParameter);

            return newParameter;
        }

        /// <summary>
        /// Convenience method that picks a location based on a heuristic:
        /// </summary>
        public ProjectUsingTaskParameterElement AddParameter(string name)
        {
            return AddParameter(name, String.Empty, String.Empty, String.Empty);
        }
        #endregion

        /// <summary>
        /// Creates an unparented UsingTaskParameterGroupElement, wrapping an unparented XmlElement.
        /// Caller should then ensure the element is added to a parent
        /// </summary>
        internal static UsingTaskParameterGroupElement CreateDisconnected(ProjectRootElement containingProject)
        {
            XmlElementWithLocation element = containingProject.CreateElement(XMakeElements.usingTaskParameterGroup);

            return new UsingTaskParameterGroupElement(element, containingProject);
        }

        /// <summary>
        /// Overridden to verify that the potential parent and siblings
        /// are acceptable. Throws InvalidOperationException if they are not.
        /// </summary>
        internal override void VerifyThrowInvalidOperationAcceptableLocation(ProjectElementContainer parent, ProjectElement previousSibling, ProjectElement nextSibling)
        {
            VerifyCorrectParent(parent);
        }

        /// <inheritdoc />
        protected override ProjectElement CreateNewInstance(ProjectRootElement owner)
        {
            return owner.CreateUsingTaskParameterGroupElement();
        }

        /// <summary>
        /// Verify the parent is a usingTaskElement and that the taskFactory attribute is set
        /// </summary>
        private static void VerifyCorrectParent(ProjectElementContainer parent)
        {
            ProjectUsingTaskElement parentUsingTask = parent as ProjectUsingTaskElement;
            ErrorUtilities.VerifyThrowInvalidOperation(parentUsingTask != null, "OM_CannotAcceptParent");

            // Now since there is not goign to be a TaskElement on the using task we need to validate and make sure there is a TaskFactory attribute on the parent element and 
            // that it is not empty
            if (parentUsingTask.TaskFactory.Length == 0)
            {
                ErrorUtilities.VerifyThrow(parentUsingTask.Link == null, "TaskFactory");
                ProjectXmlUtilities.VerifyThrowProjectRequiredAttribute(parent.XmlElement, "TaskFactory");
            }

            // UNDONE: Do check to make sure the parameter group is the first child
        }
    }
}
