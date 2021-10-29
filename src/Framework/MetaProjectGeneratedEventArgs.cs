﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

namespace net.r_eg.IeXod.Framework
{
    /// <summary>
    /// Arguments for the metaproject generated event.
    /// </summary>
    [Serializable]
    public class MetaprojectGeneratedEventArgs : BuildMessageEventArgs
    {
        /// <summary>
        /// Raw xml representing the metaproject.
        /// </summary>
        public string metaprojectXml;

        /// <summary>
        /// Initializes a new instance of the MetaprojectGeneratedEventArgs class.
        /// </summary>
        public MetaprojectGeneratedEventArgs(string metaprojectXml, string metaprojectPath, string message)
            : base(message, null, null, MessageImportance.Low, DateTime.UtcNow, metaprojectPath)
        {
            this.metaprojectXml = metaprojectXml;
            this.ProjectFile = metaprojectPath;
        }
    }
}
