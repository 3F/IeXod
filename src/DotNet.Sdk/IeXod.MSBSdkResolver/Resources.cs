// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using System.Reflection;
using System.Resources;

#if SDKRESOLVER_AS_EXTDLL
using System.Diagnostics;
#else
using net.r_eg.IeXod.Shared;
#endif

namespace net.r_eg.IeXod
{
    /// <summary>
    /// Original dotnet sdk project uses Arcade.Sdk for generating code from current Strings.resx.
    /// We're not using Arcade.Sdk but we're still using XliffTasks for generating specified XlfLanguages.
    /// TODO: So we just follow the main msbuild project that also used manual implementation (AssemblyResource files) as you can see here.
    /// </summary>
    internal static class Resources
    {
#if SDKRESOLVER_AS_EXTDLL
        private static readonly string asmname = typeof(Resources).Namespace;
#else
        private static readonly string asmname = AssemblyResources.ASM;
#endif

        /// <summary>
        /// Actual source of the resource string we'll be reading.
        /// </summary>
        private static readonly ResourceManager s_resources = new ResourceManager(asmname + ".MSBSdkResolver", typeof(Resources).GetTypeInfo().Assembly);

        /// <summary>
        /// Loads the specified resource string, either from the assembly's primary resources, or its shared resources.
        /// </summary>
        /// <remarks>This method is thread-safe.</remarks>
        /// <returns>The resource string, or null if not found.</returns>
        internal static string GetString(string name)
        {
            // NOTE: the ResourceManager.GetString() method is thread-safe
            string resource = s_resources.GetString(name, CultureInfo.CurrentUICulture);

#if SDKRESOLVER_AS_EXTDLL
            Debug.Assert(resource != null, "Missing resource '{0}'", name);
#else
            ErrorUtilities.VerifyThrow(resource != null, "Missing resource '{0}'", name);
#endif

            return resource;
        }
    }
}
