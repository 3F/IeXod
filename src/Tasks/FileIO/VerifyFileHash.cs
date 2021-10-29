// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Shared;
using net.r_eg.IeXod.Shared.FileSystem;

namespace net.r_eg.IeXod.Tasks
{
    /// <summary>
    /// Verifies that a file matches the expected file hash.
    /// </summary>
    public sealed class VerifyFileHash : TaskExtension
    {
        /// <summary>
        /// The file path.
        /// </summary>
        [Required]
        public string File { get; set; }

        /// <summary>
        /// The expected hash of the file.
        /// </summary>
        [Required]
        public string Hash { get; set; }

        /// <summary>
        /// The encoding format of <see cref="Hash"/>. Defaults to "hex".
        /// </summary>
        public string HashEncoding { get; set; } = GetFileHash._hashEncodingHex;

        /// <summary>
        /// The hashing algorithm to use. Allowed values: SHA256, SHA384, SHA512. Default = SHA256.
        /// </summary>
        public string Algorithm { get; set; } = GetFileHash._defaultFileHashAlgorithm;

        public override bool Execute()
        {
            if (!FileSystems.Default.FileExists(File))
            {
                Log.LogErrorWithCodeFromResources("FileHash.FileNotFound", File);
                return false;
            }

            if (!GetFileHash.SupportedAlgorithms.TryGetValue(Algorithm, out var algorithmFactory))
            {
                Log.LogErrorWithCodeFromResources("FileHash.UnrecognizedHashAlgorithm", Algorithm);
                return false;
            }

            if (!GetFileHash.TryParseHashEncoding(HashEncoding, out var encoding))
            {
                Log.LogErrorWithCodeFromResources("FileHash.UnrecognizedHashEncoding", HashEncoding);
                return false;
            }

            byte[] hash = GetFileHash.ComputeHash(algorithmFactory, File);
            string actualHash = GetFileHash.EncodeHash(encoding, hash);
            var comparison = encoding == Tasks.HashEncoding.Hex
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal;
            if (!string.Equals(actualHash, Hash, comparison))
            {
                Log.LogErrorWithCodeFromResources("VerifyFileHash.HashMismatch", File, Algorithm, Hash, actualHash);
                return false;
            }

            return !Log.HasLoggedErrors;
        }
    }
}
