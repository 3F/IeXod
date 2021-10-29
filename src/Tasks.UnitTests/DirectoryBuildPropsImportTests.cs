﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace net.r_eg.IeXod.UnitTests
{
    /// <summary>
    /// Tests that Microsoft.Common.props successfully imports a directory build project in the directory tree of the project being built.
    /// </summary>
    sealed public class DirectoryBuildPropsImportTests : DirectoryBuildProjectImportTestBase
    {
        protected override string CustomBuildProjectFile => "customBuild.props";

        protected override string DirectoryBuildProjectBasePathPropertyName => "_DirectoryBuildPropsBasePath";

        protected override string DirectoryBuildProjectFile => "Directory.Build.props";

        protected override string DirectoryBuildProjectFilePropertyName => "_DirectoryBuildPropsFile";

        protected override string DirectoryBuildProjectPathPropertyName => "DirectoryBuildPropsPath";

        protected override string ImportDirectoryBuildProjectPropertyName => "ImportDirectoryBuildProps";
    }
}