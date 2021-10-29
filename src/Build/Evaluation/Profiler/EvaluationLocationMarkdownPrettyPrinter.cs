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
    /// Pretty prints an evaluation location in markdown format
    /// </summary>
    internal sealed class EvaluationLocationMarkdownPrettyPrinter : EvaluationLocationPrettyPrinterBase
    {
        private const string Separator = "|";

        /// <inheritdoc/>
        internal override void AppendHeader(StringBuilder stringBuilder)
        {
            AppendDefaultHeaderWithSeparator(stringBuilder, Separator);
            stringBuilder.AppendLine("---|---|---|---|---:|---|---:|---:|---:|---:|---:|---:|---");
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

            text = text.Replace(Separator, "\\" + Separator);

            if (text.Length > 100)
                text = text.Remove(100) + "...";

            return '`' + text + '`';
        }
    }
}
