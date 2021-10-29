// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

using net.r_eg.IeXod.BackEnd;
using net.r_eg.IeXod.Collections;
using net.r_eg.IeXod.Execution;
using net.r_eg.IeXod.Shared;

using ObjectModel = System.Collections.ObjectModel;

namespace net.r_eg.IeXod.Evaluation
{
    /// <summary>
    /// Aggregation of a set of properties that correspond to a particular sub-toolset.  
    /// </summary>
    [DebuggerDisplay("SubToolsetVersion={SubToolsetVersion} #Properties={_properties.Count}")]
    public class SubToolset : ITranslatable
    {
        /// <summary>
        /// VisualStudioVersion that corresponds to this subtoolset
        /// </summary>
        private string _subToolsetVersion;

        /// <summary>
        /// The properties defined by the subtoolset.
        /// </summary> 
        private PropertyDictionary<ProjectPropertyInstance> _properties;

        /// <summary>
        /// Constructor that associates a set of properties with a sub-toolset version.  
        /// </summary>
        internal SubToolset(string subToolsetVersion, PropertyDictionary<ProjectPropertyInstance> properties)
        {
            ErrorUtilities.VerifyThrowArgumentLength(subToolsetVersion, "subToolsetVersion");

            _subToolsetVersion = subToolsetVersion;
            _properties = properties;
        }

        /// <summary>
        /// Private constructor for translation
        /// </summary>
        private SubToolset(ITranslator translator)
        {
            ((ITranslatable)this).Translate(translator);
        }

        /// <summary>
        /// VisualStudioVersion that corresponds to this subtoolset
        /// </summary>
        public string SubToolsetVersion
        {
            get
            {
                return _subToolsetVersion;
            }
        }

        /// <summary>
        /// The properties that correspond to this particular sub-toolset. 
        /// </summary>
        public IDictionary<string, ProjectPropertyInstance> Properties
        {
            get
            {
                if (_properties == null)
                {
                    return ReadOnlyEmptyDictionary<string, ProjectPropertyInstance>.Instance;
                }

                return new ObjectModel.ReadOnlyDictionary<string, ProjectPropertyInstance>(_properties);
            }
        }

        /// <summary>
        /// Translates the sub-toolset.
        /// </summary>
        void ITranslatable.Translate(ITranslator translator)
        {
            translator.Translate(ref _subToolsetVersion);
            translator.TranslateProjectPropertyInstanceDictionary(ref _properties);
        }

        /// <summary>
        /// Factory for deserialization.
        /// </summary>
        internal static SubToolset FactoryForDeserialization(ITranslator translator)
        {
            SubToolset subToolset = new SubToolset(translator);
            return subToolset;
        }
    }
}