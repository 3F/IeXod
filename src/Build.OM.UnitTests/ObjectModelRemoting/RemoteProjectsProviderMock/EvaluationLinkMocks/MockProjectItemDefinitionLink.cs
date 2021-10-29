// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace net.r_eg.IeXod.UnitTests.OM.ObjectModelRemoting
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using net.r_eg.IeXod.Construction;
    using net.r_eg.IeXod.Evaluation;
    using net.r_eg.IeXod.Evaluation.Context;
    using net.r_eg.IeXod.Execution;
    using net.r_eg.IeXod.ObjectModelRemoting;
    using net.r_eg.IeXod.Framework;
    using net.r_eg.IeXod.Logging;

    internal class MockProjectItemDefinitionLinkRemoter : MockLinkRemoter<ProjectItemDefinition>
    {
        public override ProjectItemDefinition CreateLinkedObject(IImportHolder holder)
        {
            var link = new MockProjectItemDefinitionLink(this, holder);
            return holder.Linker.LinkFactory.Create(link);
        }


        ///  ProjectItemDefinitionLink remoting
        public MockProjectLinkRemoter Project => this.OwningCollection.Export<Project, MockProjectLinkRemoter>(this.Source.Project);
        public string ItemType => this.Source.ItemType;
        public ICollection<MockProjectMetadataLinkRemoter> Metadata => this.OwningCollection.ExportCollection<ProjectMetadata, MockProjectMetadataLinkRemoter>(this.Source.Metadata);
        public MockProjectMetadataLinkRemoter GetMetadata(string name)
            => this.OwningCollection.Export<ProjectMetadata, MockProjectMetadataLinkRemoter>(this.Source.GetMetadata(name));
        public string GetMetadataValue(string name) => this.Source.GetMetadataValue(name);
        public MockProjectMetadataLinkRemoter SetMetadataValue(string name, string unevaluatedValue)
            => this.OwningCollection.Export<ProjectMetadata, MockProjectMetadataLinkRemoter>(this.Source.SetMetadataValue(name, unevaluatedValue));
    }

    internal class MockProjectItemDefinitionLink : ProjectItemDefinitionLink, ILinkMock
    {
        public MockProjectItemDefinitionLink(MockProjectItemDefinitionLinkRemoter proxy, IImportHolder holder)
        {
            this.Holder = holder;
            this.Proxy = proxy;
        }

        public IImportHolder Holder { get; }
        public ProjectCollectionLinker Linker => this.Holder.Linker;
        public MockProjectItemDefinitionLinkRemoter Proxy { get; }
        object ILinkMock.Remoter => this.Proxy;

        // ProjectItemDefinitionLink
        public override Project Project => this.Linker.Import<Project, MockProjectLinkRemoter>(this.Proxy.Project);
        public override string ItemType => this.Proxy.ItemType;
        public override ICollection<ProjectMetadata> Metadata
            => this.Linker.ImportCollection<ProjectMetadata, MockProjectMetadataLinkRemoter>(this.Proxy.Metadata);
        public override ProjectMetadata GetMetadata(string name)
            => this.Linker.Import<ProjectMetadata, MockProjectMetadataLinkRemoter>(this.Proxy.GetMetadata(name));
        public override string GetMetadataValue(string name) => this.Proxy.GetMetadataValue(name);
        public override ProjectMetadata SetMetadataValue(string name, string unevaluatedValue)
            => this.Linker.Import<ProjectMetadata, MockProjectMetadataLinkRemoter>(this.Proxy.SetMetadataValue(name, unevaluatedValue));
    }
}
