﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.Construction;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Shared;
using net.r_eg.IeXod.BackEnd;
using System;

namespace net.r_eg.IeXod.UnitTests
{
    /// <summary>
    /// A dummy element location.
    /// </summary>
    public class MockElementLocation : ElementLocation
    {
        /// <summary>
        /// Single instance
        /// </summary>
        private static MockElementLocation s_instance = new MockElementLocation();

        private string _file = "mock.targets";


        /// <summary>
        /// Initializes a new instance of the MockElementLocation class.
        /// </summary>
        /// <param name="file">The path of the file to use.</param>
        public MockElementLocation(string file)
        {
            _file = file;
        }

        /// <summary>
        /// Private constructor
        /// </summary>
        private MockElementLocation()
        {
        }

        /// <summary>
        /// File of element, eg a targets file
        /// </summary>
        public override string File
        {
            get { return _file; }
        }

        /// <summary>
        /// Line number
        /// </summary>
        public override int Line
        {
            get { return 0; }
        }

        /// <summary>
        /// Column number
        /// </summary>
        public override int Column
        {
            get { return 1; }
        }

        /// <summary>
        /// Get single instance
        /// </summary>
        internal static MockElementLocation Instance
        {
            get { return s_instance; }
        }
    }
}