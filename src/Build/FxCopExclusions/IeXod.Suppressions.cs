﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// FxCop Suppression file
// To Use:
// Add module level suppressions to this file to have them suppressed in the assembly
//

using System.Diagnostics.CodeAnalysis;

#if CODE_ANALYSIS
[module: SuppressMessage("Microsoft.Design","CA1020:AvoidNamespacesWithFewTypes", Scope="namespace", Target="net.r_eg.IeXod.Debugging", Justification="This deserves its own namespace")]
[module: SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", Scope="resource", Target="net.r_eg.IeXod.Strings.resources", MessageId="itemname", Justification="itemname is spelled correctly")]
[module: SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", Scope="resource", Target="net.r_eg.IeXod.Strings.resources", MessageId="sln", Justification="sln is the extension for a solution")]
[module: SuppressMessage("Microsoft.Design","CA1032:ImplementStandardExceptionConstructors",Justification="We require this constructor for deserialization")]
[module: SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames", Justification="We delay sign our assemblies.")]
[module: SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", Scope="resource", Target="net.r_eg.IeXod.Strings.resources", MessageId="precompilation", Justification="precompilation is correctly spelled.")]
[module: SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", Scope="resource", Target="net.r_eg.IeXod.Strings.resources", MessageId="devenv", Justification="devenv is correctly spelled.")]
[module: SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", Scope="namespace", Target="net.r_eg.IeXod.Shared", MessageId="Shared")]
[module: SuppressMessage("Microsoft.MSInternal", "CA905:SystemAndMicrosoftNamespacesRequireApproval", Scope="namespace", Target="net.r_eg.IeXod.Shared")]
[module: SuppressMessage("Microsoft.Naming","CA1703:ResourceStringsShouldBeSpelledCorrectly", MessageId="aspnetcompiler", Scope="resource", Target="net.r_eg.IeXod.Strings.resources", Justification="AspNetCompiler is the name of the task")]
[module: SuppressMessage("Microsoft.Naming","CA1703:ResourceStringsShouldBeSpelledCorrectly", MessageId="vcproj", Scope="resource", Target="net.r_eg.IeXod.Strings.resources", Justification="vcproj is an extension and is spelled correctly")]

[module: SuppressMessage("Microsoft.Security","CA2119:SealMethodsThatSatisfyPrivateInterfaces", Scope="member", Target="net.r_eg.IeXod.Construction.ElementLocation.#get_Line()", Justification="This must be overridable. SmallElementLocation and RegularElementLocation already override it")]
[module: SuppressMessage("Microsoft.Security","CA2119:SealMethodsThatSatisfyPrivateInterfaces", Scope="member", Target="net.r_eg.IeXod.Construction.ElementLocation.#get_File()", Justification="This must be overridable. SmallElementLocation and RegularElementLocation already override it")]
[module: SuppressMessage("Microsoft.Security","CA2119:SealMethodsThatSatisfyPrivateInterfaces", Scope="member", Target="net.r_eg.IeXod.Construction.ElementLocation.#get_Column()", Justification="This must be overridable. SmallElementLocation and RegularElementLocation already override it")]

#endif
