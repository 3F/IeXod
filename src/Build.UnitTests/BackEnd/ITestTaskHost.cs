// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using net.r_eg.IeXod.Framework;

namespace net.r_eg.IeXod.UnitTests.BackEnd
{
    /// <summary>
    /// Interface used by the test task to communicate what the TaskExecutionHost did to it.
    /// </summary>
    internal interface ITestTaskHost : ITaskHost
    {
        /// <summary>
        /// Called when a parameter is set on the task.
        /// </summary>
        void ParameterSet(string parameterName, object valueSet);

        /// <summary>
        /// Called when an output is read from the task.
        /// </summary>
        void OutputRead(string parameterName, object actualValue);
    }
}
