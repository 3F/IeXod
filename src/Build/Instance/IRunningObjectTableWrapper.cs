// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace net.r_eg.IeXod.Execution
{
    internal interface IRunningObjectTableWrapper
    {
        object GetObject(string itemName);
    }
}
