﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace net.r_eg.IeXod.Sdk
{
    [Serializable]
    public enum SdkLogVerbosity
    {
        /// <summary>
        /// High importance, appears in less verbose logs
        /// </summary>
        High,

        /// <summary>
        /// Normal importance
        /// </summary>
        Normal,

        /// <summary>
        /// Low importance, appears in more verbose logs
        /// </summary>
        Low
    }
}
