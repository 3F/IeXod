// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.IO;

namespace Microsoft.DotNet.Cli.Utils
{
    public class Muxer
    {
        public static readonly string MuxerName = "dotnet";

        private readonly string _muxerPath;

        internal string SharedFxVersion
        {
            get
            {
                var depsFile = new FileInfo(GetDataFromAppDomain("FX_DEPS_FILE"));
                return depsFile.Directory.Name;
            }
        }

        public string MuxerPath
        {
            get
            {
                if (_muxerPath == null)
                {
                    throw new InvalidOperationException(Resources.GetString("UnableToLocateDotnetMultiplexer"));
                }
                return _muxerPath;
            }
        }

        public Muxer()
        {
            _muxerPath = Process.GetCurrentProcess().MainModule.FileName;
        }

        public static string GetDataFromAppDomain(string propertyName)
        {
            return AppContext.GetData(propertyName) as string;
        }
    }
}
