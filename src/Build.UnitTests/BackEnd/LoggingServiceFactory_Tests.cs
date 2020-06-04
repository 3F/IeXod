// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.BackEnd.Logging;
using net.r_eg.IeXod.BackEnd;
using Xunit;

namespace net.r_eg.IeXod.UnitTests.Logging
{
    /// <summary>
    ///Test the Factory to create components of the type LoggingService
    /// </summary>
    public class LoggingServiceFactory_Tests
    {
        /// <summary>
        /// Verify we can create a synchronous LoggingService
        /// </summary>
        [Fact]
        public void TestCreateSynchronousLogger()
        {
            LoggingServiceFactory factory = new LoggingServiceFactory(LoggerMode.Synchronous, 1);
            LoggingService loggingService = (LoggingService)factory.CreateInstance(BuildComponentType.LoggingService);
            Assert.Equal(LoggerMode.Synchronous, loggingService.LoggingMode); // "Expected to create a Synchronous LoggingService"
        }

        /// <summary>
        /// Verify we can create a Asynchronous LoggingService
        /// </summary>
        [Fact]
        public void TestCreateAsynchronousLogger()
        {
            LoggingServiceFactory factory = new LoggingServiceFactory(LoggerMode.Asynchronous, 1);
            LoggingService loggingService = (LoggingService)factory.CreateInstance(BuildComponentType.LoggingService);
            Assert.Equal(LoggerMode.Asynchronous, loggingService.LoggingMode); // "Expected to create an Asynchronous LoggingService"
        }
    }
}