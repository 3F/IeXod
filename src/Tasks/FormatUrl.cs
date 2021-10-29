// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Tasks.Deployment.ManifestUtilities;

namespace net.r_eg.IeXod.Tasks
{
    /// <summary>
    /// Formats a url by canonicalizing it (i.e. " " -> "%20") and transforming "localhost" to "machinename".
    /// </summary>
    public sealed class FormatUrl : TaskExtension
    {
        public string InputUrl { get; set; }

        [Output]
        public string OutputUrl { get; set; }

        public override bool Execute()
        {
            OutputUrl = InputUrl != null ? PathUtil.Format(InputUrl) : String.Empty;
            return true;
        }
    }
}
