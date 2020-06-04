// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace net.r_eg.IeXod.Evaluation
{
    /// <summary>
    /// Interface for an object which can provide toolsets for evaulation.
    /// </summary>
    internal interface IToolsetProvider
    {
        /// <summary>
        /// Gets an enumeration of all toolsets in the provider.
        /// </summary>
        ICollection<Toolset> Toolsets
        {
            get;
        }

        /// <summary>
        /// Retrieves a specific toolset.
        /// </summary>
        /// <param name="toolsVersion">The tools version for the toolset.</param>
        /// <returns>The requested toolset.</returns>
        Toolset GetToolset(string toolsVersion);
    }
}
