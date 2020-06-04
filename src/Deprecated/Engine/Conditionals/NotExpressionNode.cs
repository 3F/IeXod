// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Globalization;
using System.IO;
using System;
using System.Xml;

using net.r_eg.IeXod.BuildEngine.Shared;

namespace net.r_eg.IeXod.BuildEngine
{
    /// <summary>
    /// Performs logical NOT on left child
    /// Does not update conditioned properties table
    /// </summary>
    internal sealed class NotExpressionNode : OperatorExpressionNode
    {
        /// <summary>
        /// Evaluate as boolean
        /// </summary>
        internal override bool BoolEvaluate(ConditionEvaluationState state)
        {
            return !LeftChild.BoolEvaluate(state);
        }

        internal override bool CanBoolEvaluate(ConditionEvaluationState state)
        {
            return LeftChild.CanBoolEvaluate(state);
        }

        /// <summary>
        /// Returns unexpanded value with '!' prepended. Useful for error messages.
        /// </summary>
        internal override string GetUnexpandedValue(ConditionEvaluationState state)
        {
            return "!" + LeftChild.GetUnexpandedValue(state);
        }

        /// <summary>
        /// Returns expanded value with '!' prepended. Useful for error messages.
        /// </summary>
        internal override string GetExpandedValue(ConditionEvaluationState state)
        {
            return "!" + LeftChild.GetExpandedValue(state);
        }
    }
}
