// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.DotNet.Cli.Utils
{
    public interface IEnvironmentProvider
    {
        IEnumerable<string> ExecutableExtensions { get; }

        string GetCommandPath(string commandName, params string[] extensions);

        string GetCommandPathFromRootPath(string rootPath, string commandName, params string[] extensions);

        string GetCommandPathFromRootPath(string rootPath, string commandName, IEnumerable<string> extensions);

        bool GetEnvironmentVariableAsBool(string name, bool defaultValue);

        string GetEnvironmentVariable(string name);

        string GetEnvironmentVariable(string variable, EnvironmentVariableTarget target);

        void SetEnvironmentVariable(string variable, string value, EnvironmentVariableTarget target);
    }
}
