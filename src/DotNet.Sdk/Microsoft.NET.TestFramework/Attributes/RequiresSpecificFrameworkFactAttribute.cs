// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP

using Xunit;

namespace Microsoft.NET.TestFramework
{
    public class RequiresSpecificFrameworkFactAttribute : FactAttribute
    {
        public RequiresSpecificFrameworkFactAttribute(string framework)
        {
            if (!EnvironmentInfo.SupportsTargetFramework(framework))
            {
                this.Skip = $"This test requires a shared framework that isn't present: {framework}";
            }
        }
    }
}

#endif
