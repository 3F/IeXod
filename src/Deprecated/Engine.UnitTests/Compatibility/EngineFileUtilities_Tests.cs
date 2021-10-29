// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using net.r_eg.IeXod;
using net.r_eg.IeXod.BuildEngine;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.UnitTests;

namespace net.r_eg.IeXod.UnitTests.OM.OrcasCompatibility
{
    /// <summary>
    /// Test Fixture Class for the v9 Object Model Public Interface Compatibility Tests for the EngineFileUtilities Class. 
    /// This is not a PRI 1 class for coverage
    /// </summary>
    [TestFixture]
    public class EngineFileUtilities_Tests
    {
        /// <summary>
        /// Test for thrown InternalErrorException when escaping a null string
        /// </summary>
        /// <remarks>found by kevinpi, Managed Lanaguages Team</remarks>
        [Test]
        public void EscapeString_Null() 
        {
            try
            {
                net.r_eg.IeXod.BuildEngine.Utilities.Escape(null);
                Assertion.Fail(); // Should not get here.
            }
            catch (Exception e)
            {
                Assertion.AssertEquals(true, e.GetType().ToString().Contains("InternalErrorException"));
            }
        }
    }
}
