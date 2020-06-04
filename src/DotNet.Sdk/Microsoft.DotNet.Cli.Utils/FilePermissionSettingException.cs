// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace Microsoft.DotNet.Cli.Utils
{
    [Serializable]
    internal class FilePermissionSettingException : Exception
    {
        public FilePermissionSettingException()
        {
        }

        public FilePermissionSettingException(string message) : base(message)
        {
        }

        public FilePermissionSettingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FilePermissionSettingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
