// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Xunit;

namespace Microsoft.NET.TestFramework
{
    public class CoreMSBuildOnlyTheoryAttribute : TheoryAttribute
    {
        public CoreMSBuildOnlyTheoryAttribute()
        {
            if (TestContext.Current.ToolsetUnderTest.ShouldUseFullFrameworkMSBuild)
            {
                this.Skip = "This test requires Core MSBuild to run";
            }
        }
    }
}
