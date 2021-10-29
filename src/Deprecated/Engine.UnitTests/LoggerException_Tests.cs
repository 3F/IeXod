// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using NUnit.Framework;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.BuildEngine;
using System.Text.RegularExpressions;


namespace net.r_eg.IeXod.UnitTests
{
    [TestFixture]
    public class InternalLoggerExceptionTests
    {
        /// <summary>
        /// Verify I implemented ISerializable correctly
        /// </summary>
        /// <owner>LukaszG</owner>
        [Test]
        public void SerializeDeserialize()
        {
            InternalLoggerException e = new InternalLoggerException("message", 
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

                InternalLoggerException e2 = (InternalLoggerException) frm.Deserialize(memstr);

                Assertion.AssertEquals(e.BuildEventArgs.Message, e2.BuildEventArgs.Message);
                Assertion.AssertEquals(e.BuildEventArgs.HelpKeyword, e2.BuildEventArgs.HelpKeyword);
                Assertion.AssertEquals(e.ErrorCode, e2.ErrorCode);
                Assertion.AssertEquals(e.HelpKeyword, e2.HelpKeyword);
                Assertion.AssertEquals(e.Message, e2.Message);
                Assertion.AssertEquals(e.InnerException.Message, e2.InnerException.Message);
            }
        }
    }
}





