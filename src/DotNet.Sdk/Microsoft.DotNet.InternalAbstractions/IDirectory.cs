// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.Extensions.EnvironmentAbstractions
{
    internal interface IDirectory
    {
        bool Exists(string path);

        ITemporaryDirectory CreateTemporaryDirectory();

        IEnumerable<string> EnumerateFiles(string path);

        IEnumerable<string> EnumerateFileSystemEntries(string path);

        string GetCurrentDirectory();

        void CreateDirectory(string path);

        void Delete(string path, bool recursive);

        void Move(string source, string destination);
    }
}
