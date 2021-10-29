// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.EnvironmentAbstractions;

namespace Microsoft.DotNet.Tools.Test.Utilities.Mock
{
    internal interface ITemporaryDirectoryMock : ITemporaryDirectory
    {
        bool DisposedTemporaryDirectory { get; }
    }
}
