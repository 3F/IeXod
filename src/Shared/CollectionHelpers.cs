// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using net.r_eg.IeXod.Evaluation;

namespace net.r_eg.IeXod.Shared
{
    /// <summary>
    /// Utilities for collections
    /// </summary>
    internal static class CollectionHelpers
    {
        /// <summary>
        /// Returns a new list containing the input list
        /// contents, except for nulls
        /// </summary>
        /// <typeparam name="T">Type of list elements</typeparam>
        internal static List<T> RemoveNulls<T>(List<T> inputs)
        {
            List<T> inputsWithoutNulls = new List<T>(inputs.Count);

            foreach (T entry in inputs)
            {
                if (entry != null)
                {
                    inputsWithoutNulls.Add(entry);
                }
            }

            // Avoid possibly having two identical lists floating around
            return (inputsWithoutNulls.Count == inputs.Count) ? inputs : inputsWithoutNulls;
        }

        /// <summary>
        /// Extension method -- combines a TryGet with a check to see that the value is equal. 
        /// </summary>
        internal static bool ContainsValueAndIsEqual(this Dictionary<string, string> dictionary, string key, string value, StringComparison comparer)
        {
            string valueFromDictionary = null;
            if (dictionary.TryGetValue(key, out valueFromDictionary))
            {
                return String.Equals(value, valueFromDictionary, comparer);
            }

            return false;
        }

        /// <summary>
        /// Returns most actual toolset from collection.
        /// </summary>
        internal static Toolset GetMostActual(this Dictionary<string, Toolset> toolsets)
        {
            // {major}.0 or Current
            static float _Parse(string v) => float.TryParse(v, out float r) ? r : -1;

            return toolsets?.OrderByDescending(t => _Parse(t.Key)).FirstOrDefault().Value;
        }
    }
}
