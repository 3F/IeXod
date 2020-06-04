// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using NUnit.Framework;

using net.r_eg.IeXod.BuildEngine;

namespace net.r_eg.IeXod.UnitTests
{
    [TestFixture]
    public class LocalNode_Tests
    {
        /// <summary>
        /// Verify when an exception is sent to the DumpExceptionToFile method, that the exception is exception is written to disk
        /// </summary>
        [Test]
        public void TestReportUnhandledException()
        {
            Exception testException = new Exception("Test Exception");
            Exception testException2 = new Exception("Test Exception2");
            string dumpFile = null;
            try
            {
                // Write the exception to the dump file

                LocalNode.DumpExceptionToFile(testException);
                LocalNode.DumpExceptionToFile(testException2);
                dumpFile = LocalNode.DumpFileName;

                // Read the file and the contents out and make sure they match what is expected
                using (StreamReader reader = new StreamReader(dumpFile))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if (i == 0)
                        {
                            Assert.IsTrue(String.Compare(reader.ReadLine(), "UNHANDLED EXCEPTIONS FROM CHILD NODE:", StringComparison.OrdinalIgnoreCase) == 0);
                            Assert.IsTrue(String.Compare(reader.ReadLine(), "===================", StringComparison.OrdinalIgnoreCase) == 0);
                            //Skip over the time stamp.
                            reader.ReadLine();
                            // Make sure the exception message is there
                            Assert.IsTrue(reader.ReadLine().Contains("Test Exception"));
                            Assert.IsTrue(String.Compare(reader.ReadLine(), "===================", StringComparison.OrdinalIgnoreCase) == 0);
                        }
                        else
                        {
                            //Skip over the time stamp.
                            reader.ReadLine();
                            Assert.IsTrue(reader.ReadLine().Contains("Test Exception2"));
                            Assert.IsTrue(String.Compare(reader.ReadLine(), "===================", StringComparison.OrdinalIgnoreCase) == 0);
                        }
                    }
                }
            }
            finally
            {
                if (File.Exists(dumpFile))
                {
                    File.Delete(dumpFile);
                }
            }
        }
    }
}
