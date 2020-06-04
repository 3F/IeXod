// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Exceptions;
using System.Text.RegularExpressions;
using Xunit;
using System.Runtime.Serialization.Formatters.Binary;

namespace net.r_eg.IeXod.UnitTests
{
    public class InternalLoggerExceptionTests
    {
        /// <summary>
        /// Verify I implemented ISerializable correctly
        /// </summary>
        [Fact]
        public void SerializeDeserialize()
        {
            InternalLoggerException e = new InternalLoggerException(
                "message",
                new Exception("innerException"),
                new BuildStartedEventArgs("evMessage", "evHelpKeyword"),
                "errorCode",
                "helpKeyword",
                false);

            using (MemoryStream memstr = new MemoryStream())
            {
                BinaryFormatter frm = new BinaryFormatter();

                frm.Serialize(memstr, e);
                memstr.Position = 0;

                InternalLoggerException e2 = (InternalLoggerException)frm.Deserialize(memstr);

                Assert.Equal(e.BuildEventArgs.Message, e2.BuildEventArgs.Message);
                Assert.Equal(e.BuildEventArgs.HelpKeyword, e2.BuildEventArgs.HelpKeyword);
                Assert.Equal(e.ErrorCode, e2.ErrorCode);
                Assert.Equal(e.HelpKeyword, e2.HelpKeyword);
                Assert.Equal(e.Message, e2.Message);
                Assert.Equal(e.InnerException.Message, e2.InnerException.Message);
            }
        }
    }
}
