// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Reflection;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Tasks;
using net.r_eg.IeXod.Utilities;
using Xunit;

namespace net.r_eg.IeXod.UnitTests
{
    sealed public class Culture_Tests
    {
        /*
        * Method:   Basic
        *
        * Test the basic functionality.
        */
        [Fact]
        public void Basic()
        {
            Culture.ItemCultureInfo info = Culture.GetItemCultureInfo("MyResource.fr.resx", null);
            Assert.Equal("fr", info.culture);
            Assert.Equal("MyResource.resx", info.cultureNeutralFilename);
        }

        /*
        * Method:   NonCultureFile
        *
        * The item doesn't have a culture, and there isn't one embedded in the file name.
        */
        [Fact]
        public void NonCultureFile()
        {
            Culture.ItemCultureInfo info = Culture.GetItemCultureInfo("MyResource.resx", null);
            Assert.Null(info.culture);
            Assert.Equal("MyResource.resx", info.cultureNeutralFilename);
        }


        /*
        * Method:   BogusEmbeddedCulture
        *
        * The item has something that looks like an embedded culture, but isn't.
        */
        [Fact]
        [Trait("Category", "netcore-osx-failing")]
        [Trait("Category", "netcore-linux-failing")]
        public void BogusEmbeddedCulture()
        {
            Culture.ItemCultureInfo info = Culture.GetItemCultureInfo("MyResource.notalocale.resx", null);
            Assert.Null(info.culture);
            Assert.Equal("MyResource.notalocale.resx", info.cultureNeutralFilename);
        }
    }
}



