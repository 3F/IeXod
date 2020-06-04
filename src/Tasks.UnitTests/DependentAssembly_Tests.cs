// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;

using net.r_eg.IeXod.Tasks;
using Xunit;

namespace net.r_eg.IeXod.UnitTests
{
    public class DependentAssembly_Tests
    {
        /// <summary>
        /// Verify that a reference without a public key works correctly
        /// </summary>
        [Fact]
        public void SerializeDeserialize()
        {
            var dependentAssembly = new DependentAssembly();

            string xml = "<assemblyIdentity name='ClassLibrary1'/>";

            dependentAssembly.Read(new XmlTextReader(xml, XmlNodeType.Document, null));

            Assert.NotNull(dependentAssembly.PartialAssemblyName);
        }
    }
}
