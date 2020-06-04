﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if FEATURE_ASSEMBLYLOADCONTEXT

using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Shared;
using net.r_eg.IeXod.Utilities;
using System.Runtime.Loader;

namespace net.r_eg.IeXod.UnitTests
{
    public class ValidateAssemblyLoadContext : Task
    {
        public override bool Execute()
        {
            var thisLoadContext = AssemblyLoadContext.GetLoadContext(typeof(ValidateAssemblyLoadContext).Assembly);

            // The straightforward implementation of this check:
            //   if (thisLoadContext is MSBuildLoadContext context)
            // fails here because MSBuildLoadContext (in this test assembly) is from MSBuild.exe via
            // IVT, but the one that actually gets used for task isolation is in net.r_eg.IeXod.dll.
            // This probably doesn't need to be how it is forever: https://github.com/microsoft/msbuild/issues/5041
            if (thisLoadContext.GetType().FullName == typeof(MSBuildLoadContext).FullName)
            {
#if NETCOREAPP && !NETCOREAPP2_1 // TODO: enable this functionality when targeting .NET Core 3.0+
                if (!thisLoadContext.Name.EndsWith(typeof(ValidateAssemblyLoadContext).Assembly.GetName().Name + ".dll"))
                {
                    Log.LogError($"Unexpected AssemblyLoadContext name: \"{thisLoadContext.Name}\", but the current executing assembly was {typeof(ValidateAssemblyLoadContext).Assembly.GetName().Name}");
                }
                else
                {
                    Log.LogMessage(MessageImportance.High, $"Task {nameof(ValidateAssemblyLoadContext)} loaded in AssemblyLoadContext named {thisLoadContext.Name}");
                }
#endif
            }
            else
            {
                Log.LogError($"Load context was a {thisLoadContext.GetType().AssemblyQualifiedName} instead of an {typeof(MSBuildLoadContext).AssemblyQualifiedName}");
            }

            return !Log.HasLoggedErrors;
        }
    }
}
#endif
