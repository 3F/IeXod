// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using net.r_eg.IeXod.BackEnd.Logging;
using net.r_eg.IeXod.Construction;
using net.r_eg.IeXod.Evaluation;
using net.r_eg.IeXod.Sdk;
using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod.BackEnd.SdkResolution
{
    internal class SdkResolverLoader
    {
#if FEATURE_ASSEMBLYLOADCONTEXT
        private readonly CoreClrAssemblyLoader _loader = new CoreClrAssemblyLoader();
#endif

        internal virtual IList<SdkResolver> LoadResolvers(LoggingContext loggingContext, ElementLocation location, SdkEnv sdkEnv)
        {
            ProjectToolsOptions options = sdkEnv.toolsOptions ?? ProjectToolsOptions.Default;

            var resolvers = InitSdkResolvers
            (
                new List<SdkResolver>(options.SdkResolvers)
                {
                    new DefaultSdkResolver() // Always add the default resolver
                },
                options
            );

            List<string> potentialResolvers = new();
            foreach(string sdkpath in options.SdkResolversPaths)
            {
                potentialResolvers.AddRange(FindPotentialSdkResolvers(sdkpath, location));
            }

            // since FindPotentialSdkResolvers override is possible
            potentialResolvers.AddRange(FindPotentialSdkResolvers(null, location));

            foreach(string potentialResolver in potentialResolvers)
            {
                LoadResolvers(potentialResolver, loggingContext, location, resolvers);
            }

            return Order(resolvers);
        }

        /// <summary>
        ///     Find all files that are to be considered SDK Resolvers. Pattern will match
        ///     Root\SdkResolver\(ResolverName)\(ResolverName).dll.
        /// </summary>
        /// <param name="rootFolder"></param>
        /// <returns></returns>
        internal virtual IList<string> FindPotentialSdkResolvers(string rootFolder, ElementLocation location)
        {
            var assembliesList = new List<string>();

            if (string.IsNullOrEmpty(rootFolder) || !FileUtilities.DirectoryExistsNoThrow(rootFolder))
            {
                return assembliesList;
            }

            foreach (var subfolder in new DirectoryInfo(rootFolder).GetDirectories())
            {
                var assembly = Path.Combine(subfolder.FullName, $"{subfolder.Name}.dll");
                var manifest = Path.Combine(subfolder.FullName, $"{subfolder.Name}.xml");

                var assemblyAdded = TryAddAssembly(assembly, assembliesList);
                if (!assemblyAdded)
                {
                    assemblyAdded = TryAddAssemblyFromManifest(manifest, subfolder.FullName, assembliesList, location);
                }

                if (!assemblyAdded)
                {
                    ProjectFileErrorUtilities.ThrowInvalidProjectFile(new BuildEventFileInfo(location), "SdkResolverNoDllOrManifest", subfolder.FullName);
                }
            }

            return assembliesList;
        }

        internal List<SdkResolver> Order(List<SdkResolver> resolvers) => resolvers.OrderBy(t => t.Priority).ToList();

        internal virtual List<SdkResolver> GetPredefinedSdkResolvers()
        {
            return new List<SdkResolver>()
            {
#if !SDKRESOLVER_AS_EXTDLL
                new MSBSdkResolver()
#endif
            };
        }

        private bool TryAddAssemblyFromManifest(string pathToManifest, string manifestFolder, List<string> assembliesList, ElementLocation location)
        {
            if (!string.IsNullOrEmpty(pathToManifest) && !FileUtilities.FileExistsNoThrow(pathToManifest)) return false;

            string path = null;

            try
            {
                // <SdkResolver>
                //   <Path>...</Path>
                // </SdkResolver>
                var manifest = SdkResolverManifest.Load(pathToManifest);

                if (manifest == null || string.IsNullOrEmpty(manifest.Path))
                {
                    ProjectFileErrorUtilities.ThrowInvalidProjectFile(new BuildEventFileInfo(location), "SdkResolverDllInManifestMissing", pathToManifest, string.Empty);
                }

                path = FileUtilities.FixFilePath(manifest.Path);
            }
            catch (XmlException e)
            {
                // Note: Not logging e.ToString() as most of the information is not useful, the Message will contain what is wrong with the XML file.
                ProjectFileErrorUtilities.ThrowInvalidProjectFile(new BuildEventFileInfo(location), e, "SdkResolverManifestInvalid", pathToManifest, e.Message);
            }

            if (!Path.IsPathRooted(path))
            {
                path = Path.Combine(manifestFolder, path);
                path = Path.GetFullPath(path);
            }

            if (!TryAddAssembly(path, assembliesList))
            {
                ProjectFileErrorUtilities.ThrowInvalidProjectFile(new BuildEventFileInfo(location), "SdkResolverDllInManifestMissing", pathToManifest, path);
            }

            return true;
        }

        private bool TryAddAssembly(string assemblyPath, List<string> assembliesList)
        {
            if (string.IsNullOrEmpty(assemblyPath) || !FileUtilities.FileExistsNoThrow(assemblyPath)) return false;

            assembliesList.Add(assemblyPath);
            return true;
        }

        protected virtual IEnumerable<Type> GetResolverTypes(Assembly assembly)
        {
            return assembly.ExportedTypes
                .Select(type => new {type, info = type.GetTypeInfo()})
                .Where(t => t.info.IsClass && t.info.IsPublic && !t.info.IsAbstract && typeof(SdkResolver).IsAssignableFrom(t.type))
                .Select(t => t.type);
        }

        protected virtual Assembly LoadResolverAssembly(string resolverPath, LoggingContext loggingContext, ElementLocation location)
        {
#if !FEATURE_ASSEMBLYLOADCONTEXT
            return Assembly.LoadFrom(resolverPath);
#else
            return _loader.LoadFromPath(resolverPath);
#endif
        }

        protected virtual void LoadResolvers(string resolverPath, LoggingContext loggingContext, ElementLocation location, List<SdkResolver> resolvers)
        {
            Assembly assembly;
            try
            {
                assembly = LoadResolverAssembly(resolverPath, loggingContext, location);
            }
            catch (Exception e)
            {
                ProjectFileErrorUtilities.ThrowInvalidProjectFile(new BuildEventFileInfo(location), e, "CouldNotLoadSdkResolverAssembly", resolverPath, e.Message);

                return;
            }

            foreach (Type type in GetResolverTypes(assembly))
            {
                try
                {
                    resolvers.Add((SdkResolver)Activator.CreateInstance(type));
                }
                catch (TargetInvocationException e)
                {
                    // .NET wraps the original exception inside of a TargetInvocationException which masks the original message
                    // Attempt to get the inner exception in this case, but fall back to the top exception message
                    string message = e.InnerException?.Message ?? e.Message;

                    ProjectFileErrorUtilities.ThrowInvalidProjectFile(new BuildEventFileInfo(location), e.InnerException ?? e, "CouldNotLoadSdkResolver", type.Name, message);
                }
                catch (Exception e)
                {
                    ProjectFileErrorUtilities.ThrowInvalidProjectFile(new BuildEventFileInfo(location), e, "CouldNotLoadSdkResolver", type.Name, e.Message);
                }
            }
        }

        private List<SdkResolver> InitSdkResolvers(List<SdkResolver> resolvers, ProjectToolsOptions options)
        {
            if(options.DisablePredefinedSdkResolvers) return resolvers;

            resolvers.AddRange(GetPredefinedSdkResolvers());
            return resolvers;
        }
    }
}
