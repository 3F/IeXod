// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace net.r_eg.IeXod.Framework
{
    /// <summary>
    /// Interface for exposing a ProjectElement to the appropriate loggers
    /// </summary>
    public interface IProjectElement
    {

        /// <summary>
        /// Gets the name of the associated element. 
        /// Useful for display in some circumstances.
        /// </summary>
        string ElementName { get; }


        /// <summary>
        /// The outer markup associated with this project element
        /// </summary>
        string OuterElement { get; }
    }
}
