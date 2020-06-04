// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;

namespace net.r_eg.IeXod.Collections
{
    /// <summary>
    /// Debug view for HashSet
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class HashSetDebugView<T> where T : class, IKeyed
    {
        private readonly RetrievableEntryHashSet<T> _set;

        public HashSetDebugView(RetrievableEntryHashSet<T> set)
        {
            _set = set ?? throw new ArgumentNullException(nameof(set));
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items => _set.ToArray();
    }
}
