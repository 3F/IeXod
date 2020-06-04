// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Microsoft.NET.TestFramework
{
    public class TestPackageReference
    {
        public TestPackageReference(string id, string version = null, string nupkgPath = null, string privateAssets = null)
        {
            ID = id;
            Version = version;
            NupkgPath = nupkgPath;
            PrivateAssets = privateAssets;
        }

        public string ID { get; private set; }
        public string Version { get; private set; }
        public string NupkgPath { get; private set; }
        public string PrivateAssets { get; private set; }

        public bool NuGetPackageExists()
        {
            return File.Exists(Path.Combine(this.NupkgPath, String.Concat(this.ID + "." + this.Version + ".nupkg")));
        }
    }
}
