// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if FEATURE_APPDOMAIN

using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Shared;
using net.r_eg.IeXod.Utilities;

namespace net.r_eg.IeXod.Tasks
{
    /// <summary>
    /// This class provides the same functionality as the Task class, but derives from MarshalByRefObject so that it can be
    /// instantiated in its own app domain.
    /// </summary>
    [LoadInSeparateAppDomain]
    public abstract class AppDomainIsolatedTaskExtension : AppDomainIsolatedTask
    {
        #region Constructors

        internal AppDomainIsolatedTaskExtension() :
            base(AssemblyResources.PrimaryResources, "MSBuild.")
        {
            _logExtension = new TaskLoggingHelperExtension(
                this,
                AssemblyResources.PrimaryResources,
                AssemblyResources.SharedResources,
                "MSBuild.");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets an instance of a TaskLoggingHelperExtension class containing task logging methods.
        /// </summary>
        /// <value>The logging helper object.</value>
        public new TaskLoggingHelper Log => _logExtension;

        // the logging helper
        private readonly TaskLoggingHelperExtension _logExtension;

        #endregion
    }
}
#endif
