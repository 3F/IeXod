// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;
using System.Runtime.InteropServices;

namespace Microsoft.NET.TestFramework
{
    public class UnixOnlyTheoryAttribute : TheoryAttribute
    {
        public UnixOnlyTheoryAttribute()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                this.Skip = "This test requires Unix to run";
            }
        }
    }
}
