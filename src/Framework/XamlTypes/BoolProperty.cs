﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace net.r_eg.IeXod.Framework.XamlTypes
{
    /// <summary>
    /// Represents the schame of a boolean property.
    /// </summary>
    public sealed class BoolProperty : BaseProperty
    {
        #region Properties

        /// <summary>
        /// Represents the logical negation of a boolean switch. 
        /// </summary>
        /// <example> 
        /// <para>
        /// For the VC++ CL task, <c>WholeProgramOptimization</c> is a boolean parameter. It's switch is <c>GL</c>. To
        /// disable whole program optimization, you need to pass the ReverseSwitch, which is <c>GL-</c>.
        /// </para>
        /// <para>
        /// This field is optional.
        /// </para>
        /// </example>
        public string ReverseSwitch
        {
            get;
            set;
        }

        #endregion
    }
}
