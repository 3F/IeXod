// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
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
    /// Performs logical OR on children
    /// Does not update conditioned properties table
    /// </summary>
    internal sealed class OrExpressionNode : OperatorExpressionNode
    {
        /// <summary>
        /// Evaluate as boolean
        /// </summary>
        internal override bool BoolEvaluate(ConditionEvaluationState state)
        {
            ProjectErrorUtilities.VerifyThrowInvalidProject
                    (LeftChild.CanBoolEvaluate(state),
                     state.conditionAttribute,
                     "ExpressionDoesNotEvaluateToBoolean",
                     LeftChild.GetUnexpandedValue(state),
                     LeftChild.GetExpandedValue(state),
                     state.parsedCondition);

            if (LeftChild.BoolEvaluate(state))
            {
                // Short circuit
                return true;
            }
            else
            {
                ProjectErrorUtilities.VerifyThrowInvalidProject
                    (RightChild.CanBoolEvaluate(state),
                     state.conditionAttribute,
                     "ExpressionDoesNotEvaluateToBoolean",
                     RightChild.GetUnexpandedValue(state),
                     RightChild.GetExpandedValue(state),
                     state.parsedCondition);

                return RightChild.BoolEvaluate(state);
            }
        }

        #region REMOVE_COMPAT_WARNING
        private bool possibleOrCollision = true;
        internal override bool PossibleOrCollision
        {
            set { this.possibleOrCollision = value; }
            get { return this.possibleOrCollision; }
        }
        #endregion
    }
}
