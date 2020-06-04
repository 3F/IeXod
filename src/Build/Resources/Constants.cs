﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using net.r_eg.IeXod.Collections;
using net.r_eg.IeXod.Evaluation;
using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod.Internal
{
    /// <summary>
    /// Contains a list of the special (reserved) properties that are settable by MSBuild code only.
    /// </summary>
    internal static class ReservedPropertyNames
    {
        // NOTE: if you add to this list, update the ReservedProperties hashtable below
        internal const string projectDirectory = "MSBuildProjectDirectory";
        internal const string projectDirectoryNoRoot = "MSBuildProjectDirectoryNoRoot";
        internal const string projectFile = "MSBuildProjectFile";
        internal const string projectExtension = "MSBuildProjectExtension";
        internal const string projectFullPath = "MSBuildProjectFullPath";
        internal const string projectName = "MSBuildProjectName";
        internal const string thisFileDirectory = "MSBuildThisFileDirectory";
        internal const string thisFileDirectoryNoRoot = "MSBuildThisFileDirectoryNoRoot";
        internal const string thisFile = "MSBuildThisFile"; // "MSBuildThisFileFile" sounds silly!
        internal const string thisFileExtension = "MSBuildThisFileExtension";
        internal const string thisFileFullPath = "MSBuildThisFileFullPath";
        internal const string thisFileName = "MSBuildThisFileName";
        internal const string binPath = "MSBuildBinPath";
        internal const string projectDefaultTargets = "MSBuildProjectDefaultTargets";
        internal const string extensionsPath = "MSBuildExtensionsPath";
        internal const string extensionsPath32 = "MSBuildExtensionsPath32";
        internal const string extensionsPath64 = "MSBuildExtensionsPath64";
        internal const string userExtensionsPath = "MSBuildUserExtensionsPath";
        internal const string toolsPath = MSBuildConstants.ToolsPath;
        internal const string toolsVersion = "MSBuildToolsVersion";
        internal const string appVersion = "IeXodVersion";
        internal const string appProductVersion = "IeXodProductVersion"; //+ alpha, beta, RC, ...
        internal const string msbuildRuntimeType = "MSBuildRuntimeType";
        internal const string overrideTasksPath = "MSBuildOverrideTasksPath";
        internal const string defaultOverrideToolsVersion = "DefaultOverrideToolsVersion";
        internal const string startupDirectory = "MSBuildStartupDirectory";
        internal const string buildNodeCount = "MSBuildNodeCount";
        internal const string lastTaskResult = "MSBuildLastTaskResult";
        internal const string extensionsPathSuffix = "MSBuild";
        internal const string userExtensionsPathSuffix = "Microsoft\\MSBuild";
        internal const string programFiles32 = "MSBuildProgramFiles32";
        internal const string localAppData = "LocalAppData";
        internal const string assemblyVersion = "MSBuildAssemblyVersion";
        internal const string version = "MSBuildVersion";
        internal const string osName = "OS";
        internal const string frameworkToolsRoot = "MSBuildFrameworkToolsRoot";
        internal const string interactive = "MSBuildInteractive";

        /// <summary>
        /// Lookup for reserved property names. Intentionally do not include MSBuildExtensionsPath* or MSBuildUserExtensionsPath in this list.  We need tasks to be able to override those.
        /// </summary>
        private static readonly HashSet<string> ReservedProperties = new HashSet<string>(MSBuildNameIgnoreCaseComparer.Default)
        {
            projectDirectory,
            projectDirectoryNoRoot,
            projectFile,
            projectExtension,
            projectFullPath,
            projectName,

            thisFileDirectory,
            thisFileDirectoryNoRoot,
            thisFile,
            thisFileExtension,
            thisFileFullPath,
            thisFileName,

            binPath,
            projectDefaultTargets,
            toolsPath,
            toolsVersion,
            appVersion,
            msbuildRuntimeType,
            startupDirectory,
            buildNodeCount,
            lastTaskResult,
            programFiles32,
            assemblyVersion,
            version,
            interactive
        };

        /// <summary>
        /// Indicates if the given property is a reserved property.
        /// </summary>
        /// <returns>true, if specified property is reserved</returns>
        internal static bool IsReservedProperty(string property)
        {
            return ReservedProperties.Contains(property);
        }
    }

    /// <summary>
    /// Constants used by the Engine
    /// </summary>
    internal static class Constants
    {
        private static readonly FileVersionInfo _fileinfo = FileVersionInfo.GetVersionInfo(typeof(Constants).GetTypeInfo().Assembly.Location);

        /// <summary>
        /// If no default tools version is specified in the config file or registry, we'll use 2.0.
        /// The engine will use its binpath for the matching toolset path.
        /// </summary>
        internal const string defaultToolsVersion = "2.0";

        /// <summary>
        /// The toolsversion we will fall back to as a last resort if the default one cannot be found, this fallback should be the most current toolsversion known
        /// </summary>
        internal static string defaultFallbackToolsVersion = MSBuildConstants.CurrentToolsVersion;

        /// <summary>
        /// The toolsversion we will use when we construct the solution wrapper metaprojects; this should be the most current toolsversion known
        /// </summary>
        internal static string defaultSolutionWrapperProjectToolsVersion = MSBuildConstants.CurrentToolsVersion;

        /// <summary>
        /// Name of the property used to specify a Visual Studio version.
        /// </summary>
        internal const string VisualStudioVersionPropertyName = "VisualStudioVersion";

        /// <summary>
        /// Name of the property used to select which sub-toolset to use. 
        /// </summary>
        internal const string SubToolsetVersionPropertyName = VisualStudioVersionPropertyName;

        /// <summary>
        /// Value we should be setting VisualStudioVersion as the ultimate fallback when Dev10 is installed. 
        /// </summary>
        internal const string Dev10SubToolsetValue = "10.0";

        /// <summary>
        /// Number representing the current assembly's timestamp
        /// </summary>
        internal static long assemblyTimestamp;

        /// <summary>
        /// Current version of this MSBuild Engine assembly in the 
        /// form, e.g, "4.0"
        /// </summary>
        internal static string AssemblyVersion => MSBuildConstants.CurrentProductVersion;

        /// <summary>
        /// Version number of this IeXod. System.Version notation.
        /// </summary>
        internal static string AppVersion => _fileinfo.FileVersion;

        /// <summary>
        /// Product version of this IeXod that may contain additional information (beta, RC, ...).
        /// </summary>
        internal static string AppProductVersion => _fileinfo.ProductVersion;

        /// <summary>
        /// Number representing the current assembly's timestamp
        /// </summary>
        internal static long AssemblyTimestamp
        {
            get
            {
                if (assemblyTimestamp == 0)
                {
                    // Get the file version from the currently executing assembly.
                    // Use .CodeBase instead of .Location, because .Location doesn't
                    // work when net.r_eg.IeXod.dll has been shadow-copied, for example
                    // in scenarios where NUnit is loading net.r_eg.IeXod.
                    string path = FileUtilities.ExecutingAssemblyPath;

                    assemblyTimestamp = new FileInfo(path).LastWriteTime.Ticks;
                }

                return assemblyTimestamp;
            }
        }

        // Name of the environment variable that always points to 32-bit program files.
        internal const string programFilesx86 = "ProgramFiles(x86)";

        internal const string MSBuildAllProjectsPropertyName = "MSBuildAllProjects";
    }

    /// <summary>
    /// The set of available static methods.
    /// NOTE: Do not allow methods here that could do "bad" things under any circumstances.
    /// These must be completely benign operations, as they run during project load, which must be safe in VS.
    /// Key = Type or Type::Method, Value = AssemblyQualifiedTypeName (where null = mscorlib)
    /// </summary>
    /// <remarks>
    /// Placed here to avoid StyleCop error.
    /// </remarks>
    internal static class AvailableStaticMethods
    {
        /// <summary>
        /// Static methods that are allowed in constants. Key = Type or Type::Method, Value = Tuple of AssemblyQualifiedTypeName (where null = mscorlib) or the actual type object
        /// </summary>
        private static ConcurrentDictionary<string, Tuple<string, Type>> s_availableStaticMethods;

        /// <summary>
        /// Locker to protect initialization
        /// </summary>
        private static Object s_locker = new Object();

        static AvailableStaticMethods()
        {
            InitializeAvailableMethods();
        }

        /// <summary>
        /// Whether a key is present
        /// </summary>
        internal static bool ContainsKey(string key)
        {
            return s_availableStaticMethods.ContainsKey(key);
        }

        /// <summary>
        /// Add an entry if not already present
        /// </summary>
        internal static bool TryAdd(string key, Tuple<string, Type> value)
        {
            return s_availableStaticMethods.TryAdd(key, value);
        }

        /// <summary>
        /// Constructs the fully qualified method name and adds it to the cache
        /// </summary>
        /// <param name="typeFullName"></param>
        /// <param name="simpleMethodName"></param>
        /// <param name="typeInformation"></param>
        /// <returns></returns>
        public static bool TryAdd(string typeFullName, string simpleMethodName, Tuple<string, Type> typeInformation)
        {
            return s_availableStaticMethods.TryAdd(CreateQualifiedMethodName(typeFullName, simpleMethodName), typeInformation);
        }

        /// <summary>
        /// Get an entry if present
        /// </summary>
        internal static bool TryGetValue(string key, out Tuple<string, Type> value)
        {
            return s_availableStaticMethods.TryGetValue(key, out value);
        }

        /// <summary>
        /// Get an entry or null if not present
        /// </summary>
        internal static Tuple<string, Type> GetValue(string key)
        {
            Tuple<string, Type> typeInformation;
            return s_availableStaticMethods.TryGetValue(key, out typeInformation) ? typeInformation : null;
        }

        /// <summary>
        /// Tries to retrieve the type information for a type name / method name combination. 
        /// 
        /// It does 2 lookups:
        /// 1st try: 'typeFullName'
        /// 2nd try: 'typeFullName::simpleMethodName'
        /// 
        /// </summary>
        /// <param name="typeFullName">namespace qualified type name</param>
        /// <param name="simpleMethodName">name of the method</param>
        /// <returns></returns>
        internal static Tuple<string, Type> GetTypeInformationFromTypeCache(string typeFullName, string simpleMethodName)
        {
            return
                GetValue(typeFullName) ??
                GetValue(CreateQualifiedMethodName(typeFullName, simpleMethodName));
        }

        /// <summary>
        /// Returns the fully qualified format for the given method: see typeFullName::methodName
        /// </summary>
        /// <param name="typeFullName">namespace qualified type name</param>
        /// <param name="simpleMethodName">simple name of the method</param>
        /// <returns></returns>
        private static string CreateQualifiedMethodName(string typeFullName, string simpleMethodName)
        {
            return $"{typeFullName}::{simpleMethodName}";
        }


        /// <summary>
        /// Re-initialize.
        /// Unit tests need this when they enable "unsafe" methods -- which will then go in the collection,
        /// and mess up subsequent tests.
        /// </summary>
        internal static void Reset_ForUnitTestsOnly()
        {
            lock (s_locker)
            {
                s_availableStaticMethods = null;
                InitializeAvailableMethods();
            }
        }

        /// <summary>
        /// Fill up the dictionary for first use
        /// </summary>
        private static void InitializeAvailableMethods()
        {
            if (s_availableStaticMethods == null)
            {
                lock (s_locker)
                {
                    if (s_availableStaticMethods == null)
                    {
                        var availableStaticMethods = new ConcurrentDictionary<string, Tuple<string, Type>>(StringComparer.OrdinalIgnoreCase);

                        // Pre declare our common type Tuples
                        var environmentType = new Tuple<string, Type>(null, typeof(Environment));
                        var directoryType = new Tuple<string, Type>(null, typeof(Directory));
                        var fileType = new Tuple<string, Type>(null, typeof(File));
                        var runtimeInformationType = new Tuple<string, Type>(null, typeof(RuntimeInformation));
                        var osPlatformType = new Tuple<string, Type>(null, typeof(OSPlatform));

                        // Make specific static methods available (Assembly qualified type names are *NOT* supported, only null which means mscorlib):
                        availableStaticMethods.TryAdd("System.Environment::ExpandEnvironmentVariables", environmentType);
                        availableStaticMethods.TryAdd("System.Environment::GetEnvironmentVariable", environmentType);
                        availableStaticMethods.TryAdd("System.Environment::GetEnvironmentVariables", environmentType);
                        availableStaticMethods.TryAdd("System.Environment::GetFolderPath", environmentType);
                        availableStaticMethods.TryAdd("System.Environment::GetLogicalDrives", environmentType);

// All the following properties only have getters
#if FEATURE_GET_COMMANDLINE
                        availableStaticMethods.TryAdd("System.Environment::CommandLine", environmentType);
#endif
#if FEATURE_64BIT_ENVIRONMENT_QUERY
                        availableStaticMethods.TryAdd("System.Environment::Is64BitOperatingSystem", environmentType);
                        availableStaticMethods.TryAdd("System.Environment::Is64BitProcess", environmentType);
#endif

                        availableStaticMethods.TryAdd("System.Environment::MachineName", environmentType);
#if FEATURE_OSVERSION
                        availableStaticMethods.TryAdd("System.Environment::OSVersion", environmentType);
#endif
                        availableStaticMethods.TryAdd("System.Environment::ProcessorCount", environmentType);
                        availableStaticMethods.TryAdd("System.Environment::StackTrace", environmentType);
                        availableStaticMethods.TryAdd("System.Environment::SystemDirectory", environmentType);
#if FEATURE_SYSTEMPAGESIZE
                        availableStaticMethods.TryAdd("System.Environment::SystemPageSize", environmentType);
#endif
                        availableStaticMethods.TryAdd("System.Environment::TickCount", environmentType);
#if FEATURE_USERDOMAINNAME
                        availableStaticMethods.TryAdd("System.Environment::UserDomainName", environmentType);
#endif
#if FEATURE_USERINTERACTIVE
                        availableStaticMethods.TryAdd("System.Environment::UserInteractive", environmentType);
#endif
                        availableStaticMethods.TryAdd("System.Environment::UserName", environmentType);
#if FEATURE_DOTNETVERSION
                        availableStaticMethods.TryAdd("System.Environment::Version", environmentType);
#endif
#if FEATURE_WORKINGSET
                        availableStaticMethods.TryAdd("System.Environment::WorkingSet", environmentType);
#endif

                        availableStaticMethods.TryAdd("System.IO.Directory::GetDirectories", directoryType);
                        availableStaticMethods.TryAdd("System.IO.Directory::GetFiles", directoryType);
                        availableStaticMethods.TryAdd("System.IO.Directory::GetLastAccessTime", directoryType);
                        availableStaticMethods.TryAdd("System.IO.Directory::GetLastWriteTime", directoryType);
                        availableStaticMethods.TryAdd("System.IO.Directory::GetParent", directoryType);
                        availableStaticMethods.TryAdd("System.IO.File::Exists", fileType);
                        availableStaticMethods.TryAdd("System.IO.File::GetCreationTime", fileType);
                        availableStaticMethods.TryAdd("System.IO.File::GetAttributes", fileType);
                        availableStaticMethods.TryAdd("System.IO.File::GetLastAccessTime", fileType);
                        availableStaticMethods.TryAdd("System.IO.File::GetLastWriteTime", fileType);
                        availableStaticMethods.TryAdd("System.IO.File::ReadAllText", fileType);

#if FEATURE_CULTUREINFO_GETCULTUREINFO
                        availableStaticMethods.TryAdd("System.Globalization.CultureInfo::GetCultureInfo", new Tuple<string, Type>(null, typeof(CultureInfo))); // user request
#endif
                        availableStaticMethods.TryAdd("System.Globalization.CultureInfo::new", new Tuple<string, Type>(null, typeof(CultureInfo))); // user request
                        availableStaticMethods.TryAdd("System.Globalization.CultureInfo::CurrentUICulture", new Tuple<string, Type>(null, typeof(CultureInfo))); // user request

                        // All static methods of the following are available (Assembly qualified type names are supported):
                        availableStaticMethods.TryAdd("MSBuild", new Tuple<string, Type>(null, typeof(IntrinsicFunctions)));
                        availableStaticMethods.TryAdd("System.Byte", new Tuple<string, Type>(null, typeof(Byte)));
                        availableStaticMethods.TryAdd("System.Char", new Tuple<string, Type>(null, typeof(Char)));
                        availableStaticMethods.TryAdd("System.Convert", new Tuple<string, Type>(null, typeof(Convert)));
                        availableStaticMethods.TryAdd("System.DateTime", new Tuple<string, Type>(null, typeof(DateTime)));
                        availableStaticMethods.TryAdd("System.Decimal", new Tuple<string, Type>(null, typeof(Decimal)));
                        availableStaticMethods.TryAdd("System.Double", new Tuple<string, Type>(null, typeof(Double)));
                        availableStaticMethods.TryAdd("System.Enum", new Tuple<string, Type>(null, typeof(Enum)));
                        availableStaticMethods.TryAdd("System.Guid", new Tuple<string, Type>(null, typeof(Guid)));
                        availableStaticMethods.TryAdd("System.Int16", new Tuple<string, Type>(null, typeof(Int16)));
                        availableStaticMethods.TryAdd("System.Int32", new Tuple<string, Type>(null, typeof(Int32)));
                        availableStaticMethods.TryAdd("System.Int64", new Tuple<string, Type>(null, typeof(Int64)));
                        availableStaticMethods.TryAdd("System.IO.Path", new Tuple<string, Type>(null, typeof(Path)));
                        availableStaticMethods.TryAdd("System.Math", new Tuple<string, Type>(null, typeof(Math)));
                        availableStaticMethods.TryAdd("System.UInt16", new Tuple<string, Type>(null, typeof(UInt16)));
                        availableStaticMethods.TryAdd("System.UInt32", new Tuple<string, Type>(null, typeof(UInt32)));
                        availableStaticMethods.TryAdd("System.UInt64", new Tuple<string, Type>(null, typeof(UInt64)));
                        availableStaticMethods.TryAdd("System.SByte", new Tuple<string, Type>(null, typeof(SByte)));
                        availableStaticMethods.TryAdd("System.Single", new Tuple<string, Type>(null, typeof(Single)));
                        availableStaticMethods.TryAdd("System.String", new Tuple<string, Type>(null, typeof(String)));
                        availableStaticMethods.TryAdd("System.StringComparer", new Tuple<string, Type>(null, typeof(StringComparer)));
                        availableStaticMethods.TryAdd("System.TimeSpan", new Tuple<string, Type>(null, typeof(TimeSpan)));
                        availableStaticMethods.TryAdd("System.Text.RegularExpressions.Regex", new Tuple<string, Type>(null, typeof(Regex)));
                        availableStaticMethods.TryAdd("System.UriBuilder", new Tuple<string, Type>(null, typeof(UriBuilder)));
                        availableStaticMethods.TryAdd("System.Version", new Tuple<string, Type>(null, typeof(Version)));
                        availableStaticMethods.TryAdd("System.Runtime.InteropServices.RuntimeInformation", runtimeInformationType);
                        availableStaticMethods.TryAdd("System.Runtime.InteropServices.OSPlatform", osPlatformType);
                        availableStaticMethods.TryAdd("net.r_eg.IeXod", new Tuple<string, Type>(null, typeof(IntrinsicFunctions)));
                        availableStaticMethods.TryAdd("IeXod", new Tuple<string, Type>(null, typeof(IntrinsicFunctions)));

                        var tToolLocationHelper = new Tuple<string, Type>(typeof(IeXod.Utilities.ToolLocationHelper).AssemblyQualifiedName, null);                        
                        availableStaticMethods.TryAdd("IeXod.Utilities.ToolLocationHelper", tToolLocationHelper);
                        availableStaticMethods.TryAdd("ToolLocationHelper", tToolLocationHelper);
                        availableStaticMethods.TryAdd("net.r_eg.IeXod.Utilities.ToolLocationHelper", tToolLocationHelper);
                        availableStaticMethods.TryAdd("Microsoft.Build.Utilities.ToolLocationHelper", tToolLocationHelper);

                        s_availableStaticMethods = availableStaticMethods;
                    }
                }
            }
        }
    }
}
