// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
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

    internal class MockProjectPropertyLinkRemoter : MockLinkRemoter<ProjectProperty>
    {
        public override ProjectProperty CreateLinkedObject(IImportHolder holder)
        {
            var link = new MockProjectPropertyLink(this, holder);
            return holder.Linker.LinkFactory.Create(link);
        }


        ///  ProjectPropertyLink remoting
        public MockProjectLinkRemoter Project => this.OwningCollection.Export<Project, MockProjectLinkRemoter>(this.Source.Project);
        public MockProjectPropertyElementLinkRemoter Xml => (MockProjectPropertyElementLinkRemoter)this.ExportElement(this.Source.Xml);
        public string Name => this.Source.Name;
        public string EvaluatedIncludeEscaped => ProjectPropertyLink.GetEvaluatedValueEscaped(this.Source);
        public string UnevaluatedValue { get => this.Source.UnevaluatedValue; set=> this.Source.UnevaluatedValue = value; }
        public bool IsEnvironmentProperty => this.Source.IsEnvironmentProperty;
        public bool IsGlobalProperty => this.Source.IsGlobalProperty;
        public bool IsReservedProperty => this.Source.IsReservedProperty;
        public MockProjectPropertyLinkRemoter Predecessor => this.OwningCollection.Export<ProjectProperty, MockProjectPropertyLinkRemoter>(this.Source.Predecessor);
        public bool IsImported => this.Source.IsImported;
    }

    internal class MockProjectPropertyLink : ProjectPropertyLink, ILinkMock
    {
        public MockProjectPropertyLink(MockProjectPropertyLinkRemoter proxy, IImportHolder holder)
        {
            this.Holder = holder;
            this.Proxy = proxy;
        }

        public IImportHolder Holder { get; }
        public ProjectCollectionLinker Linker => this.Holder.Linker;
        public MockProjectPropertyLinkRemoter Proxy { get; }
        object ILinkMock.Remoter => this.Proxy;

        // ProjectPropertyLink
        public override Project Project => this.Linker.Import<Project, MockProjectLinkRemoter>(this.Proxy.Project);
        public override ProjectPropertyElement Xml => (ProjectPropertyElement)this.Proxy.Xml.Import(this.Linker);
        public override string Name => this.Proxy.Name;
        public override string EvaluatedIncludeEscaped => this.Proxy.EvaluatedIncludeEscaped;
        public override string UnevaluatedValue { get => this.Proxy.UnevaluatedValue; set => this.Proxy.UnevaluatedValue = value; }
        public override bool IsEnvironmentProperty => this.Proxy.IsEnvironmentProperty;
        public override bool IsGlobalProperty => this.Proxy.IsGlobalProperty;
        public override bool IsReservedProperty => this.Proxy.IsReservedProperty;
        public override ProjectProperty Predecessor => this.Linker.Import<ProjectProperty, MockProjectPropertyLinkRemoter>(this.Proxy.Predecessor);
        public override bool IsImported => this.Proxy.IsImported;
    }
}
