// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace net.r_eg.IeXod.Shared.Debugging
{
    internal class CommonWriter
    {
        // Action<string id, string callsite, IEnumerable<string> args>
        // id - something that identifies a group of messages. Process name and node type (central, worker) is a good id. The writer could choose different files depending on the ID. Or just print it differently.
        // callsite - class and method that logged the message
        // args - payload
        public static Action<string, string, IEnumerable<string>> Writer { get; set; }
    }
}
