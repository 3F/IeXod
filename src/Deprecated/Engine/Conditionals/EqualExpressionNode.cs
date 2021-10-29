// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Globalization;
using System.IO;
using System;

using net.r_eg.IeXod.BuildEngine.Shared;

namespace net.r_eg.IeXod.BuildEngine
{
    /// <summary>
    /// Compares for equality
    /// </summary>
    internal sealed class EqualExpressionNode : MultipleComparisonNode
    {
        /// <summary>
        /// Compare numbers
        /// </summary>
        protected override bool Compare(double left, double right)
        {
            return left == right;
        }

        /// <summary>
        /// Compare booleans
        /// </summary>
        protected override bool Compare(bool left, bool right)
        {
            return left == right;
        }

        /// <summary>
        /// Compare strings
        /// </summary>
        protected override bool Compare(string left, string right)
        {
            return String.Equals(left, right, StringComparison.OrdinalIgnoreCase);
        }
    }
}
