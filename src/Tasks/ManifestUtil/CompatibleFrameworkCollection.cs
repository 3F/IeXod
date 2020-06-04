﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace net.r_eg.IeXod.Tasks.Deployment.ManifestUtilities
{
    [ComVisible(false)]
    public sealed class CompatibleFrameworkCollection : IEnumerable
    {
        private readonly List<CompatibleFramework> _list = new List<CompatibleFramework>();

        internal CompatibleFrameworkCollection(IEnumerable<CompatibleFramework> compatibleFrameworks)
        {
            if (compatibleFrameworks == null)
            {
                return;
            }
            _list.AddRange(compatibleFrameworks);
        }

        public CompatibleFramework this[int index] => _list[index];

        public void Add(CompatibleFramework compatibleFramework)
        {
            _list.Add(compatibleFramework);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public int Count => _list.Count;

        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        internal CompatibleFramework[] ToArray()
        {
            return _list.ToArray();
        }
    }
}
