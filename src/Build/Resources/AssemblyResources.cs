// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace net.r_eg.IeXod.Shared
{
    /// <summary>
    /// This class provides access to the assembly's resources.
    /// FIXME: Since msbuild conatins its weird AssemblyResources defenitions between Shared, Framework, Utilities, Tasks, main code, etc.,
    //         we will temporarily rely on the same mechanism according to the new logical structure. You need review this later.
    /// </summary>
    internal static class AssemblyResources
    {
        internal const string ASM = nameof(IeXod);

        private static readonly Assembly currasm = typeof(AssemblyResources).GetTypeInfo().Assembly;

        // core assembly resources
        private static readonly ResourceManager s_resources = new(ASM + ".Strings", currasm);
        // shared resources
        private static readonly ResourceManager s_sharedResources = new(ASM + ".Strings.shared", currasm);
        // shared resources
        private static readonly ResourceManager s_utilitiesResources = new(ASM + ".Utilities", currasm);

        private static readonly ConcurrentDictionary<string, Assembly> external = new();

        // external tasks resources
        private static readonly Lazy<ResourceManager> s_tasksResources, s_tasksSharedResources;

        /// <summary>
        /// A slot for msbuild.exe to add a resource manager over its own resources, that can also be consulted.
        /// </summary>
        private static ResourceManager s_msbuildExeResourceManager;

        internal static ResourceManager PrimaryResources => s_resources;

        internal static ResourceManager SharedResources => s_sharedResources;

        internal static ResourceManager PrimaryTasksResources => s_tasksResources.Value;

        internal static ResourceManager SharedTasksResources => s_tasksSharedResources.Value;

        /// <summary>
        /// The internals of the Engine are exposed to MSBuild.exe, so they must share the same AssemblyResources class and 
        /// ResourceUtilities class that uses it. To make this possible, MSBuild.exe registers its resources here and they are
        /// normally consulted last. This assumes that there are no duplicated resource ID's between the Engine and MSBuild.exe.
        /// (Actually there are currently two: LoggerCreationError and LoggerNotFoundError.
        /// We can't change the resource ID's this late in the cycle and we sometimes want to load the MSBuild.exe ones,
        /// because they're a little different. So for that purpose we call GetStringLookingInMSBuildExeResourcesFirst() )
        /// </summary>
        internal static void RegisterMSBuildExeResources(ResourceManager manager)
        {
            ErrorUtilities.VerifyThrow(s_msbuildExeResourceManager == null, "Only one extra resource manager");

            s_msbuildExeResourceManager = manager;
        }

        /// <summary>
        /// Loads the specified resource string, either from the assembly's primary resources, or its shared resources.
        /// </summary>
        /// <remarks>This method is thread-safe.</remarks>
        /// <param name="name"></param>
        /// <returns>The resource string, or null if not found.</returns>
        internal static string GetString(string name) => GetString(name, null);

        internal static string GetString(string name, Func<string, string> fallback)
        {
            string resource = GetStringFromEngineResources(name);

            if(resource == null)
            {
                resource = GetStringFromMSBuildExeResources(name);
            }

            if(resource == null && fallback != null)
            {
                resource = fallback(name);
            }

            ErrorUtilities.VerifyThrow(resource != null, "Missing resource '{0}'", name);
            return resource;
        }

        /// <summary>
        /// Loads the specified resource string.
        /// </summary>
        /// <returns>The resource string, or null if not found.</returns>
        internal static string GetStringLookingInMSBuildExeResourcesFirst(string name)
        {
            string resource = GetStringFromMSBuildExeResources(name);

            if (resource == null)
            {
                resource = GetStringFromEngineResources(name);
            }

            return resource;
        }

        internal static string FormatString(string unformatted, params object[] args)
        {
            ErrorUtilities.VerifyThrowArgumentNull(unformatted, nameof(unformatted));

            return ResourceUtilities.FormatString(unformatted, args);
        }

        internal static string FormatResourceString(string resourceName, params object[] args)
        {
            ErrorUtilities.VerifyThrowArgumentNull(resourceName, nameof(resourceName));

            return FormatString(GetString(resourceName), args);
        }

        static AssemblyResources()
        {
            s_tasksResources = new Lazy<ResourceManager>(() => GetResourceManager(ASM + ".Tasks.Strings", ASM + ".Tasks,"));
            s_tasksSharedResources = new Lazy<ResourceManager>(() => GetResourceManager(ASM + ".Tasks.Strings.shared", ASM + ".Tasks,"));
        }

        /// <summary>
        /// Loads the specified resource string, from the Engine or else Shared resources.
        /// </summary>
        /// <returns>The resource string, or null if not found.</returns>
        private static string GetStringFromEngineResources(string name)
        {
            return s_resources.GetString(name, CultureInfo.CurrentUICulture)
                ?? s_sharedResources.GetString(name, CultureInfo.CurrentUICulture)
                ?? s_utilitiesResources.GetString(name, CultureInfo.CurrentUICulture)
                ?? s_tasksResources.Value?.GetString(name, CultureInfo.CurrentUICulture)
                ?? s_tasksSharedResources.Value?.GetString(name, CultureInfo.CurrentUICulture);
        }

        /// <summary>
        /// Loads the specified resource string, from the MSBuild.exe resources.
        /// </summary>
        /// <returns>The resource string, or null if not found.</returns>
        private static string GetStringFromMSBuildExeResources(string name)
        {
            string resource = null;

            if (s_msbuildExeResourceManager != null)
            {
                // Try MSBuild.exe's resources
                resource = s_msbuildExeResourceManager.GetString(name, CultureInfo.CurrentUICulture);
            }

            return resource;
        }

        private static ResourceManager GetResourceManager(string baseName, string assemblyId)
        {
            Assembly asm = FindAssembly(assemblyId);

            return (asm == null) ? null : new ResourceManager
            (
                baseName ?? throw new ArgumentNullException(nameof(baseName)),
                asm
            );
        }

        private static Assembly FindAssembly(string id)
        {
            if(external.TryGetValue(id ?? throw new ArgumentNullException(nameof(id)), out Assembly found))
            {
                return found;
            }

            found = AppDomain.CurrentDomain.GetAssemblies()
                            .FirstOrDefault(a => a.FullName.StartsWith(id));

            if(found != null)
            {
                external[id] = found;
            }
            return found;
        }
    }
}
