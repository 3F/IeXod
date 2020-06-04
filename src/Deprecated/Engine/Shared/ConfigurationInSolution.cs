// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Globalization;
using System.Text;

using net.r_eg.IeXod.BuildEngine;

namespace net.r_eg.IeXod.BuildEngine.Shared
{
    /// <summary>
    /// This represents basic configuration functionality used in solution and project configurations.
    /// Since solution configurations don't need anything else, they are represented with this class.
    /// </summary>
    /// <owner>LukaszG</owner>
    internal class ConfigurationInSolution
    {
        internal const char configurationPlatformSeparator = '|';

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configurationName"></param>
        /// <param name="platformName"></param>
        /// <owner>LukaszG</owner>
        internal ConfigurationInSolution(string configurationName, string platformName)
        {
            this.configurationName = configurationName;
            this.platformName = platformName;

            // Some configurations don't have the platform part
            if ((platformName != null) && (platformName.Length > 0))
            {
                this.fullName = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", configurationName, configurationPlatformSeparator, platformName);
            }
            else
            {
                this.fullName = configurationName;
            }
        }

        private string configurationName;

        /// <summary>
        /// The configuration part of this, uh, configuration - e.g. "Debug", "Release"
        /// </summary>
        /// <owner>LukaszG</owner>
        internal string ConfigurationName
        {
            get { return this.configurationName; }
        }

        private string platformName;

        /// <summary>
        /// The platform part of this configuration - e.g. "Any CPU", "Win32"
        /// </summary>
        /// <owner>LukaszG</owner>
        internal string PlatformName
        {
            get { return this.platformName; }
        }

        private string fullName;

        /// <summary>
        /// The full name of this configuration - e.g. "Debug|Any CPU"
        /// </summary>
        /// <owner>LukaszG</owner>
        internal string FullName
        {
            get { return this.fullName; }
        }

        private BuildItemGroup projectBuildItems;

        /// <summary>
        /// Build items corresponding to projects built in this configuration
        /// </summary>
        internal BuildItemGroup ProjectBuildItems
        {
            get { return this.projectBuildItems; }
            set { this.projectBuildItems = value; }
        }
    }
}
