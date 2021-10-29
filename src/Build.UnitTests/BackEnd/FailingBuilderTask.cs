// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Collections;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Utilities;

namespace net.r_eg.IeXod.UnitTests
{
    public  class FailingBuilderTask : Task
    {
        public FailingBuilderTask()
            : base(null)
        { }

        public override bool Execute()
        {
            // BuildEngine.BuildProjectFile is how the GenerateTemporaryTargetAssembly task builds projects.
            return BuildEngine.BuildProjectFile(CurrentProject, new string[] { "ErrorTask" }, new Hashtable(), null);
        }

        [Required]
        public string CurrentProject { get; set; }
    }
}
