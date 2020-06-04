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
    /// Evaluates a numeric comparison, such as less-than, or greater-or-equal-than
    /// Does not update conditioned properties table.
    /// </summary>
    internal abstract class NumericComparisonExpressionNode : OperatorExpressionNode
    {
        /// <summary>
        /// Compare numbers
        /// </summary>
        protected abstract bool Compare(double left, double right);

        /// <summary>
        /// Evaluate as boolean
        /// </summary>
        internal override bool BoolEvaluate(ConditionEvaluationState state)
        {
            ProjectErrorUtilities.VerifyThrowInvalidProject
                (LeftChild.CanNumericEvaluate(state) && RightChild.CanNumericEvaluate(state),
                 state.conditionAttribute, 
                "ComparisonOnNonNumericExpression",
                 state.parsedCondition,
                 /* helpfully display unexpanded token and expanded result in error message */
                 (LeftChild.CanNumericEvaluate(state) ? RightChild.GetUnexpandedValue(state) : LeftChild.GetUnexpandedValue(state)),
                 (LeftChild.CanNumericEvaluate(state) ? RightChild.GetExpandedValue(state) : LeftChild.GetExpandedValue(state)));

            return Compare(LeftChild.NumericEvaluate(state), RightChild.NumericEvaluate(state));
        }
    }
}
