﻿// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Extensions.EnvironmentAbstractions
{
    internal interface IEnvironment
    {
        string GetEnvironmentVariable(string name);
    }
}
