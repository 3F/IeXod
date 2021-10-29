// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using net.r_eg.IeXod.Framework.Profiler;

namespace net.r_eg.IeXod.Evaluation
{
    /// <summary>
    /// Pretty prints an evaluation location in tab separated value (TSV) format
    /// </summary>
    internal sealed class EvaluationLocationTabSeparatedPrettyPrinter : EvaluationLocationPrettyPrinterBase
    {
        private const string Separator = "\t";

        /// <inheritdoc/> 
        internal override void AppendHeader(StringBuilder stringBuilder)
        {
            AppendDefaultHeaderWithSeparator(stringBuilder, Separator);
        }

        /// <inheritdoc/>
        internal override void AppendLocation(StringBuilder stringBuilder, TimeSpan totalTime, EvaluationLocation evaluationLocation, ProfiledLocation profiledLocation)
        {
            AppendDefaultLocationWithSeparator(stringBuilder, totalTime, evaluationLocation, profiledLocation, Separator);
        }

        /// <inheritdoc/>
        protected override string NormalizeExpression(string description, EvaluationLocationKind kind)
        {
            var text = GetElementOrConditionText(description, kind);
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            // Swap tabs for spaces, so we don't mess up the TSV format
            text = text.Replace(Separator, " ");

            return text;
        }
    }
}
