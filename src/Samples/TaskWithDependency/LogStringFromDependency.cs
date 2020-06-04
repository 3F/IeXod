// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.Framework;
using System.Reflection;
using Dependency;

namespace TaskWithDependency
{
    public class LogStringFromDependency : net.r_eg.IeXod.Utilities.Task
    {
        public override bool Execute()
        {
            Log.LogMessage($"Message from dependency: {Alpha.GetString()}");

            return true;
        }
    }
}
