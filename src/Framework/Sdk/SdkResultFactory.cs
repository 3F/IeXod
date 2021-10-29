// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace net.r_eg.IeXod.Sdk
{
    /// <summary>
    ///     An abstract interface class provided to <see cref="SdkResolver" /> to create an
    ///     <see cref="SdkResult" /> object indicating success / failure.
    /// </summary>
    public abstract class SdkResultFactory
    {
        /// <summary>
        ///     Create an <see cref="SdkResolver" /> object indicating success resolving the SDK.
        /// </summary>
        /// <param name="path">Path to the SDK.</param>
        /// <param name="version">Version of the SDK that was resolved.</param>
        /// <param name="warnings">Optional warnings to display during resolution.</param>
        /// <returns></returns>
        public abstract SdkResult IndicateSuccess(string path, string version, IEnumerable<string> warnings = null);

        /// <summary>
        ///     Create an <see cref="SdkResolver" /> object indicating failure resolving the SDK.
        /// </summary>
        /// <param name="errors">
        ///     Errors / reasons the SDK could not be resolved. Will be logged as a
        ///     build error if no other SdkResolvers were able to indicate success.
        /// </param>
        /// <param name="warnings"></param>
        /// <returns></returns>
        public abstract SdkResult IndicateFailure(IEnumerable<string> errors, IEnumerable<string> warnings = null);
    }
}
