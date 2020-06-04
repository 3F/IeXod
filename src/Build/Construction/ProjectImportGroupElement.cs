// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using net.r_eg.IeXod.Collections;
using net.r_eg.IeXod.ObjectModelRemoting;
using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod.Construction
{
    /// <summary>
    /// ProjectImportGroupElement represents the ImportGroup element in the MSBuild project.
    /// </summary>
    [DebuggerDisplay("#Imports={Count} Condition={Condition} Label={Label}")]
    public class ProjectImportGroupElement : ProjectElementContainer
    {
        #region Constructors
        /// <summary>
        /// External projects support
        /// </summary>
        internal ProjectImportGroupElement(ProjectImportGroupElementLink link)
            : base(link)
        {
        }

        /// <summary>
        /// Initialize a parented ProjectImportGroupElement
        /// </summary>
        internal ProjectImportGroupElement(XmlElementWithLocation xmlElement, ProjectElementContainer parent, ProjectRootElement containingProject)
            : base(xmlElement, parent, containingProject)
        {
            ErrorUtilities.VerifyThrowArgumentNull(parent, nameof(parent));
        }

        /// <summary>
        /// Initialize an unparented ProjectImportGroupElement
        /// </summary>
        private ProjectImportGroupElement(XmlElementWithLocation xmlElement, ProjectRootElement containingProject)
            : base(xmlElement, null, containingProject)
        {
        }

        #endregion // Constructors

        #region Properties

        /// <summary>
        /// Get any contained properties.
        /// </summary>
        public ICollection<ProjectImportElement> Imports => new ReadOnlyCollection<ProjectImportElement>(Children.OfType<ProjectImportElement>());

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Convenience method that picks a location based on a heuristic:
        /// Adds a new import after the last import in this import group.
        /// </summary>
        public ProjectImportElement AddImport(string project)
        {
            ErrorUtilities.VerifyThrowArgumentLength(project, nameof(project));

            ProjectImportElement newImport = ContainingProject.CreateImportElement(project);
            AppendChild(newImport);

            return newImport;
        }

        /// <summary>
        /// Creates an unparented ProjectImportGroupElement, wrapping an unparented XmlElement.
        /// Caller should then ensure the element is added to a parent
        /// </summary>
        internal static ProjectImportGroupElement CreateDisconnected(ProjectRootElement containingProject)
        {
            XmlElementWithLocation element = containingProject.CreateElement(XMakeElements.importGroup);

            return new ProjectImportGroupElement(element, containingProject);
        }

        /// <summary>
        /// Overridden to verify that the potential parent and siblings
        /// are acceptable. Throws InvalidOperationException if they are not.
        /// </summary>
        internal override void VerifyThrowInvalidOperationAcceptableLocation(ProjectElementContainer parent, ProjectElement previousSibling, ProjectElement nextSibling)
        {
            ErrorUtilities.VerifyThrowInvalidOperation(parent is ProjectRootElement, "OM_CannotAcceptParent");
        }

        /// <inheritdoc />
        protected override ProjectElement CreateNewInstance(ProjectRootElement owner)
        {
            return owner.CreateImportGroupElement();
        }

        #endregion // Methods
    }
}
