﻿// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace Microsoft.DotNet.Cli.Utils
{
    public struct CommandResult
    {
        public static readonly CommandResult Empty = new CommandResult();

        public ProcessStartInfo StartInfo { get; }
        public int ExitCode { get; }
        public string StdOut { get; }
        public string StdErr { get; }

        public CommandResult(ProcessStartInfo startInfo, int exitCode, string stdOut, string stdErr)
        {
            StartInfo = startInfo;
            ExitCode = exitCode;
            StdOut = stdOut;
            StdErr = stdErr;
        }
    }
}
