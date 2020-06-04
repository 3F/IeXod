// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace net.r_eg.IeXod.Tasks.Deployment.Bootstrapper
{
    /// <summary>
    /// Handles and stores xml validation events for a product, and contains the XmlValidationResults of a package.
    /// </summary>
    internal sealed class ProductValidationResults : XmlValidationResults
    {
        private readonly Dictionary<string, XmlValidationResults> _packageValidationResults =
            new Dictionary<string, XmlValidationResults>(StringComparer.Ordinal);

        public ProductValidationResults(string filePath) : base(filePath)
        {
        }

        /// <summary>
        /// Adds the validation results of a package of the specified culture into the ProductValidationResults.
        /// </summary>
        /// <param name="culture">The culture of the XmlValidationResults to add.</param>
        /// <param name="results">The vaue of the results to add.</param>
        public void AddPackageResults(string culture, XmlValidationResults results)
        {
            if (!_packageValidationResults.ContainsKey(culture))
            {
                _packageValidationResults.Add(culture, results);
            }
            else
            {
                System.Diagnostics.Debug.Fail("Validation results have already been added for culture '{0}'", culture);
            }
        }

        /// <summary>
        /// Gets the XmlValidationResults for the specified culture.
        /// </summary>
        /// <param name="culture">The culture of the XmlValidationResults to get.</param>
        /// <returns>The XmlValidationResults associated with the specified culture.</returns>
        public XmlValidationResults PackageResults(string culture)
        {
            _packageValidationResults.TryGetValue(culture, out XmlValidationResults results);
            return results;
        }
    }
}
