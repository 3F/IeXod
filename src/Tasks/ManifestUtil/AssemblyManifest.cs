﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace net.r_eg.IeXod.Tasks.Deployment.ManifestUtilities
{
    /// <summary>
    /// Describes a Win32 assembly manifest.
    /// </summary>
    /// <remarks>This is a serialization format, do not remove or rename private fields.</remarks>
    [ComVisible(false)]
    [XmlRoot("AssemblyManifest")]
    public class AssemblyManifest : Manifest
    {
        private ProxyStub[] _externalProxyStubs;

        /// <summary>
        /// Specifies the set of external proxy stubs referenced by the manifest for isolated applications and Reg-Free COM.
        /// </summary>
        [XmlIgnore]
        public ProxyStub[] ExternalProxyStubs => _externalProxyStubs;

        #region " XmlSerializer "

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [XmlArray("ExternalProxyStubs")]
        public ProxyStub[] XmlExternalProxyStubs
        {
            get => _externalProxyStubs;
            set => _externalProxyStubs = value;
        }

        #endregion
    }
}
