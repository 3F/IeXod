// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;
using net.r_eg.IeXod.Framework;

namespace net.r_eg.IeXod.Tasks.Hosting
{
    /// <summary>
    /// Defines an interface for the Vbc/Csc tasks to communicate information about
    /// analyzers and rulesets to the IDE.
    /// </summary>
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(true)]
    [Guid("B5A95716-2053-4B70-9FBF-E4148EBA96BC")]
    public interface IAnalyzerHostObject
    {
        bool SetAnalyzers(ITaskItem[] analyzers);
        bool SetRuleSet(string ruleSetFile);
        bool SetAdditionalFiles(ITaskItem[] additionalFiles);
    }
}
