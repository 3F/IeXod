// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace net.r_eg.IeXod.Tasks.Deployment.Bootstrapper
{
    /// <summary>
    /// A buildable version of a Product.  Used for the BootstrapperBuilder's Build method.
    /// </summary>
    public class ProductBuilder : IProductBuilder
    {
        internal ProductBuilder(Product product)
        {
            Product = product;
        }

        /// <summary>
        /// The Product corresponding to this ProductBuilder
        /// </summary>
        public Product Product { get; }

        internal string Name => Product.Name;

        internal string ProductCode => Product.ProductCode;
    }
}
