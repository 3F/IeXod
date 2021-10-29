// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.DotNet.Cli.Utils
{
    public interface IReporter
    {

        void WriteLine(string message);

        void WriteLine();

        void Write(string message);

    }
}
