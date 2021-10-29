// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Tasks.Deployment.ManifestUtilities;
using net.r_eg.IeXod.Utilities;

namespace net.r_eg.IeXod.Tasks
{
    /// <summary>
    /// Updates selected properties in a manifest and resigns.
    /// </summary>
    public class UpdateManifest : Task
    {
        [Required]
        public string ApplicationPath { get; set; }

        public string TargetFrameworkVersion { get; set; }

        [Required]
        public ITaskItem ApplicationManifest { get; set; }

        [Required]
        public ITaskItem InputManifest { get; set; }

        [Output]
        public ITaskItem OutputManifest { get; set; }

        public override bool Execute()
        {
            Manifest.UpdateEntryPoint(InputManifest.ItemSpec, OutputManifest.ItemSpec, ApplicationPath, ApplicationManifest.ItemSpec, TargetFrameworkVersion);

            return true;
        }
    }
}
