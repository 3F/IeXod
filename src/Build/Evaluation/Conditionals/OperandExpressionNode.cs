﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Globalization;
using System.IO;
using System;

using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod.Evaluation
{
    /// <summary>
    /// Base class for all nodes that are operands (are leaves in the parse tree)
    /// </summary>
    internal abstract class OperandExpressionNode : GenericExpressionNode
    {
        #region REMOVE_COMPAT_WARNING

        internal override bool DetectAnd()
        {
            return false;
        }

        internal override bool DetectOr()
        {
            return false;
        }
        #endregion

    }
}