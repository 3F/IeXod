// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.DotNet.Cli.Utils
{
    public class CommandAvailableAsDotNetToolException : GracefulException
    {
        public CommandAvailableAsDotNetToolException(string commandName) : base(GetMessage(commandName))
        {
        }

        public CommandAvailableAsDotNetToolException(string commandName, Exception innerException) : base(
            GetMessage(commandName), innerException)
        {
        }

        private static string GetMessage(string commandName)
        {
            var commandRemoveLeadingDotnet = commandName.Replace("dotnet-", string.Empty);
            var packageName = "dotnet-" + commandRemoveLeadingDotnet.ToLower();

            return string.Format(Resources.GetString("CannotFindCommandAvailableAsTool"),
                commandRemoveLeadingDotnet,
                packageName);
        }
    }
}
