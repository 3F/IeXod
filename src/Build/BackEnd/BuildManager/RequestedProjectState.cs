﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using net.r_eg.IeXod.BackEnd;

namespace net.r_eg.IeXod.Execution
{
    /// <summary>
    /// Interface defining properties, items, and metadata of interest for a <see cref="BuildRequestData"/>.
    /// </summary>
    public class RequestedProjectState : ITranslatable
    {
        private List<string> _propertyFilters;
        private IDictionary<string, List<string>> _itemFilters;

        /// <summary>
        /// Properties of interest.
        /// </summary>
        public List<string> PropertyFilters
        {
            get => _propertyFilters;
            set => _propertyFilters = value;
        }

        /// <summary>
        /// Items and metadata of interest.
        /// </summary>
        public IDictionary<string, List<string>> ItemFilters
        {
            get => _itemFilters;
            set => _itemFilters = value;
        }

        void ITranslatable.Translate(ITranslator translator)
        {
            translator.Translate(ref _propertyFilters);
            translator.TranslateDictionary(ref _itemFilters, TranslateString, TranslateMetadataForItem, CreateItemMetadataDictionary);
        }

        private static IDictionary<string, List<string>> CreateItemMetadataDictionary(int capacity)
        {
            return new Dictionary<string, List<string>>(capacity, StringComparer.OrdinalIgnoreCase);
        }

        private static void TranslateMetadataForItem(ref List<string> list, ITranslator translator)
        {
            translator.Translate(ref list);
        }

        private static void TranslateString(ref string s, ITranslator translator)
        {
            translator.Translate(ref s);
        }
    }
}
