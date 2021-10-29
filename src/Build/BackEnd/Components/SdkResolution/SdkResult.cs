// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using SdkReference = net.r_eg.IeXod.Sdk.SdkReference;
using SdkResultBase = net.r_eg.IeXod.Sdk.SdkResult;

namespace net.r_eg.IeXod.BackEnd.SdkResolution
{
    /// <summary>
    /// An internal implementation of <see cref="net.r_eg.IeXod.Framework.SdkResult"/>.
    /// </summary>
    internal sealed class SdkResult : SdkResultBase, INodePacket
    {
        private string _path;
        private string _version;

        public SdkResult(ITranslator translator)
        {
            Translate(translator);
        }

        public SdkResult(SdkReference sdkReference, IEnumerable<string> errors, IEnumerable<string> warnings)
        {
            Success = false;
            SdkReference = sdkReference;
            Errors = errors;
            Warnings = warnings;
        }

        public SdkResult(SdkReference sdkReference, string path, string version, IEnumerable<string> warnings)
        {
            Success = true;
            SdkReference = sdkReference;
            _path = path;
            _version = version;
            Warnings = warnings;
        }

        public SdkResult()
        {
        }

        public Construction.ElementLocation ElementLocation { get; set; }

        public IEnumerable<string> Errors { get; }

        public override string Path => _path;

        public override SdkReference SdkReference { get; protected set; }

        public override string Version => _version;

        public IEnumerable<string> Warnings { get; }
        public void Translate(ITranslator translator)
        {
            translator.Translate(ref _path);
            translator.Translate(ref _version);
        }

        public NodePacketType Type => NodePacketType.ResolveSdkResponse;

        public static INodePacket FactoryForDeserialization(ITranslator translator)
        {
            return new SdkResult(translator);
        }
    }
}
