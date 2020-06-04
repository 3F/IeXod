// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using net.r_eg.IeXod.Collections;
using net.r_eg.IeXod.Construction;
using net.r_eg.IeXod.Execution;
using net.r_eg.IeXod.Shared;
using net.r_eg.IeXod.Shared.FileSystem;
using ErrorUtilities = net.r_eg.IeXod.Shared.ErrorUtilities;
using InvalidToolsetDefinitionException = net.r_eg.IeXod.Exceptions.InvalidToolsetDefinitionException;

namespace net.r_eg.IeXod.Evaluation
{
    /// <summary>
    /// Class used to read toolset configurations.
    /// </summary>
    internal class ToolsetConfigurationReader : ToolsetReader
    {
        /// <summary>
        /// A section of a toolset configuration
        /// </summary>
        private ToolsetConfigurationSection _configurationSection = null;

        /// <summary>
        /// Delegate used to read application configurations
        /// </summary>
        private readonly Func<Configuration> _readApplicationConfiguration;

        /// <summary>
        /// Flag indicating that an attempt has been made to read the configuration
        /// </summary>
        private bool _configurationReadAttempted = false;

        /// <summary>
        /// Character used to separate search paths specified for MSBuildExtensionsPath* in
        /// the config file
        /// </summary>
        private static readonly char[] s_separatorForExtensionsPathSearchPaths = MSBuildConstants.SemicolonChar;

        /// <summary>
        /// Cached values of tools version -> project import search paths table
        /// </summary>
        private readonly Dictionary<string, Dictionary<string, ProjectImportPathMatch>> _projectImportSearchPathsCache;

        /// <summary>
        /// Default constructor
        /// </summary>
        internal ToolsetConfigurationReader(PropertyDictionary<ProjectPropertyInstance> environmentProperties, PropertyDictionary<ProjectPropertyInstance> globalProperties)
            : this(environmentProperties, globalProperties, ReadApplicationConfiguration)
        {
        }

        /// <summary>
        /// Constructor taking a delegate for unit test purposes only
        /// </summary>
        internal ToolsetConfigurationReader(PropertyDictionary<ProjectPropertyInstance> environmentProperties, PropertyDictionary<ProjectPropertyInstance> globalProperties, Func<Configuration> readApplicationConfiguration)
            : base(environmentProperties, globalProperties)
        {
            ErrorUtilities.VerifyThrowArgumentNull(readApplicationConfiguration, "readApplicationConfiguration");
            _readApplicationConfiguration = readApplicationConfiguration;
            _projectImportSearchPathsCache = new Dictionary<string, Dictionary<string, ProjectImportPathMatch>>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns the list of tools versions
        /// </summary>
        protected override IEnumerable<ToolsetPropertyDefinition> ToolsVersions
        {
            get
            {
                if (ConfigurationSection != null)
                {
                    foreach (ToolsetElement toolset in ConfigurationSection.Toolsets)
                    {
                        ElementLocation location = ElementLocation.Create(
                            toolset.ElementInformation.Source,
                            toolset.ElementInformation.LineNumber,
                            0);

                        if (toolset.toolsVersion != null && toolset.toolsVersion.Length == 0)
                        {
                            InvalidToolsetDefinitionException.Throw(
                                "InvalidToolsetValueInConfigFileValue",
                                location.LocationString);
                        }

                        yield return new ToolsetPropertyDefinition(toolset.toolsVersion, string.Empty, location);
                    }
                }
            }
        }

        /// <summary>
        /// Returns the default tools version, or null if none was specified
        /// </summary>
        protected override string DefaultToolsVersion => ConfigurationSection?.Default;

        /// <summary>
        /// Returns the path to find override tasks, or null if none was specified
        /// </summary>
        protected override string MSBuildOverrideTasksPath => ConfigurationSection?.MSBuildOverrideTasksPath;

        /// <summary>
        /// DefaultOverrideToolsVersion attribute on msbuildToolsets element, specifying the tools version that should be used by 
        /// default to build projects with this version of MSBuild.
        /// </summary>
        protected override string DefaultOverrideToolsVersion => ConfigurationSection?.DefaultOverrideToolsVersion;

        /// <summary>
        /// Lazy getter for the ToolsetConfigurationSection
        /// Returns null if the section is not present
        /// </summary>
        private ToolsetConfigurationSection ConfigurationSection
        {
            get
            {
                if (null == _configurationSection && !_configurationReadAttempted)
                {
                    try
                    {
                        Configuration configuration = _readApplicationConfiguration();
                        _configurationSection = ToolsetConfigurationReaderHelpers.ReadToolsetConfigurationSection(configuration);
                    }
                    catch (ConfigurationException ex)
                    {
                        // ConfigurationException is obsolete, but we catch it rather than 
                        // ConfigurationErrorsException (which is what we throw below) because it is more 
                        // general and we don't want to miss catching some other derived exception.
                        InvalidToolsetDefinitionException.Throw(ex, "ConfigFileReadError", ElementLocation.Create(ex.Source, ex.Line, 0).LocationString, ex.BareMessage);
                    }
                    finally
                    {
                        _configurationReadAttempted = true;
                    }
                }

                return _configurationSection;
            }
        }

        /// <summary>
        /// Provides an enumerator over property definitions for a specified tools version
        /// </summary>
        protected override IEnumerable<ToolsetPropertyDefinition> GetPropertyDefinitions(string toolsVersion)
        {
            ToolsetElement toolsetElement = ConfigurationSection.Toolsets.GetElement(toolsVersion);

            if (toolsetElement == null)
            {
                yield break;
            }

            foreach (ToolsetElement.PropertyElement propertyElement in toolsetElement.PropertyElements)
            {
                ElementLocation location = ElementLocation.Create(propertyElement.ElementInformation.Source, propertyElement.ElementInformation.LineNumber, 0);

                if (propertyElement.Name != null && propertyElement.Name.Length == 0)
                {
                    InvalidToolsetDefinitionException.Throw("InvalidToolsetValueInConfigFileValue", location.LocationString);
                }

                yield return new ToolsetPropertyDefinition(propertyElement.Name, propertyElement.Value, location);
            }
        }

        /// <summary>
        /// Provides an enumerator over the set of sub-toolset names available to a particular
        /// tools version.  MSBuild config files do not currently support sub-toolsets, so
        /// we return nothing. 
        /// </summary>
        /// <param name="toolsVersion">The tools version.</param>
        /// <returns>An enumeration of the sub-toolsets that belong to that tools version.</returns>
        protected override IEnumerable<string> GetSubToolsetVersions(string toolsVersion)
        {
            yield break;
        }

        /// <summary>
        /// Provides an enumerator over property definitions for a specified sub-toolset version 
        /// under a specified toolset version. In the ToolsetConfigurationReader case, breaks 
        /// immediately because we do not currently support sub-toolsets in the configuration file. 
        /// </summary>
        /// <param name="toolsVersion">The tools version.</param>
        /// <param name="subToolsetVersion">The sub-toolset version.</param>
        /// <returns>An enumeration of property definitions.</returns>
        protected override IEnumerable<ToolsetPropertyDefinition> GetSubToolsetPropertyDefinitions(string toolsVersion, string subToolsetVersion)
        {
            yield break;
        }

        /// <summary>
        /// Returns a map of project property names / list of search paths for the specified toolsVersion and os
        /// </summary>
        protected override Dictionary<string, ProjectImportPathMatch> GetProjectImportSearchPathsTable(string toolsVersion, string os)
        {
            Dictionary<string, ProjectImportPathMatch> kindToPathsCache;
            var key = toolsVersion + ":" + os;
            if (_projectImportSearchPathsCache.TryGetValue(key, out kindToPathsCache))
            {
                return kindToPathsCache;
            }

            // Read and populate the map
            kindToPathsCache = new Dictionary<string, ProjectImportPathMatch>();
            _projectImportSearchPathsCache[key] = kindToPathsCache;

            ToolsetElement toolsetElement = ConfigurationSection.Toolsets.GetElement(toolsVersion);
            var propertyCollection = toolsetElement?.AllProjectImportSearchPaths?.GetElement(os)?.PropertyElements;
            if (propertyCollection == null || propertyCollection.Count == 0)
            {
                return kindToPathsCache;
            }

            kindToPathsCache = ComputeDistinctListOfSearchPaths(propertyCollection);

            return kindToPathsCache;
        }

        /// <summary>
        /// Returns a list of the search paths for a given search path property collection
        /// </summary>
        private Dictionary<string, ProjectImportPathMatch> ComputeDistinctListOfSearchPaths(ToolsetElement.PropertyElementCollection propertyCollection)
        {
            var pathsTable = new Dictionary<string, ProjectImportPathMatch>();

            foreach (ToolsetElement.PropertyElement property in propertyCollection)
            {
                if (string.IsNullOrEmpty(property.Value) || string.IsNullOrEmpty(property.Name))
                {
                    continue;
                }

                //FIXME: handle ; in path on Unix
                var paths = property.Value
                    .Split(s_separatorForExtensionsPathSearchPaths, StringSplitOptions.RemoveEmptyEntries)
                    .Distinct()
                    .Where(path => !string.IsNullOrEmpty(path));

                pathsTable.Add(property.Name, new ProjectImportPathMatch(property.Name, paths.ToList()));
            }

            return pathsTable;
        }

        /// <summary>
        /// Reads the application configuration file.
        /// NOTE: this is abstracted into a method to support unit testing GetToolsetDataFromConfiguration().
        /// Unit tests wish to avoid reading (nunit.exe) application configuration file.
        /// </summary>
        private static Configuration ReadApplicationConfiguration()
        {
            // When running from the command-line or from VS, use the msbuild.exe.config file.
            if (BuildEnvironmentHelper.Instance.Mode != BuildEnvironmentMode.None &&
                !BuildEnvironmentHelper.Instance.RunningTests &&
                FileSystems.Default.FileExists(BuildEnvironmentHelper.Instance.CurrentMSBuildConfigurationFile))
            {
                var map = QualifyConfig(BuildEnvironmentHelper.Instance.CurrentMSBuildConfigurationFile);

                try { return ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None, true); }
                finally { File.Delete(map.ExeConfigFilename); }
            }

            // When running tests or the expected config file doesn't exist, fall-back to default
            return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        private static ExeConfigurationFileMap QualifyConfig(string file)
        {
            var xml = XDocument.Load(file);

            foreach(var section in xml.Descendants("section"))
            {
                var type = section.Attribute("type");

                // Microsoft.Build.Evaluation.ToolsetConfigurationSection, Microsoft.Build, Version=15.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
                if(type.Value.IndexOf("Microsoft.Build", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    type.Value = typeof(ToolsetConfigurationSection).AssemblyQualifiedName;
                }
            }

            var fout = FileUtilities.GetTemporaryFile();
            xml.Save(fout);

            return new ExeConfigurationFileMap { ExeConfigFilename = fout };
        }
    }
}
