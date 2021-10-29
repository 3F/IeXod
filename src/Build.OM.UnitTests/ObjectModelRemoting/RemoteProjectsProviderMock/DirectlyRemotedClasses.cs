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
    using System.Diagnostics;
    using net.r_eg.IeXod.Sdk;

    internal static class DirectlyRemotedClasses
    {
        internal static RemotedResolvedImport Export(this ResolvedImport resolvedImport, ProjectCollectionLinker exporter)
        {
            return new RemotedResolvedImport(resolvedImport, exporter);
        }

        internal static ResolvedImport Import(this RemotedResolvedImport remoted, ProjectCollectionLinker importer)
        {
            return remoted.Import(importer);
        }
    }

    internal class RemotedResolvedImport
    {
        public RemotedResolvedImport(ResolvedImport resolvedImport, ProjectCollectionLinker exporter)
        {
            this.ImportingElement = exporter.Export<ProjectElement, MockProjectImportElementLinkRemoter>(resolvedImport.ImportingElement);
            this.ImportedProject = exporter.Export<ProjectElement, MockProjectRootElementLinkRemoter>(resolvedImport.ImportedProject);
            this.IsImported = resolvedImport.IsImported;
            this.SdkResult = resolvedImport.SdkResult;
        }

        public MockProjectImportElementLinkRemoter ImportingElement { get; }
        public MockProjectRootElementLinkRemoter ImportedProject { get; }

        // this is remotable enough.
        public SdkResult SdkResult { get; }

        public bool IsImported { get; }

        ResolvedImport Import(ProjectCollectionLinker importer)
        {
            var importElement = (ProjectImportElement)importer.Import<ProjectElement, MockProjectImportElementLinkRemoter>(this.ImportingElement);
            var projectElement = (ProjectRootElement)importer.Import<ProjectElement, MockProjectRootElementLinkRemoter>(this.ImportedProject);
            return importer.LinkFactory.Create(importElement, projectElement, 0, this.SdkResult, this.IsImported);
        }
    }
}
