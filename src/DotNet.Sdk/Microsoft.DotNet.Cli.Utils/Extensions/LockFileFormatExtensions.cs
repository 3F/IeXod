// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.ProjectModel;

namespace Microsoft.DotNet.Cli.Utils
{
    public static class LockFileFormatExtensions
    {
        public static async Task<LockFile> ReadWithLock(this LockFileFormat subject, string path)
        {
            return await ConcurrencyUtilities.ExecuteWithFileLockedAsync(
                path, 
                lockedToken =>
                {
                    if (!File.Exists(path))
                    {
                        throw new GracefulException(string.Join(
                            Environment.NewLine,
                            string.Format(Resources.GetString("FileNotFound"), path),
                            Resources.GetString("ProjectNotRestoredOrRestoreFailed")));
                    }
                    
                    var lockFile = FileAccessRetrier.RetryOnFileAccessFailure(() => subject.Read(path), Resources.GetString("CouldNotAccessAssetsFile"));

                    return lockFile;       
                },
                CancellationToken.None);
        }
    }
}
