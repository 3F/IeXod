// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.BuildEngine.Shared;

namespace net.r_eg.IeXod.BuildEngine
{
    /// <summary>
    /// Internal enum for distinguishing between cache content types
    /// </summary>
    internal enum CacheContentType
    {
        // Cached build results for targets - only accessible internally from the engine
        BuildResults = 0,

        // Items cached from tasks
        Items = 1,

        // Properties cached from tasks
        Properties = 2,
        LastContentTypeIndex = 2
    }

    /// <summary>
    /// This class is responsible for maintaining the set of object 
    /// cached during a build session. This class is not thread safe and 
    /// is intended to be used from the Engine thread.
    /// </summary>
    internal class CacheManager
    {
        #region Constructors
        internal CacheManager(string defaultVersion)
        {
            cacheContents = new Hashtable[(int)CacheContentType.LastContentTypeIndex + 1];

            for (int i = 0; i < (int)CacheContentType.LastContentTypeIndex + 1; i++)
            {
                cacheContents[i] = new Hashtable(StringComparer.OrdinalIgnoreCase);
            }

            this.defaultToolsVersion = defaultVersion;
        }

        #endregion

        #region Properties
        #endregion

        #region Methods
        private CacheScope GetCacheScopeIfExists(string scopeName, BuildPropertyGroup scopeProperties, string scopeToolsVersion, CacheContentType cacheContentType)
        {
            CacheScope cacheScope = null;

            // Default the version to the default engine version
            if (scopeToolsVersion == null)
            {
                scopeToolsVersion = defaultToolsVersion;
            }

            // Retrieve list of scopes by this name
            if (cacheContents[(int)cacheContentType].ContainsKey(scopeName))
            {
                List<CacheScope> scopesByName = (List<CacheScope>)cacheContents[(int)cacheContentType][scopeName];

                // If the list exists search for matching scope properties otherwise create the list
                if (scopesByName != null)
                {
                    lock (cacheManagerLock)
                    {
                        for (int i = 0; i < scopesByName.Count; i++)
                        {
                            if (scopesByName[i].ScopeProperties.IsEquivalent(scopeProperties) && (String.Compare(scopeToolsVersion, scopesByName[i].ScopeToolsVersion, StringComparison.OrdinalIgnoreCase) == 0))
                            {
                                cacheScope = scopesByName[i];
                                break;
                            }
                        }
                    }
                }
            }

            return cacheScope;
        }

        /// <summary>
        /// This method return a cache scope with particular name and properties. If the cache
        /// scope doesn't exist it will be created. This method is thread safe.
        /// </summary>
        internal CacheScope GetCacheScope(string scopeName, BuildPropertyGroup scopeProperties, string scopeToolsVersion, CacheContentType cacheContentType)
        {
            // If the version is not specified default to the engine version
            if (scopeToolsVersion == null)
            {
                scopeToolsVersion = defaultToolsVersion;
            }

            // Retrieve the cache scope if it exists
            CacheScope cacheScope = GetCacheScopeIfExists(scopeName, scopeProperties, scopeToolsVersion, cacheContentType);
            // If the scope doesn't exist create it
            if (cacheScope == null)
            {
                lock (cacheManagerLock)
                {
                    cacheScope = GetCacheScopeIfExists(scopeName, scopeProperties, scopeToolsVersion, cacheContentType);
                    
                    if (cacheScope == null)
                    {
                        // If the list of scopes doesn't exist create it
                        if (!cacheContents[(int)cacheContentType].ContainsKey(scopeName))
                        {
                            cacheContents[(int)cacheContentType].Add(scopeName, new List<CacheScope>());
                        }
                        // Create the scope and add it to the list
                        List<CacheScope> scopesByName = (List<CacheScope>)cacheContents[(int)cacheContentType][scopeName];
                        cacheScope = new CacheScope(scopeName, scopeProperties, scopeToolsVersion);
                        scopesByName.Add(cacheScope);
                    }
                }
            }

            return cacheScope;
        }

        /// <summary>
        /// Sets multiple cache entries for the given scope
        /// </summary>
        /// <param name="entries"></param>
        /// <param name="scopeName"></param>
        /// <param name="scopeProperties"></param>
        internal void SetCacheEntries(CacheEntry[] entries, string scopeName, BuildPropertyGroup scopeProperties, string scopeToolsVersion, CacheContentType cacheContentType)
        {
            // If the list exists search for matching scope properties otherwise create the list
            CacheScope cacheScope = GetCacheScope(scopeName, scopeProperties, scopeToolsVersion, cacheContentType);

            // Add the entry to the right scope
            cacheScope.AddCacheEntries(entries);
        }

        /// <summary>
        /// Gets multiple cache entries from the given scope.
        /// </summary>
        /// <param name="names"></param>
        /// <param name="scopeName"></param>
        /// <param name="scopeProperties"></param>
        /// <returns></returns>
        internal CacheEntry[] GetCacheEntries(string[] names, string scopeName, BuildPropertyGroup scopeProperties, string scopeToolsVersion, CacheContentType cacheContentType)
        {
            CacheScope cacheScope = GetCacheScopeIfExists(scopeName, scopeProperties, scopeToolsVersion, cacheContentType);

            if (cacheScope != null)
            {
                return cacheScope.GetCacheEntries(names);
            }

            return new CacheEntry[names.Length];
        }

        /// <summary>
        /// This method get a result from the cache if every target is cached.
        /// If any of the target are not present in the cache null is returned. This method is not thread safe.
        /// </summary>
        internal BuildResult GetCachedBuildResult(BuildRequest buildRequest, out ArrayList actuallyBuiltTargets)
        {
            actuallyBuiltTargets = null;

            if (!buildRequest.UseResultsCache)
            {
                return null;
            }

            // Retrieve list of scopes by this name
            string projectName = buildRequest.ProjectToBuild == null ?
                                 buildRequest.ProjectFileName : buildRequest.ProjectToBuild.FullFileName;

            // If the list exists search for matching scope properties otherwise create the list
            CacheScope cacheScope = GetCacheScopeIfExists(projectName, buildRequest.GlobalProperties, buildRequest.ToolsetVersion, CacheContentType.BuildResults);

            // If there is no cache entry for this project return null
            if (cacheScope == null)
            {
                return null;
            }

            return cacheScope.GetCachedBuildResult(buildRequest, out actuallyBuiltTargets);
        }

        /// <summary>
        /// Clear a particular scope
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="buildPropertyGroup"></param>
        /// <param name="toolsVersion"></param>
        internal void ClearCacheScope(string projectName, BuildPropertyGroup buildPropertyGroup, string toolsVersion, CacheContentType cacheContentType)
        {
            // Retrieve list of scopes by this name
            if (cacheContents[(int)cacheContentType].ContainsKey(projectName))
            {
                List<CacheScope> scopesByName = (List<CacheScope>)cacheContents[(int)cacheContentType][projectName];

                // If the list exists search for matching scope properties otherwise create the list
                if (scopesByName != null)
                {
                    // If the version is not specified default to the engine version
                    if (toolsVersion == null)
                    {
                        toolsVersion = defaultToolsVersion;
                    }

                    lock (cacheManagerLock)
                    {
                        for (int i = 0; i < scopesByName.Count; i++)
                        {
                            if (scopesByName[i].ScopeProperties.IsEquivalent(buildPropertyGroup) && (String.Compare(toolsVersion, scopesByName[i].ScopeToolsVersion, StringComparison.OrdinalIgnoreCase) == 0))
                            {
                                scopesByName.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Clears the whole contents of the cache.
        /// </summary>
        internal void ClearCache()
        {
            // Abandon the old cache contents
            for (int i = 0; i < (int)CacheContentType.LastContentTypeIndex + 1; i++)
            {
                cacheContents[i] = new Hashtable(StringComparer.OrdinalIgnoreCase);
            }
        }

        #endregion

        #region Data
        // Array of cache contents per namespace
        private Hashtable[] cacheContents;
        // Lock object for the cache manager
        private object cacheManagerLock = new object();
        // The default toolset version
        private string defaultToolsVersion;
        #endregion
    }
}
