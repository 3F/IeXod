// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod.Tasks
{
    /// <summary>
    /// A unified assembly name.
    /// </summary>
    internal class UnifiedAssemblyName
    {
        public UnifiedAssemblyName(AssemblyNameExtension preUnified, AssemblyNameExtension postUnified, bool isUnified, UnificationReason unificationReason, bool isPrerequisite, bool? isRedistRoot, string redistName)
        {
            PreUnified = preUnified;
            PostUnified = postUnified;
            IsUnified = isUnified;
            IsPrerequisite = isPrerequisite;
            IsRedistRoot = isRedistRoot;
            RedistName = redistName;
            UnificationReason = unificationReason;
        }

        public AssemblyNameExtension PreUnified { get; }

        public AssemblyNameExtension PostUnified { get; }

        public bool IsUnified { get; }

        public UnificationReason UnificationReason { get; }

        public bool IsPrerequisite { get; }

        public bool? IsRedistRoot { get; }

        public string RedistName { get; }
    }
}
