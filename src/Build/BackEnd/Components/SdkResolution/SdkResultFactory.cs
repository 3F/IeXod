// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using SdkReference = net.r_eg.IeXod.Sdk.SdkReference;
using SdkResultBase = net.r_eg.IeXod.Sdk.SdkResult;
using SdkResultFactoryBase = net.r_eg.IeXod.Sdk.SdkResultFactory;

namespace net.r_eg.IeXod.BackEnd.SdkResolution
{
    /// <summary>
    /// An internal implementation of <see cref="net.r_eg.IeXod.Framework.SdkResultFactory"/>.
    /// </summary>
    internal class SdkResultFactory : SdkResultFactoryBase
    {
        private readonly SdkReference _sdkReference;

        internal SdkResultFactory(SdkReference sdkReference)
        {
            _sdkReference = sdkReference;
        }

        public override SdkResultBase IndicateFailure(IEnumerable<string> errors, IEnumerable<string> warnings = null)
        {
            return new SdkResult(_sdkReference, errors, warnings);
        }

        public override SdkResultBase IndicateSuccess(string path, string version, IEnumerable<string> warnings = null)
        {
            return new SdkResult(_sdkReference, path, version, warnings);
        }
    }
}
