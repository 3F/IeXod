// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;

namespace Microsoft.Extensions.EnvironmentAbstractions
{
    internal interface IFile
    {
        bool Exists(string path);

        string ReadAllText(string path);

        Stream OpenRead(string path);

        Stream OpenFile(
            string path,
            FileMode fileMode,
            FileAccess fileAccess,
            FileShare fileShare,
            int bufferSize,
            FileOptions fileOptions);

        void CreateEmptyFile(string path);

        void WriteAllText(string path, string content);

        void Move(string source, string destination);

        void Copy(string source, string destination);

        void Delete(string path);
    }
}
