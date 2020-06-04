// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using ElementLocation = net.r_eg.IeXod.Construction.ElementLocation;
using net.r_eg.IeXod.Shared;
using System.Diagnostics;

namespace net.r_eg.IeXod.BackEnd
{
    /// <summary>
    /// Contains information about a target name and reference location.
    /// </summary>
    [DebuggerDisplay("Name={TargetName}")]
    internal class TargetSpecification : ITranslatable
    {
        private string _targetName;
        private ElementLocation _referenceLocation;

        /// <summary>
        /// Construct a target specification.
        /// </summary>
        /// <param name="targetName">The name of the target</param>
        /// <param name="referenceLocation">The location from which it was referred.</param>
        internal TargetSpecification(string targetName, ElementLocation referenceLocation)
        {
            ErrorUtilities.VerifyThrowArgumentLength(targetName, "targetName");
            ErrorUtilities.VerifyThrowArgumentNull(referenceLocation, "referenceLocation");

            this._targetName = targetName;
            this._referenceLocation = referenceLocation;
        }

        private TargetSpecification()
        {
        }

        /// <summary>
        /// Gets or sets the target name            
        /// </summary>
        public string TargetName => _targetName;

        /// <summary>
        /// Gets or sets the reference location
        /// </summary>
        public ElementLocation ReferenceLocation => _referenceLocation;

        void ITranslatable.Translate(ITranslator translator)
        {
            translator.Translate(ref _targetName);
            translator.Translate(ref _referenceLocation, ElementLocation.FactoryForDeserialization);
        }

        internal static TargetSpecification FactoryForDeserialization(ITranslator translator)
        {
            var instance = new TargetSpecification();
            ((ITranslatable) instance).Translate(translator);

            return instance;
        }
    }
}
