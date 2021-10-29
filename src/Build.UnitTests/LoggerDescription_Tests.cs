// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using net.r_eg.IeXod.Framework;
using Xunit;
using net.r_eg.IeXod.Logging;

namespace net.r_eg.IeXod.UnitTests
{
    public class LoggerDescription_Tests
    {
        [Fact]
        public void LoggerDescriptionCustomSerialization()
        {
            string className = "Class";
            string loggerAssemblyName = "Class";
            string loggerFileAssembly = null;
            string loggerSwitchParameters = "Class";
            LoggerVerbosity verbosity = LoggerVerbosity.Detailed;

            LoggerDescription description = new LoggerDescription(className, loggerAssemblyName, loggerFileAssembly, loggerSwitchParameters, verbosity);
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            BinaryReader reader = new BinaryReader(stream);
            try
            {
                stream.Position = 0;
                description.WriteToStream(writer);
                long streamWriteEndPosition = stream.Position;
                stream.Position = 0;
                LoggerDescription description2 = new LoggerDescription();
                description2.CreateFromStream(reader);
                long streamReadEndPosition = stream.Position;
                Assert.Equal(streamWriteEndPosition, streamReadEndPosition); // "Stream end positions should be equal"

                Assert.Equal(description.Verbosity, description2.Verbosity); // "Expected Verbosity to Match"
                Assert.Equal(description.LoggerId, description2.LoggerId); // "Expected Verbosity to Match"
                Assert.Equal(0, string.Compare(description.LoggerSwitchParameters, description2.LoggerSwitchParameters, StringComparison.OrdinalIgnoreCase)); // "Expected LoggerSwitchParameters to Match"
                Assert.Equal(0, string.Compare(description.Name, description2.Name, StringComparison.OrdinalIgnoreCase)); // "Expected Name to Match"
            }
            finally
            {
                reader.Dispose();
                writer = null;
                stream = null;
            }
        }
    }
}
