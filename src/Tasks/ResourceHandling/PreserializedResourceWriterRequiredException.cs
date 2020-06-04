// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace net.r_eg.IeXod.Tasks.ResourceHandling
{
    [Serializable]
    internal sealed class PreserializedResourceWriterRequiredException : Exception
    {
        public PreserializedResourceWriterRequiredException() { }

        private PreserializedResourceWriterRequiredException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
