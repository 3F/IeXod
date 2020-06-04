// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using net.r_eg.IeXod.Construction;

namespace net.r_eg.IeXod.ObjectModelRemoting
{
    /// <summary>
    /// External projects support.
    /// Allow for creating a local representation to external object of type <see cref="ProjectTaskElement"/>
    /// </summary>
    public abstract class ProjectTaskElementLink : ProjectElementContainerLink
    {
        /// <summary>
        /// Access to remote <see cref="ProjectTaskElement.Parameters"/>.
        /// </summary>
        public abstract IDictionary<string, string> Parameters { get; }

        /// <summary>
        /// Access to remote <see cref="ProjectTaskElement.ParameterLocations"/>.
        /// </summary>
        public abstract IEnumerable<KeyValuePair<string, ElementLocation>> ParameterLocations { get; }

        /// <summary>
        /// Facilitate remoting the <see cref="ProjectTaskElement.GetParameter"/>.
        /// </summary>
        public abstract string GetParameter(string name);

        /// <summary>
        /// Facilitate remoting the <see cref="ProjectTaskElement.SetParameter"/>.
        /// </summary>
        public abstract void SetParameter(string name, string unevaluatedValue);

        /// <summary>
        /// Facilitate remoting the <see cref="ProjectTaskElement.RemoveParameter"/>.
        /// </summary>
        public abstract void RemoveParameter(string name);

        /// <summary>
        /// Facilitate remoting the <see cref="ProjectTaskElement.RemoveAllParameters"/>.
        /// </summary>
        public abstract void RemoveAllParameters();
    }
}
