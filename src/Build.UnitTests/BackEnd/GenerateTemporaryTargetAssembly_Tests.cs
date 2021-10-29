// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using net.r_eg.IeXod.UnitTests;
using Shouldly;
using System.Linq;
using System.Reflection;
using Xunit;

namespace net.r_eg.IeXod.Engine.UnitTests.BackEnd
{
    sealed public class GenerateTemporaryTargetAssembly_Tests
    {
        [Fact]
        public void FailsWithOnlyTargetErrors()
        {
            using (TestEnvironment testenv = TestEnvironment.Create())
            {
                TransientTestFile otherproj = testenv.CreateFile("otherproj.csproj", @"
<Project>
    <Target Name=""ErrorTask"">
        <Error Text=""Task successfully failed."" />
    </Target>
</Project>");
                MockLogger logger = ObjectModelHelpers.BuildProjectExpectFailure(@$"
<Project>
    <UsingTask TaskName=""FailingBuilderTask"" AssemblyFile=""{Assembly.GetExecutingAssembly().Location}"" />
    <Target Name=""MyTarget"">
        <FailingBuilderTask CurrentProject=""{otherproj.Path}"" />
    </Target>
</Project>");
                logger.ErrorCount.ShouldBe(1);
                logger.Errors.First().Message.ShouldBe("Task successfully failed.");
            }
        }
    }
}
