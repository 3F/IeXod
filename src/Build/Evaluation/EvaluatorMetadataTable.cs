﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using net.r_eg.IeXod.Construction;
using net.r_eg.IeXod.Collections;

using EscapingUtilities = net.r_eg.IeXod.Shared.EscapingUtilities;

namespace net.r_eg.IeXod.Evaluation
{
    /// <summary>
    /// Implementation of a metadata table for use by the evaluator.
    /// Accumulates ProjectMetadataElement objects and their evaluated value,
    /// overwriting any previous metadata with that name.
    /// </summary>
    internal class EvaluatorMetadataTable : IMetadataTable
    {
        /// <summary>
        /// The actual metadata dictionary.
        /// </summary>
        private Dictionary<string, EvaluatorMetadata> _metadata;

        /// <summary>
        /// The type of item the metadata should be considered to apply to.
        /// </summary>
        private string _implicitItemType;

        /// <summary>
        /// Creates a new table using the specified item type.
        /// </summary>
        public EvaluatorMetadataTable(string implicitItemType)
        {
            _implicitItemType = implicitItemType;
        }

        /// <summary>
        /// Enumerator over the entries in this table
        /// </summary>
        internal IEnumerable<EvaluatorMetadata> Entries => _metadata?.Values ?? Enumerable.Empty<EvaluatorMetadata>();

        /// <summary>
        /// Retrieves any value we have in our metadata table for the metadata name specified,
        /// whatever the item type.
        /// If no value is available, returns empty string.
        /// </summary>
        public string GetEscapedValue(string name)
        {
            return GetEscapedValue(null, name);
        }

        /// <summary>
        /// Retrieves any value we have in our metadata table for the metadata name and item type specified.
        /// If no value is available, returns empty string.
        /// </summary>
        public string GetEscapedValue(string itemType, string name)
        {
            return GetEscapedValueIfPresent(itemType, name) ?? String.Empty;
        }

        /// <summary>
        /// Retrieves any value we have in our metadata table for the metadata name and item type specified.
        /// If no value is available, returns null.
        /// </summary>
        public string GetEscapedValueIfPresent(string itemType, string name)
        {
            if (_metadata == null)
            {
                return null;
            }

            string value = null;

            if (itemType == null || String.Equals(_implicitItemType, itemType, StringComparison.OrdinalIgnoreCase))
            {
                EvaluatorMetadata metadatum;
                _metadata.TryGetValue(name, out metadatum);

                if (metadatum != null)
                {
                    value = metadatum.EvaluatedValueEscaped;
                }
            }

            return value;
        }

        /// <summary>
        /// Adds a metadata entry to the table
        /// </summary>
        internal void SetValue(ProjectMetadataElement xml, string evaluatedValueEscaped)
        {
            if (_metadata == null)
            {
                _metadata = new Dictionary<string, EvaluatorMetadata>(MSBuildNameIgnoreCaseComparer.Default);
            }

            _metadata[xml.Name] = new EvaluatorMetadata(xml, evaluatedValueEscaped);
        }

        /// <summary>
        /// An entry in the evaluator's metadata table.
        /// </summary>
        public class EvaluatorMetadata
        {
            /// <summary>
            /// Construct a new EvaluatorMetadata
            /// </summary>
            public EvaluatorMetadata(ProjectMetadataElement xml, string evaluatedValueEscaped)
            {
                this.Xml = xml;
                this.EvaluatedValueEscaped = evaluatedValueEscaped;
            }

            /// <summary>
            /// Gets or sets the metadata Xml
            /// </summary>
            public ProjectMetadataElement Xml
            {
                get;
                private set;
            }

            /// <summary>
            /// Gets or sets the evaluated value, unescaped
            /// </summary>
            public string EvaluatedValue
            {
                get
                {
                    return EscapingUtilities.UnescapeAll(EvaluatedValueEscaped);
                }
            }

            /// <summary>
            /// Gets or sets the evaluated value, escaped as necessary
            /// </summary>
            internal string EvaluatedValueEscaped
            {
                get;
                private set;
            }
        }
    }
}
