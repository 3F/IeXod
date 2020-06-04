﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod.Evaluation
{
    /// <summary>
    /// Base class for nodes that are operators (have children in the parse tree)
    /// </summary>
    internal abstract class OperatorExpressionNode : GenericExpressionNode
    {
        /// <summary>
        /// Numeric evaluation is never allowed for operators
        /// </summary>
        internal override double NumericEvaluate(ConditionEvaluator.IConditionEvaluationState state)
        {
            // Should be unreachable: all calls check CanNumericEvaluate() first
            ErrorUtilities.VerifyThrow(false, "Cannot numeric evaluate an operator");
            return 0.0D;
        }

        /// <summary>
        /// Version evaluation is never allowed for operators
        /// </summary>
        internal override Version VersionEvaluate(ConditionEvaluator.IConditionEvaluationState state)
        {
            ErrorUtilities.VerifyThrow(false, "Cannot version evaluate an operator");
            return null;
        }

        /// <summary>
        /// Whether boolean evaluation is allowed: always allowed for operators
        /// </summary>
        internal override bool CanBoolEvaluate(ConditionEvaluator.IConditionEvaluationState state)
        {
            return true;
        }

        /// <summary>
        /// Whether the node can be evaluated as a numeric: by default,
        /// this is not allowed
        /// </summary>
        internal override bool CanNumericEvaluate(ConditionEvaluator.IConditionEvaluationState state)
        {
            return false;
        }

        /// <summary>
        /// Whether the node can be evaluated as a version: by default,
        /// this is not allowed
        /// </summary>
        internal override bool CanVersionEvaluate(ConditionEvaluator.IConditionEvaluationState state)
        {
            return false;
        }

        /// <summary>
        /// Value after any item and property expressions are expanded
        /// </summary>
        /// <returns></returns>
        internal override string GetExpandedValue(ConditionEvaluator.IConditionEvaluationState state)
        {
            return null;
        }

        /// <summary>
        /// Value before any item and property expressions are expanded
        /// </summary>
        /// <returns></returns>
        internal override string GetUnexpandedValue(ConditionEvaluator.IConditionEvaluationState state)
        {
            return null;
        }

        /// <summary>
        /// If any expression nodes cache any state for the duration of evaluation, 
        /// now's the time to clean it up
        /// </summary>
        internal override void ResetState()
        {
            if (LeftChild != null)
            {
                LeftChild.ResetState();
            }

            if (RightChild != null)
            {
                RightChild.ResetState();
            }
        }

        /// <summary>
        /// Storage for the left child
        /// </summary>
        internal GenericExpressionNode LeftChild { set; get; }

        /// <summary>
        /// Storage for the right child
        /// </summary>
        internal GenericExpressionNode RightChild { set; get; }

        #region REMOVE_COMPAT_WARNING
        internal override bool DetectAnd()
        {
            // Read the state of the current node
            bool detectedAnd = this.PossibleAndCollision;
            // Reset the flags on the current node
            this.PossibleAndCollision = false;
            // Process the children of the node if preset
            bool detectAndRChild = false;
            bool detectAndLChild = false;
            if (RightChild != null)
            {
                detectAndRChild = RightChild.DetectAnd();
            }
            if (LeftChild != null)
            {
                detectAndLChild = LeftChild.DetectAnd();
            }
            return detectedAnd || detectAndRChild || detectAndLChild;
        }

        internal override bool DetectOr()
        {
            // Read the state of the current node
            bool detectedOr = this.PossibleOrCollision;
            // Reset the flags on the current node
            this.PossibleOrCollision = false;
            // Process the children of the node if preset
            bool detectOrRChild = false;
            bool detectOrLChild = false;
            if (RightChild != null)
            {
                detectOrRChild = RightChild.DetectOr();
            }
            if (LeftChild != null)
            {
                detectOrLChild = LeftChild.DetectOr();
            }
            return detectedOr || detectOrRChild || detectOrLChild;
        }
        #endregion
    }
}
