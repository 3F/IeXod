// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using net.r_eg.IeXod.Evaluation;
using net.r_eg.IeXod.Shared;
using net.r_eg.IeXod.UnitTests;
using Xunit;
using Xunit.Abstractions;

namespace net.r_eg.IeXod.Tasks.UnitTests
{
    public sealed class RegressionTests
    {
        private readonly ITestOutputHelper _output;

        public RegressionTests(ITestOutputHelper output)
        {
            _output = output;
        }

        /// <summary>
        /// Verifies that when a user overrides the BaseIntermediateOutputPath that the build still works.
        /// </summary>
        /// <remarks>This was written because of regression https://github.com/Microsoft/msbuild/issues/1509. </remarks>
        [Fact(Skip = "IeXod. L-164")]
        public void OverrideBaseIntermediateOutputPathSucceeds()
        {
            Project project = ObjectModelHelpers.CreateInMemoryProject($@"
                <Project DefaultTargets=""Build"" xmlns=""msbuildnamespace"" ToolsVersion=""msbuilddefaulttoolsversion"">
                    <Import Project=""$(IeXodBinPath)\Microsoft.Common.props"" />

                    <PropertyGroup>
                        <BaseIntermediateOutputPath>obj\x86\Debug</BaseIntermediateOutputPath>
                    </PropertyGroup>

                    <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />

                    <Target Name=""Build"" />
                </Project>
                ");

            bool result = project.Build(new MockLogger(_output));

            Assert.True(result);
        }

        /// <summary>
        /// Tests fix for https://github.com/microsoft/msbuild/issues/1479.
        /// </summary>
        [ConditionalFact(typeof(NativeMethodsShared), nameof(NativeMethodsShared.IsWindows), Skip = "IeXod. L-139")]
        public void AssemblyAttributesLocation()
        {
            var expectedCompileItems = "a.cs;" + Path.Combine("obj", "Debug", ".NETFramework,Version=v4.0.AssemblyAttributes.cs");

            var project = ObjectModelHelpers.CreateInMemoryProject($@"
<Project>
  <Import Project=""$(IeXodBinPath)\Microsoft.Common.props"" />
  <ItemGroup>
    <Compile Include=""a.cs""/>       
  </ItemGroup>
  <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />

  <Target Name=""CopyFilesToOutputDirectory""/>

  <Target Name=""CoreCompile"">
    <Error Text=""Expected '@(Compile)' == '{expectedCompileItems}'""
           Condition=""'@(Compile)' != '{expectedCompileItems}'""/>
  </Target>
</Project>
");
            var logger = new MockLogger(_output);
            bool result = project.Build(logger);
            Assert.True(result, "Output:" + Environment.NewLine + logger.FullLog);
        }
    }
}
