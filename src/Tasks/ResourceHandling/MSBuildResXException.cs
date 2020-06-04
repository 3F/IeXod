// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace net.r_eg.IeXod.Tasks.ResourceHandling
{
    [Serializable]
    internal class MSBuildResXException : Exception
    {
        public MSBuildResXException()
        {
        }

        public MSBuildResXException(string message) : base(message)
        {
        }

        public MSBuildResXException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MSBuildResXException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
