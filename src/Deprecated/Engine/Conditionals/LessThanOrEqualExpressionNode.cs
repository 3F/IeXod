// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
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
    /// Compares for left <= right
    /// </summary>
    internal sealed class LessThanOrEqualExpressionNode : NumericComparisonExpressionNode
    {
        /// <summary>
        /// Compare numerically
        /// </summary>
        protected override bool Compare(double left, double right)
        {
            return left <= right;
        }
    }
}
