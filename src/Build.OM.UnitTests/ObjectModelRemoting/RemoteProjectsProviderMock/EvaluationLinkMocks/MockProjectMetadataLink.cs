// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace net.r_eg.IeXod.UnitTests.OM.ObjectModelRemoting
{
    using net.r_eg.IeXod.Construction;
    using net.r_eg.IeXod.Evaluation;
    using net.r_eg.IeXod.ObjectModelRemoting;

    internal class MockProjectMetadataLinkRemoter : MockLinkRemoter<ProjectMetadata>
    {
        public override ProjectMetadata CreateLinkedObject(IImportHolder holder)
        {
            var link = new MockProjectMetadataLink(this, holder);
            return holder.Linker.LinkFactory.Create(link);
        }


        ///  ProjectMetadataLink remoting
        public object Parent
        {
            get
            {
                var parent = ProjectMetadataLink.GetParent(this.Source);
                if (parent == null) return null;
                var itemParent = parent as ProjectItem;
                if (itemParent != null)
                {
                    return this.OwningCollection.Export<ProjectItem, MockProjectItemLinkRemoter>(itemParent);
                }

                return this.OwningCollection.Export<ProjectItemDefinition, MockProjectItemDefinitionLinkRemoter>((ProjectItemDefinition)parent);
            }
        }

        public MockProjectMetadataElementLinkRemoter Xml => (MockProjectMetadataElementLinkRemoter)this.OwningCollection.ExportElement(this.Source.Xml);
        public string EvaluatedValueEscaped => ProjectMetadataLink.GetEvaluatedValueEscaped(this.Source);
        public MockProjectMetadataLinkRemoter Predecessor => this.OwningCollection.Export<ProjectMetadata, MockProjectMetadataLinkRemoter>(this.Source.Predecessor);
    }

    internal class MockProjectMetadataLink : ProjectMetadataLink, ILinkMock
    {
        public MockProjectMetadataLink(MockProjectMetadataLinkRemoter proxy, IImportHolder holder)
        {
            this.Holder = holder;
            this.Proxy = proxy;
        }

        public IImportHolder Holder { get; }
        public ProjectCollectionLinker Linker => this.Holder.Linker;
        public MockProjectMetadataLinkRemoter Proxy { get; }
        object ILinkMock.Remoter => this.Proxy;

        // ProjectMetadataLink
        public override object Parent
        {
            get
            {
                var parentRemoter = this.Proxy.Parent;
                if (parentRemoter == null) return null;
                var itemParent = parentRemoter as MockProjectItemLinkRemoter;
                if (itemParent != null)
                {
                    return this.Linker.Import<ProjectItem, MockProjectItemLinkRemoter>(itemParent);
                }

                return this.Linker.Import<ProjectItemDefinition, MockProjectItemDefinitionLinkRemoter>((MockProjectItemDefinitionLinkRemoter)parentRemoter);
            }
        }

        public override ProjectMetadataElement Xml => (ProjectMetadataElement)this.Proxy.Xml.Import(this.Linker);
        public override string EvaluatedValueEscaped => this.Proxy.EvaluatedValueEscaped;
        public override ProjectMetadata Predecessor => this.Linker.Import<ProjectMetadata, MockProjectMetadataLinkRemoter>(this.Proxy.Predecessor);
    }
}
