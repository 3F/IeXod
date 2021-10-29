// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Utilities;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyLoadContextTest
{
    public class RegisterObject : Task
    {
        internal const string CacheKey = "RegressionForMSBuild#5080";

        public override bool Execute()
        {
            BuildEngine4.RegisterTaskObject(
                  CacheKey,
                  new RegisterObject(),
                  RegisteredTaskObjectLifetime.Build,
                  allowEarlyCollection: false);

            return true;
        }
    }

    public class RetrieveObject : Task
    {
        public override bool Execute()
        {
            var entry = (RegisterObject)BuildEngine4.GetRegisteredTaskObject(RegisterObject.CacheKey, RegisteredTaskObjectLifetime.Build);

            return true;
        }
    }
}
