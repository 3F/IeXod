// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Resources.Extensions;
using System.Text;
using System.Threading.Tasks;

namespace net.r_eg.IeXod.Tasks.ResourceHandling
{
    internal class BinaryFormatterByteArrayResource : IResource
    {
        public string Name { get; }
        public string OriginatingFile { get; }
        public byte[] Bytes { get; }

        /// <summary>
        /// BinaryFormatter byte arrays contain the type name, but it is not directly accessible from the resx.
        /// </summary>
        public string TypeAssemblyQualifiedName => null;

        /// <summary>
        /// BinaryFormatter byte arrays contain the type name, but it is not directly accessible from the resx.
        /// </summary>
        public string TypeFullName => null;

        public BinaryFormatterByteArrayResource(string name, byte[] bytes, string originatingFile)
        {
            Name = name;
            Bytes = bytes;
            OriginatingFile = originatingFile;
        }

        public void AddTo(IResourceWriter writer)
        {
            if (writer is PreserializedResourceWriter preserializedResourceWriter)
            {
                preserializedResourceWriter.AddBinaryFormattedResource(Name, Bytes);
            }
            else
            {
                throw new PreserializedResourceWriterRequiredException();
            }
        }
    }
}
