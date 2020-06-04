﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod.Tasks
{
    /// <summary>
    /// Resolve searchpath type {GAC}
    /// </summary>
    internal class GacResolver : Resolver
    {
        /// <summary>
        /// Delegate to get the assembly path in the GAC
        /// </summary>
        private readonly GetAssemblyPathInGac _getAssemblyPathInGac;

        /// <summary>
        /// Construct.
        /// </summary>
        /// <param name="targetProcessorArchitecture">Like x86 or IA64\AMD64, the processor architecture being targeted.</param>
        /// <param name="searchPathElement">The search path element.</param>
        /// <param name="getAssemblyName">Delegate to get the assembly name object.</param>
        /// <param name="fileExists">Delegate to check if the file exists.</param>
        /// <param name="getRuntimeVersion">Delegate to get the runtime version.</param>
        /// <param name="targetedRuntimeVesion">The targeted runtime version.</param>
        /// <param name="getAssemblyPathInGac">Delegate to get assembly path in the GAC.</param>
        public GacResolver(System.Reflection.ProcessorArchitecture targetProcessorArchitecture, string searchPathElement, GetAssemblyName getAssemblyName, FileExists fileExists, GetAssemblyRuntimeVersion getRuntimeVersion, Version targetedRuntimeVesion, GetAssemblyPathInGac getAssemblyPathInGac)
            : base(searchPathElement, getAssemblyName, fileExists, getRuntimeVersion, targetedRuntimeVesion, targetProcessorArchitecture, true)
        {
            _getAssemblyPathInGac = getAssemblyPathInGac;
        }

        /// <summary>
        /// Resolve a reference to a specific file name.
        /// </summary>
        /// <param name="assemblyName">The assembly name object of the assembly.</param>
        /// <param name="sdkName"></param>
        /// <param name="rawFileNameCandidate">The reference's 'include' treated as a raw file name.</param>
        /// <param name="isPrimaryProjectReference">Whether or not this reference was directly from the project file (and therefore not a dependency)</param>
        /// <param name="wantSpecificVersion">Whether an exact version match is requested.</param>
        /// <param name="executableExtensions">Allowed executable extensions.</param>
        /// <param name="hintPath">The item's hintpath value.</param>
        /// <param name="assemblyFolderKey">Like "hklm\Vendor RegKey" as provided to a reference by the <AssemblyFolderKey> on the reference in the project.</param>
        /// <param name="assembliesConsideredAndRejected">Receives the list of locations that this function tried to find the assembly. May be "null".</param>
        /// <param name="foundPath">The path where the file was found.</param>
        /// <param name="userRequestedSpecificFile">Whether or not the user wanted a specific file (for example, HintPath is a request for a specific file)</param>
        /// <returns>True if the file was resolved.</returns>
        public override bool Resolve
        (
            AssemblyNameExtension assemblyName,
            string sdkName,
            string rawFileNameCandidate,
            bool isPrimaryProjectReference,
            bool wantSpecificVersion,
            string[] executableExtensions,
            string hintPath,
            string assemblyFolderKey,
            List<ResolutionSearchLocation> assembliesConsideredAndRejected,
            out string foundPath,
            out bool userRequestedSpecificFile
        )
        {
            foundPath = null;
            userRequestedSpecificFile = false;

            if (assemblyName != null)
            {
                // {GAC} was passed in.
                string gacResolved = _getAssemblyPathInGac(assemblyName, targetProcessorArchitecture, getRuntimeVersion,
                    targetedRuntimeVersion, fileExists, fullFusionName: false, specificVersion: wantSpecificVersion);

                if (!string.IsNullOrEmpty(gacResolved) && fileExists(gacResolved))
                {
                    foundPath = gacResolved;
                    return true;
                }
                else
                {
                    // Record this as a location that was considered.
                    if (assembliesConsideredAndRejected != null)
                    {
                        var considered = new ResolutionSearchLocation
                        {
                            FileNameAttempted = assemblyName.FullName,
                            SearchPath = searchPathElement,
                            AssemblyName = assemblyName,
                            Reason = NoMatchReason.NotInGac
                        };
                        assembliesConsideredAndRejected.Add(considered);
                    }
                }
            }

            return false;
        }
    }
}
