// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.DotNet.Cli.Utils
{
    public class CommandUnknownException : GracefulException
    {
        public CommandUnknownException(string commandName) : base(string.Format(
            Resources.GetString("NoExecutableFoundMatchingCommand"),
            commandName))
        {
        }

        public CommandUnknownException(string commandName, Exception innerException) : base(
            string.Format(
                Resources.GetString("NoExecutableFoundMatchingCommand"),
                commandName),
            innerException)
        {
        }
    }
}
