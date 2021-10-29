// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.Versioning;
using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod.Tasks.AssemblyDependency
{
    /// <summary>
    /// Information about a managed assembly.
    /// </summary>
    /// <remarks>
    /// After initial construction, this object is readonly and data-only,
    /// allowing it to be safely cached.
    /// </remarks>
    class AssemblyMetadata
    {
        public readonly AssemblyNameExtension[] Dependencies;
        public readonly FrameworkName FrameworkName;
        public readonly string[] ScatterFiles;

        public AssemblyMetadata(string path)
        {
            using (var import = new AssemblyInformation(path))
            {
                Dependencies = import.Dependencies;
                FrameworkName = import.FrameworkNameAttribute;
                ScatterFiles = NativeMethodsShared.IsWindows ? import.Files : null;
            }
        }
    }
}
