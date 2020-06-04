// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.Evaluation;
using System;
using Xunit;

namespace net.r_eg.IeXod.UnitTests
{
    sealed public class CreateProperty_Tests : IDisposable
    {
        public CreateProperty_Tests()
        {
            ProjectCollection.GlobalProjectCollection.UnloadAllProjects();
        }

        public void Dispose()
        {
            ProjectCollection.GlobalProjectCollection.UnloadAllProjects();
        }

        /// <summary>
        /// Make sure that I can use the CreateProperty task to blank out a property value.
        /// </summary>
        [Fact]
        public void CreateBlankProperty()
        {
            MockLogger logger = ObjectModelHelpers.BuildProjectExpectSuccess(@"

                    <Project ToolsVersion=`msbuilddefaulttoolsversion` xmlns=`msbuildnamespace`>
                        <PropertyGroup>
                            <NumberOfProcessors>Twenty</NumberOfProcessors>
                        </PropertyGroup>

                        <Target Name=`Build`>
                            <CreateProperty Value=``>
                                <Output PropertyName=`NumberOfProcessors` TaskParameter=`Value`/>
                            </CreateProperty>
                            <Message Text=`NumberOfProcessors='$(NumberOfProcessors)'`/>
                        </Target>
                    </Project>

                ");

            logger.AssertLogContains("NumberOfProcessors=''");
        }

        /// <summary>
        /// Make sure that I can use the CreateProperty task to create a property
        /// that has a parsable semicolon in it.
        /// </summary>
        [Fact]
        public void CreatePropertyWithSemicolon()
        {
            MockLogger logger = ObjectModelHelpers.BuildProjectExpectSuccess(@"

                    <Project ToolsVersion=`msbuilddefaulttoolsversion` xmlns=`msbuildnamespace`>
                        <Target Name=`Build`>
                            <CreateProperty Value=`Clean ; Build`>
                                <Output PropertyName=`TargetsToRunLaterProperty` TaskParameter=`Value`/>
                            </CreateProperty>
                            <Message Text=`TargetsToRunLaterProperty = $(TargetsToRunLaterProperty)`/>
                            <CreateItem Include=`$(TargetsToRunLaterProperty)`>
                                <Output ItemName=`TargetsToRunLaterItem` TaskParameter=`Include`/>
                            </CreateItem>
                            <Message Text=`TargetsToRunLaterItem = @(TargetsToRunLaterItem,'----')`/>
                        </Target>
                    </Project>

                ");

            logger.AssertLogContains("TargetsToRunLaterProperty = Clean;Build");
            logger.AssertLogContains("TargetsToRunLaterItem = Clean----Build");
        }

        /// <summary>
        /// Make sure that I can use the CreateProperty task to create a property
        /// that has a literal semicolon in it.
        /// </summary>
        [Fact]
        public void CreatePropertyWithLiteralSemicolon()
        {
            MockLogger logger = ObjectModelHelpers.BuildProjectExpectSuccess(@"

                    <Project ToolsVersion=`msbuilddefaulttoolsversion` xmlns=`msbuildnamespace`>
                        <Target Name=`Build`>
                            <CreateProperty Value=`Clean%3BBuild`>
                                <Output PropertyName=`TargetsToRunLaterProperty` TaskParameter=`Value`/>
                            </CreateProperty>
                            <Message Text=`TargetsToRunLaterProperty = $(TargetsToRunLaterProperty)`/>
                            <CreateItem Include=`$(TargetsToRunLaterProperty)`>
                                <Output ItemName=`TargetsToRunLaterItem` TaskParameter=`Include`/>
                            </CreateItem>
                            <Message Text=`TargetsToRunLaterItem = @(TargetsToRunLaterItem,'----')`/>
                        </Target>
                    </Project>

                ");

            logger.AssertLogContains("TargetsToRunLaterProperty = Clean;Build");
            logger.AssertLogContains("TargetsToRunLaterItem = Clean;Build");
        }
    }
}
