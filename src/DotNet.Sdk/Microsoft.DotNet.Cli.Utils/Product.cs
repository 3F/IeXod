// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.DotNet.Cli.Utils
{
    public class Product
    {
        public static string LongName => Resources.GetString("DotNetSdkInfo");
        public static readonly string Version = GetProductVersion();

        private static string GetProductVersion()
        {
            DotnetVersionFile versionFile = DotnetFiles.VersionFileObject;
            return versionFile.BuildNumber ?? string.Empty;
        }
    }
}
