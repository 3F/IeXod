// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using NUnit.Framework;
using net.r_eg.IeXod.BuildEngine.Shared;

namespace net.r_eg.IeXod.UnitTests
{
 [TestFixture]
 public class TypeLoader_Tests
 {
    [Test]
    public void Basic()
    {

        Assertion.Assert(TypeLoader.IsPartialTypeNameMatch("Csc", "csc")); // ==> exact match
        Assertion.Assert(TypeLoader.IsPartialTypeNameMatch("net.r_eg.IeXod.Tasks.Csc", "net.r_eg.IeXod.Tasks.Csc")); // ==> exact match
        Assertion.Assert(TypeLoader.IsPartialTypeNameMatch("net.r_eg.IeXod.Tasks.Csc", "Csc")); // ==> partial match
        Assertion.Assert(TypeLoader.IsPartialTypeNameMatch("net.r_eg.IeXod.Tasks.Csc", "Tasks.Csc")); // ==> partial match
        Assertion.Assert(TypeLoader.IsPartialTypeNameMatch("MyTasks.ATask+NestedTask", "NestedTask")); // ==> partial match
        Assertion.Assert(TypeLoader.IsPartialTypeNameMatch("MyTasks.ATask\\\\+NestedTask", "NestedTask")); // ==> partial match
        Assertion.Assert(!TypeLoader.IsPartialTypeNameMatch("MyTasks.CscTask", "Csc")); // ==> no match
        Assertion.Assert(!TypeLoader.IsPartialTypeNameMatch("MyTasks.MyCsc", "Csc")); // ==> no match
        Assertion.Assert(!TypeLoader.IsPartialTypeNameMatch("MyTasks.ATask\\.Csc", "Csc")); // ==> no match
        Assertion.Assert(!TypeLoader.IsPartialTypeNameMatch("MyTasks.ATask\\\\\\.Csc", "Csc")); // ==> no match
    }

     [Test]
     public void Regress_Mutation_TrailingPartMustMatch()
     {
         Assertion.Assert(!TypeLoader.IsPartialTypeNameMatch("net.r_eg.IeXod.Tasks.Csc", "Vbc"));
     }

     [Test]
     public void Regress_Mutation_ParameterOrderDoesntMatter()
     {
         Assertion.Assert(TypeLoader.IsPartialTypeNameMatch("Csc", "net.r_eg.IeXod.Tasks.Csc"));
     }

 }
}
