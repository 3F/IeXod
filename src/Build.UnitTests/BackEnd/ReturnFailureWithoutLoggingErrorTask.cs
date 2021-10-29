// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.BackEnd.Logging;
using net.r_eg.IeXod.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace net.r_eg.IeXod.UnitTests
{
    /// <summary>
    /// This task was created for https://github.com/microsoft/msbuild/issues/2036
    /// </summary>
    public class ReturnFailureWithoutLoggingErrorTask : Task
    {
        /// <summary>
        /// Intentionally return false without logging an error to test proper error catching.
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            return false;
        }
    }
}
