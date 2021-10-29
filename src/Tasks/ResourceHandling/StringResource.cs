// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace net.r_eg.IeXod.Tasks.ResourceHandling
{
    internal class StringResource : LiveObjectResource
    {
        public string OriginatingFile { get; }

        public new string TypeFullName => typeof(string).FullName;

        public StringResource(string name, string value, string filename) :
            base(name, value)
        {
            OriginatingFile = filename;
        }

        public new void AddTo(IResourceWriter writer)
        {
            writer.AddResource(Name, (string)Value);
        }

        public override string ToString()
        {
            return $"StringResource(\"{Name}\", \"{Value}\")";
        }
    }
}
