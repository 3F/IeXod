// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// FxCop Suppression file
// To Use:
// Add add module level suppressions to this file to have them suppressed in the assembly

using System.Diagnostics.CodeAnalysis;

#if CODE_ANALYSIS
[module: SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")]
[module: SuppressMessage("Microsoft.MSInternal", "CA905:SystemAndMicrosoftNamespacesRequireApproval", Scope="namespace", Target="net.r_eg.IeXod.CommandLine", Justification="This is an approved namespace.")]
[module: SuppressMessage("Microsoft.Naming","CA1709:IdentifiersShouldBeCasedCorrectly", MessageId="STA", Scope="type", Target="net.r_eg.IeXod.Framework.RunInSTAAttribute", Justification="Not worth breaking custormers because of case.")]
#endif
