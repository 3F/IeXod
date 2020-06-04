// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

using net.r_eg.IeXod.Shared;
using net.r_eg.IeXod.Utilities;

namespace net.r_eg.IeXod.Tasks
{
    /// <summary>
    /// COM reference wrapper class for the tlbimp tool using a PIA. 
    /// </summary>
    internal sealed class PiaReference : ComReference
    {
        #region Constructors

        internal PiaReference(TaskLoggingHelper taskLoggingHelper, bool silent, ComReferenceInfo referenceInfo, string itemName)
            : base(taskLoggingHelper, silent, referenceInfo, itemName)
        {
            // do nothing
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the resolved assembly path for the typelib wrapper.
        /// </summary>
        internal override bool FindExistingWrapper(out ComReferenceWrapperInfo wrapperInfo, DateTime componentTimestamp)
        {
            wrapperInfo = null;

            // Let NDP do the dirty work...
            TypeLibConverter converter = new TypeLibConverter();

            if (!converter.GetPrimaryInteropAssembly(ReferenceInfo.attr.guid, ReferenceInfo.attr.wMajorVerNum, ReferenceInfo.attr.wMinorVerNum, ReferenceInfo.attr.lcid,
                out string asmName, out string asmCodeBase))
            {
                return false;
            }

            // let's try to load the assembly to determine its path and if it's there
            try
            {
                if (!string.IsNullOrEmpty(asmCodeBase))
                {
                    var uri = new Uri(asmCodeBase);

                    // make sure the PIA can be loaded
                    Assembly assembly = Assembly.UnsafeLoadFrom(uri.LocalPath);

                    // got here? then assembly must have been loaded successfully.
                    wrapperInfo = new ComReferenceWrapperInfo
                    {
                        path = uri.LocalPath,
                        assembly = assembly,

                        // We need to remember the original assembly name of this PIA in case it gets redirected to a newer 
                        // version and other COM components use that name to reference the PIA. assembly.FullName wouldn't
                        // work here since we'd get the redirected assembly name.
                        originalPiaName = new AssemblyNameExtension(AssemblyName.GetAssemblyName(uri.LocalPath))
                    };
                }
                else
                {
                    Assembly assembly = Assembly.Load(asmName);

                    // got here? then assembly must have been loaded successfully.
                    wrapperInfo = new ComReferenceWrapperInfo
                    {
                        path = assembly.Location,
                        assembly = assembly,

                        // We need to remember the original assembly name of this PIA in case it gets redirected to a newer 
                        // version and other COM components use that name to reference the PIA. 
                        originalPiaName = new AssemblyNameExtension(asmName, true)
                    };
                }
            }
            catch (FileNotFoundException)
            {
                // This means that assembly file cannot be found.
                // We don't need to do anything here; wrapperInfo is not set 
                // and we'll assume that the assembly doesn't exist.
            }
            catch (BadImageFormatException)
            {
                // Similar case as above, except we should additionally warn the user that the assembly file 
                // is not really a valid assembly file.
                if (!Silent)
                {
                    Log.LogWarningWithCodeFromResources("ResolveComReference.BadAssemblyImage", asmName);
                }
            }

            // have we found the wrapper?
            if (wrapperInfo != null)
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}
