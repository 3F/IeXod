﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using net.r_eg.IeXod.Shared.FileSystem;
using System.Reflection;

namespace net.r_eg.IeXod.Shared
{
    internal class BuildEnvironmentHelper
    {
        // Since this class is added as 'link' to shared source in multiple projects,
        // MSBuildConstants.CurrentVisualStudioVersion is not available in all of them.
        private const string CurrentVisualStudioVersion = "16.0";

        // MSBuildConstants.CurrentToolsVersion
        private const string CurrentToolsVersion = "Current";

        /// <summary>
        /// Name of the Visual Studio (and Blend) process.
        // VS ASP intellisense server fails without Microsoft.VisualStudio.Web.Host. Remove when issue fixed: https://devdiv.visualstudio.com/DevDiv/_workitems/edit/574986
        /// </summary>
        private static readonly string[] s_visualStudioProcess = {"DEVENV", "BLEND", "Microsoft.VisualStudio.Web.Host"};

        /// <summary>
        /// Name of the MSBuild process(es)
        /// </summary>
        private static readonly string[] s_msBuildProcess = {"MSBUILD", "MSBUILDTASKHOST"};

        /// <summary>
        /// Name of MSBuild executable files.
        /// </summary>
        private static readonly string[] s_msBuildExeNames = { "MSBuild.exe", "MSBuild.dll" };

        internal static char[] StreamTrimChars { get; } = new[] { ' ', '\r', '\n' };

        /// <summary>
        /// Gets the cached Build Environment instance.
        /// </summary>
        public static BuildEnvironment Instance
        {
            get
            {
                try
                {
                    return BuildEnvironmentHelperSingleton.s_instance;
                }
                catch (TypeInitializationException e)
                {
                    if (e.InnerException != null)
                    {
                        // Throw the error that caused the TypeInitializationException.
                        // (likely InvalidOperationException)
                        throw e.InnerException;
                    }

                    throw;
                }
            }
        }

        /// <summary>
        /// Find the location of MSBuild.exe based on the current environment.
        /// </summary>
        /// <remarks>
        /// This defines the order and precedence for various methods of discovering MSBuild and associated toolsets.
        /// At a high level, an install under Visual Studio is preferred as the user may have SDKs installed to a
        /// specific instance of Visual Studio and build will only succeed if we can discover those. See
        /// https://github.com/Microsoft/msbuild/issues/1461 for details.
        /// </remarks>
        /// <returns>Build environment.</returns>
        private static BuildEnvironment Initialize()
        {
            // See https://github.com/Microsoft/msbuild/issues/1461 for specification of ordering and details.
            var possibleLocations = new Func<BuildEnvironment>[]
            {
                TryFromEnvironmentVariable,
                TryFromVisualStudioProcess,
                TryFromMSBuildProcess,
                TryFromMSBuildAssembly,
                TryFromDevConsole,
                TryFromSetupApi,
                TryFromVSDir,
                TryFromHMSBuild,
                TryFromAppContextBaseDirectory
            };

            foreach (var location in possibleLocations)
            {
                var env = location();
                if (env != null)
                    return env;
            }

            // If we can't find a suitable environment, continue in the 'None' mode. If not running tests,
            // we will use the current running process for the CurrentMSBuildExePath value.  This is likely
            // wrong, but many things use the CurrentMSBuildToolsDirectory value which must be set for basic
            // functionality to work.
            //
            // If we are running tests, then the current running process may be a test runner located in the
            // NuGet packages folder.  So in that case, we use the location of the current assembly, which
            // will be in the output path of the test project, which is what we want.

            string msbuildExePath;
            if (s_runningTests())
            {
                msbuildExePath = typeof(BuildEnvironmentHelper).Assembly.Location;
            }
            else
            {
                msbuildExePath = s_getProcessFromRunningProcess();
            }

            return new BuildEnvironment(
                BuildEnvironmentMode.None,
                msbuildExePath,
                runningTests: s_runningTests(),
                runningInVisualStudio: false,
                visualStudioPath: null);
        }

        private static BuildEnvironment TryFromEnvironmentVariable()
        {
            var msBuildExePath = s_getEnvironmentVariable("IEXOD_PATH_TO_MSBUILD_EXE");

            return msBuildExePath == null
                ? null
                : TryFromMSBuildAssemblyUnderVisualStudio(msBuildExePath, msBuildExePath, true) ?? TryFromStandaloneMSBuildExe(msBuildExePath);
        }

        private static BuildEnvironment TryFromVisualStudioProcess()
        {
            if (!NativeMethodsShared.IsWindows)
                return null;

            var vsProcess = s_getProcessFromRunningProcess();
            if (!IsProcessInList(vsProcess, s_visualStudioProcess)) return null;

            var vsRoot = FileUtilities.GetFolderAbove(vsProcess, 3);
            string msBuildExe = GetMSBuildExeFromVsRoot(vsRoot);

            return new BuildEnvironment(
                BuildEnvironmentMode.VisualStudio,
                msBuildExe,
                runningTests: false,
                runningInVisualStudio: true,
                visualStudioPath: vsRoot);
        }

        private static BuildEnvironment TryFromMSBuildProcess()
        {
            var msBuildExe = s_getProcessFromRunningProcess();
            if (!IsProcessInList(msBuildExe, s_msBuildProcess)) return null;

            // First check if we're in a VS installation
            if (NativeMethodsShared.IsWindows &&
                Regex.IsMatch(msBuildExe, $@".*\\MSBuild\\{CurrentToolsVersion}\\Bin\\.*MSBuild(?:TaskHost)?\.exe", RegexOptions.IgnoreCase))
            {
                return new BuildEnvironment(
                    BuildEnvironmentMode.VisualStudio,
                    msBuildExe,
                    runningTests: false,
                    runningInVisualStudio: false,
                    visualStudioPath: GetVsRootFromMSBuildAssembly(msBuildExe));
            }

            // Standalone mode running in MSBuild.exe
            return new BuildEnvironment(
                BuildEnvironmentMode.Standalone,
                msBuildExe,
                runningTests: false,
                runningInVisualStudio: false,
                visualStudioPath: null);
        }

        private static BuildEnvironment TryFromMSBuildAssembly()
        {
            var buildAssembly = s_getExecutingAssemblyPath();
            if (buildAssembly == null) return null;

            // Check for MSBuild.[exe|dll] next to the current assembly
            var msBuildExe = Path.Combine(FileUtilities.GetFolderAbove(buildAssembly), "MSBuild.exe");
            var msBuildDll = Path.Combine(FileUtilities.GetFolderAbove(buildAssembly), "MSBuild.dll");

            // First check if we're in a VS installation
            var environment = TryFromMSBuildAssemblyUnderVisualStudio(buildAssembly, msBuildExe);
            if (environment != null)
            {
                return environment;
            }

            // We're not in VS, check for MSBuild.exe / dll to consider this a standalone environment.
            string msBuildPath = null;
            if (FileSystems.Default.FileExists(msBuildExe)) msBuildPath = msBuildExe;
            else if (FileSystems.Default.FileExists(msBuildDll)) msBuildPath = msBuildDll;

            if (!string.IsNullOrEmpty(msBuildPath))
            {
                // Standalone mode with toolset
                return new BuildEnvironment(
                    BuildEnvironmentMode.Standalone,
                    msBuildPath,
                    runningTests: s_runningTests(),
                    runningInVisualStudio: false,
                    visualStudioPath: null);
            }

            return null;

        }

        private static BuildEnvironment TryFromMSBuildAssemblyUnderVisualStudio(string msbuildAssembly, string msbuildExe, bool allowLegacyToolsVersion = false)
        {
            string msBuildPathPattern = allowLegacyToolsVersion
                ? $@".*\\MSBuild\\({CurrentToolsVersion}|\d+\.0)\\Bin\\.*"
                : $@".*\\MSBuild\\{CurrentToolsVersion}\\Bin\\.*";

            if (NativeMethodsShared.IsWindows &&
                Regex.IsMatch(msbuildAssembly, msBuildPathPattern, RegexOptions.IgnoreCase))
            {
                // In a Visual Studio path we must have MSBuild.exe
                if (FileSystems.Default.FileExists(msbuildExe))
                {
                    return new BuildEnvironment(
                        BuildEnvironmentMode.VisualStudio,
                        msbuildExe,
                        runningTests: s_runningTests(),
                        runningInVisualStudio: false,
                        visualStudioPath: GetVsRootFromMSBuildAssembly(msbuildExe));
                }
            }

            return null;
        }

        private static BuildEnvironment TryFromDevConsole()
        {
            // VSINSTALLDIR and VisualStudioVersion are set from the Developer Command Prompt.
            var vsInstallDir = s_getEnvironmentVariable("VSINSTALLDIR");
            var vsVersion = s_getEnvironmentVariable("VisualStudioVersion");

            if (string.IsNullOrEmpty(vsInstallDir) || string.IsNullOrEmpty(vsVersion) ||
                vsVersion != CurrentVisualStudioVersion || !FileSystems.Default.DirectoryExists(vsInstallDir)) return null;

            return new BuildEnvironment(
                BuildEnvironmentMode.VisualStudio,
                GetMSBuildExeFromVsRoot(vsInstallDir),
                runningTests: s_runningTests(),
                runningInVisualStudio: false,
                visualStudioPath: vsInstallDir);
        }

        private static BuildEnvironment TryFromSetupApi()
        {
            Version v = new(CurrentVisualStudioVersion);
            var instances = s_getVisualStudioInstances()
                .Where(i => i.Version.Major == v.Major && FileSystems.Default.DirectoryExists(i.Path))
                .ToList();

            if (instances.Count == 0) return null;

            if (instances.Count > 1)
            {
                // TODO: Warn user somehow. We may have picked the wrong one.
            }

            return new BuildEnvironment(
                BuildEnvironmentMode.VisualStudio,
                GetMSBuildExeFromVsRoot(instances[0].Path),
                runningTests: s_runningTests(),
                runningInVisualStudio: false,
                visualStudioPath: instances[0].Path);
        }

        private static BuildEnvironment TryFromVSDir()
        {
            string vsdir = s_getEnvironmentVariable("VSAPPIDDIR");
            if(vsdir == null) return null;

            string vsMSBuild = Path.GetFullPath(Path.Combine(vsdir, @"..\..\MSBuild\"));

            string path = Path.Combine(vsMSBuild, CurrentToolsVersion);
            if(!Directory.Exists(path))
            {
                // legacy tools versions
                path = Directory.EnumerateDirectories(vsMSBuild, "*.0", SearchOption.TopDirectoryOnly).FirstOrDefault();
                if(path == null) return null;
            }

            path = Path.Combine(path, "Bin", "MSBuild.exe");

            return TryFromMSBuildAssemblyUnderVisualStudio(path, path, allowLegacyToolsVersion: true)
                    ?? TryFromStandaloneMSBuildExe(path);
        }

        private static BuildEnvironment TryFromHMSBuild()
        {
            string hmsbuild = s_getHMSBuildPath();
            if(hmsbuild == null) return null;

            Process p = new()
            {
                StartInfo = new ProcessStartInfo(hmsbuild, "-only-path -notamd64")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                }
            };

            p.Start();
            p.WaitForExit();
            if(p.ExitCode != 0) return null;

            string path = p.StandardOutput.ReadToEnd().Trim(StreamTrimChars);

            return string.IsNullOrEmpty(path)
                    ? null
                    : TryFromMSBuildAssemblyUnderVisualStudio(path, path, true)
                        ?? TryFromStandaloneMSBuildExe(path);
        }

        private static BuildEnvironment TryFromAppContextBaseDirectory()
        {
            // Assemblies compiled against anything older than .NET 4.0 won't have a System.AppContext
            // Try the base directory that the assembly resolver uses to probe for assemblies.
            // Under certain scenarios the assemblies are loaded from spurious locations like the NuGet package cache
            // but the toolset files are copied to the app's directory via "contentFiles".

            var appContextBaseDirectory = s_getAppContextBaseDirectory();
            if (string.IsNullOrEmpty(appContextBaseDirectory)) return null;

            // Look for possible MSBuild exe names in the AppContextBaseDirectory
            return s_msBuildExeNames
                .Select((name) => TryFromStandaloneMSBuildExe(Path.Combine(appContextBaseDirectory, name)))
                .FirstOrDefault(env => env != null);
        }

        private static BuildEnvironment TryFromStandaloneMSBuildExe(string msBuildExePath)
        {
            if (!string.IsNullOrEmpty(msBuildExePath) && FileSystems.Default.FileExists(msBuildExePath))
            {
                // MSBuild.exe was found outside of Visual Studio. Assume Standalone mode.
                return new BuildEnvironment(
                    BuildEnvironmentMode.Standalone,
                    msBuildExePath,
                    runningTests: s_runningTests(),
                    runningInVisualStudio: false,
                    visualStudioPath: null);
            }

            return null;
        }

        private static string GetVsRootFromMSBuildAssembly(string msBuildAssembly)
        {
            return FileUtilities.GetFolderAbove(msBuildAssembly,
                Regex.IsMatch(msBuildAssembly, $@"\\Bin\\Amd64\\MSBuild\.exe", RegexOptions.IgnoreCase)
                    ? 5
                    : 4);
        }

        private static string GetMSBuildExeFromVsRoot(string visualStudioRoot)
        {
            return FileUtilities.CombinePaths(
                visualStudioRoot,
                "MSBuild",
                CurrentToolsVersion,
                "Bin",
                IntPtr.Size == 8 ? "amd64" : string.Empty,
                "MSBuild.exe");
        }

        private static bool? _runningTests;
        private static readonly object _runningTestsLock = new object();

        private static bool CheckIfRunningTests()
        {
            if (_runningTests != null)
            {
                return _runningTests.Value;
            }

            lock (_runningTestsLock)
            {
                if (_runningTests != null)
                {
                    return _runningTests.Value;
                }

                //  Check if running tests via the TestInfo class in net.r_eg.IeXod.Framework.
                //  See the comments on the TestInfo class for an explanation of why it works this way.
                var frameworkAssembly = typeof(Framework.ITask).Assembly;
                var testInfoType = frameworkAssembly.GetType("net.r_eg.IeXod.Framework.TestInfo");
                var runningTestsField = testInfoType.GetField("s_runningTests", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

                _runningTests = (bool)runningTestsField.GetValue(null);

                return _runningTests.Value;
            }
        }

        /// <summary>
        /// Returns true if processName appears in the processList
        /// </summary>
        /// <param name="processName">Name of the process</param>
        /// <param name="processList">List of processes to check</param>
        /// <returns></returns>
        private static bool IsProcessInList(string processName, string[] processList)
        {
            var processFileName = Path.GetFileNameWithoutExtension(processName);

            if (string.IsNullOrEmpty(processFileName))
            {
                return false;
            }

            return processList.Any(s =>
                processFileName.Equals(s, StringComparison.OrdinalIgnoreCase));
        }

        private static string GetProcessFromRunningProcess()
        {
#if RUNTIME_TYPE_NETCORE
            // The EntryAssembly property can return null when a managed assembly has been loaded from
            // an unmanaged application (for example, using custom CLR hosting).
            if (AssemblyUtilities.EntryAssembly == null)
            {
                return Process.GetCurrentProcess().MainModule.FileName;
            }

            return AssemblyUtilities.GetAssemblyLocation(AssemblyUtilities.EntryAssembly);
#else
            return Process.GetCurrentProcess().MainModule.FileName;
#endif
        }

        private static string GetExecutingAssemblyPath()
        {
            return FileUtilities.ExecutingAssemblyPath;
        }

        private static string GetAppContextBaseDirectory()
        {
#if !CLR2COMPATIBILITY // Assemblies compiled against anything older than .NET 4.0 won't have a System.AppContext
            return AppContext.BaseDirectory;
#else
            return null;
#endif
        }

        private static string GetEnvironmentVariable(string variable)
        {
            return Environment.GetEnvironmentVariable(variable);
        }

        private static string GetHMSBuildPath()
        {
            string hmsbuild = Path.Combine(BuildEnvironment.ExecutingAssemblyPath, "hMSBuild.bat");

            return File.Exists(hmsbuild) ? hmsbuild : null;
        }

        /// <summary>
        /// Resets the current singleton instance (for testing).
        /// </summary>
        internal static void ResetInstance_ForUnitTestsOnly(Func<string> getProcessFromRunningProcess = null,
            Func<string> getExecutingAssemblyPath = null, Func<string> getAppContextBaseDirectory = null,
            Func<IEnumerable<VisualStudioInstance>> getVisualStudioInstances = null,
            Func<string, string> getEnvironmentVariable = null,
            Func<bool> runningTests = null,
            Func<string> getHMSBuildPath = null)
        {
            s_getProcessFromRunningProcess = getProcessFromRunningProcess ?? GetProcessFromRunningProcess;
            s_getExecutingAssemblyPath = getExecutingAssemblyPath ?? GetExecutingAssemblyPath;
            s_getAppContextBaseDirectory = getAppContextBaseDirectory ?? GetAppContextBaseDirectory;
            s_getVisualStudioInstances = getVisualStudioInstances ?? VisualStudioLocationHelper.GetInstances;
            s_getEnvironmentVariable = getEnvironmentVariable ?? GetEnvironmentVariable;
            s_getHMSBuildPath = getHMSBuildPath ?? GetHMSBuildPath;

            //  Tests which specifically test the BuildEnvironmentHelper need it to be able to act as if it is not running tests
            s_runningTests = runningTests ?? CheckIfRunningTests;

            BuildEnvironmentHelperSingleton.s_instance = Initialize();
        }

        private static Func<string> s_getProcessFromRunningProcess = GetProcessFromRunningProcess;
        private static Func<string> s_getExecutingAssemblyPath = GetExecutingAssemblyPath;
        private static Func<string> s_getAppContextBaseDirectory = GetAppContextBaseDirectory;
        private static Func<IEnumerable<VisualStudioInstance>> s_getVisualStudioInstances = VisualStudioLocationHelper.GetInstances;
        private static Func<string, string> s_getEnvironmentVariable = GetEnvironmentVariable;
        private static Func<string> s_getHMSBuildPath = GetHMSBuildPath;
        private static Func<bool> s_runningTests = CheckIfRunningTests;


        private static class BuildEnvironmentHelperSingleton
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static BuildEnvironmentHelperSingleton()
            { }

            public static BuildEnvironment s_instance = Initialize();
        }
    }

    /// <summary>
    /// Enum which defines which environment / mode MSBuild is currently running.
    /// </summary>
    internal enum BuildEnvironmentMode
    {
        /// <summary>
        /// Running from Visual Studio directly or from MSBuild installed under an instance of Visual Studio.
        /// Toolsets and extensions will be loaded from the Visual Studio instance.
        /// </summary>
        VisualStudio,

        /// <summary>
        /// Running in a standalone toolset mode. All toolsets and extensions paths are relative to the app 
        /// running and not dependent on Visual Studio. (e.g. dotnet CLI, open source clone of our repo)
        /// </summary>
        Standalone,

        /// <summary>
        /// Running without any defined toolsets. Most functionality limited. Likely will not be able to
        /// build or evaluate a project. (e.g. reference to Microsoft.*.dll without a toolset definition
        /// or Visual Studio instance installed).
        /// </summary>
        None
    }

    /// <summary>
    /// Defines the current environment for build tools.
    /// </summary>
    internal class BuildEnvironment
    {
        public BuildEnvironment(BuildEnvironmentMode mode, string currentMSBuildExePath, bool runningTests, bool runningInVisualStudio, string visualStudioPath)
        {
            FileInfo currentMSBuildExeFile = null;
            DirectoryInfo currentToolsDirectory = null;

            Mode = mode;
            RunningTests = runningTests;
            RunningInVisualStudio = runningInVisualStudio;
            CurrentMSBuildExePath = currentMSBuildExePath;
            VisualStudioInstallRootDirectory = visualStudioPath;

            if (!string.IsNullOrEmpty(currentMSBuildExePath))
            {
                currentMSBuildExeFile = new FileInfo(currentMSBuildExePath);
                currentToolsDirectory = currentMSBuildExeFile.Directory;

                CurrentMSBuildToolsDirectory = currentMSBuildExeFile.DirectoryName;
                CurrentMSBuildConfigurationFile = string.Concat(currentMSBuildExePath, ".config");
                MSBuildToolsDirectory32 = CurrentMSBuildToolsDirectory;
                MSBuildToolsDirectory64 = CurrentMSBuildToolsDirectory;
            }

            // We can't detect an environment, don't try to set other paths.
            if (mode == BuildEnvironmentMode.None || currentMSBuildExeFile == null || currentToolsDirectory == null)
                return;

            // Check to see if our current folder is 'amd64'
            bool runningInAmd64 = string.Equals(currentToolsDirectory.Name, "amd64", StringComparison.OrdinalIgnoreCase);

            var msBuildExeName = currentMSBuildExeFile.Name;
            var folderAbove = currentToolsDirectory.Parent?.FullName;

            if (folderAbove != null)
            {
                // Calculate potential paths to other architecture MSBuild.exe
                var potentialAmd64FromX86 = FileUtilities.CombinePaths(CurrentMSBuildToolsDirectory, "amd64", msBuildExeName);
                var potentialX86FromAmd64 = Path.Combine(folderAbove, msBuildExeName);

                // Check for existence of an MSBuild file. Note this is not necessary in a VS installation where we always want to
                // assume the correct layout.
                var existsCheck = mode == BuildEnvironmentMode.VisualStudio ? new Func<string, bool>(_ => true) : File.Exists;

                // Running in amd64 folder and the X86 path is valid
                if (runningInAmd64 && existsCheck(potentialX86FromAmd64))
                {
                    MSBuildToolsDirectory32 = folderAbove;
                    MSBuildToolsDirectory64 = CurrentMSBuildToolsDirectory;
                }
                // Not running in amd64 folder and the amd64 path is valid
                else if (!runningInAmd64 && existsCheck(potentialAmd64FromX86))
                {
                    MSBuildToolsDirectory32 = CurrentMSBuildToolsDirectory;
                    MSBuildToolsDirectory64 = Path.Combine(CurrentMSBuildToolsDirectory, "amd64");
                }
            }

            MSBuildExtensionsPath = mode == BuildEnvironmentMode.VisualStudio
                ? Path.Combine(VisualStudioInstallRootDirectory, "MSBuild")
                : MSBuildToolsDirectory32;
        }

        internal static string ExecutingAssemblyPath { get; }
            = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        internal BuildEnvironmentMode Mode { get; }

        /// <summary>
        /// Gets the flag that indicates if we are running in a test harness.
        /// </summary>
        internal bool RunningTests { get; }

        /// <summary>
        /// Returns true when the entry point application is Visual Studio.
        /// </summary>
        internal bool RunningInVisualStudio { get; }

        /// <summary>
        /// Path to the MSBuild 32-bit tools directory.
        /// </summary>
        internal string MSBuildToolsDirectory32 { get; }

        /// <summary>
        /// Path to the MSBuild 64-bit (AMD64) tools directory.
        /// </summary>
        internal string MSBuildToolsDirectory64 { get; }

        /// <summary>
        /// Path to the Sdks folder for this MSBuild instance.
        /// </summary>
        internal string MSBuildSDKsPath
        {
            get
            {
                string defaultSdkPath;

                if (VisualStudioInstallRootDirectory != null)
                {
                    // Can't use the N-argument form of Combine because it doesn't exist on .NET 3.5
                    defaultSdkPath = FileUtilities.CombinePaths(VisualStudioInstallRootDirectory, "MSBuild", "Sdks");
                }
                else
                {
                    defaultSdkPath = Path.Combine(CurrentMSBuildToolsDirectory, "Sdks");
                }

                // Allow an environment-variable override of the default SDK location
                return Environment.GetEnvironmentVariable("MSBuildSDKsPath") ?? defaultSdkPath;
            }
        }

        /// <summary>
        /// Full path to the current MSBuild configuration file.
        /// </summary>
        internal string CurrentMSBuildConfigurationFile { get; }

        /// <summary>
        /// Full path to current MSBuild.exe.
        /// <remarks>
        /// This path is likely not the current running process. We may be inside
        /// Visual Studio or a test harness. In that case this will point to the
        /// version of MSBuild found to be associated with the current environment.
        /// </remarks>
        /// </summary>
        internal string CurrentMSBuildExePath { get; private set; }

        /// <summary>
        /// Full path to the current MSBuild tools directory. This will be 32-bit unless
        /// we're executing from the 'AMD64' folder.
        /// </summary>
        internal string CurrentMSBuildToolsDirectory { get; }

        /// <summary>
        /// Path to the root Visual Studio install directory
        /// (e.g. 'C:\Program Files (x86)\Microsoft Visual Studio\Preview\Enterprise')
        /// </summary>
        internal string VisualStudioInstallRootDirectory { get; }

        /// <summary>
        /// MSBuild extensions path. On Standalone this defaults to the MSBuild folder. In
        /// VisualStudio mode this folder will be %VSINSTALLDIR%\MSBuild.
        /// </summary>
        internal string MSBuildExtensionsPath { get; set; }

        /// <summary>
        /// Path to the IeXod directory.
        /// </summary>
        internal string IeXodBinPath => ExecutingAssemblyPath;
    }
}
