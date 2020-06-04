// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;

using net.r_eg.IeXod.Framework;
using Shouldly;
using Xunit;

namespace net.r_eg.IeXod.UnitTests
{
    public class AttributeTests
    {
        /// <summary>
        /// Test RequiredRuntimeAttribute
        /// </summary>
        [Fact]
        public void RequiredRuntimeAttribute()
        {

            RequiredRuntimeAttribute attribute =
                typeof(X).GetTypeInfo().GetCustomAttribute<RequiredRuntimeAttribute>();

            attribute.RuntimeVersion.ShouldBe("v5");
        }

        [Fact]
        public void OutputAttribute()
        {
            OutputAttribute attribute =
                typeof(X).GetMember("TestValue2", BindingFlags.NonPublic | BindingFlags.Static)[0].GetCustomAttribute<OutputAttribute>();
            attribute.ShouldNotBeNull();
        }

        [Fact]
        public void RequiredAttribute()
        {
            RequiredAttribute attribute =
                typeof(X).GetMember("TestValue", BindingFlags.NonPublic | BindingFlags.Static)[0].GetCustomAttribute<RequiredAttribute>();
            attribute.ShouldNotBeNull();
        }
    }

    /// <summary>
    /// Sample class with RequiredRuntimeAttribute on it
    /// </summary>
    [RequiredRuntime("v5")]
    internal static class X
    {
        [Required]
        internal static bool TestValue
        {
            get
            {
                return true;
            }
        }

        [Output]
        internal static bool TestValue2
        {
            get
            {
                return true;
            }
        }
    }
}






