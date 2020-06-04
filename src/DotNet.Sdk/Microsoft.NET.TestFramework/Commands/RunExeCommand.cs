// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit.Abstractions;

namespace Microsoft.NET.TestFramework.Commands
{
    public class RunExeCommand : TestCommand
    {
        private readonly string _commandPath;

        public RunExeCommand(ITestOutputHelper log, string commandPath, params string[] args) : base(log)
        {
            if (!File.Exists(commandPath))
            {
                throw new ArgumentException($"Cannot find command {commandPath}");
            }

            _commandPath = commandPath;
            Arguments.AddRange(args);
        }

        protected override SdkCommandSpec CreateCommand(IEnumerable<string> args)
        {
            var sdkCommandSpec = new SdkCommandSpec()
            {
                FileName = _commandPath,
                Arguments = args.ToList(),
                WorkingDirectory = WorkingDirectory,
            };
            TestContext.Current.AddTestEnvironmentVariables(sdkCommandSpec);
            return sdkCommandSpec;
        }
    }
}
