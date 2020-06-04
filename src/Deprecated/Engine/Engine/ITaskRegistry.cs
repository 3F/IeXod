// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Xml;
using net.r_eg.IeXod.BuildEngine.Shared;
using net.r_eg.IeXod.Framework;

namespace net.r_eg.IeXod.BuildEngine
{
    internal interface ITaskRegistry
    {
        void RegisterTask(UsingTask usingTask, Expander expander, EngineLoggingServices loggingServices, BuildEventContext context);
        bool GetRegisteredTask(string taskName, string taskProjectFile, XmlNode taskNode, bool exactMatchRequired, EngineLoggingServices loggingServices, BuildEventContext context, out LoadedType taskClass);
        void Clear();
    }
}
