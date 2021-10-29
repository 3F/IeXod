// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;
using System.Runtime.InteropServices;

namespace net.r_eg.IeXod.Tasks.Deployment.Bootstrapper
{
    [ComVisible(false)]
    internal class Package
    {
        public Package(Product product, XmlNode node, XmlValidationResults validationResults, string name, string culture)
        {
            Product = product;
            Node = node;
            Name = name;
            Culture = culture;
            ValidationResults = validationResults;
        }

        internal XmlNode Node { get; }

        public string Name { get; }

        public string Culture { get; }

        public Product Product { get; }

        internal bool ValidationPassed
        {
            get
            {
                if (ValidationResults == null)
                {
                    return true;
                }
                return ValidationResults.ValidationPassed;
            }
        }

        internal XmlValidationResults ValidationResults { get; }
    }
}
