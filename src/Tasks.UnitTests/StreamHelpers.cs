// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
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
    sealed internal class StreamHelpers
    {
        /*
         * Method:  StringToStream (overload)
         * 
         * Take a string and convert it into a Stream.
         * Use the default encoding which means this machine's ANSI codepage.
         */
        static internal Stream StringToStream(string value)
        {
#if FEATURE_ENCODING_DEFAULT
            return StringToStream(value, System.Text.Encoding.Default); // We want this to be Default which is ANSI
#else
            return StringToStream(value, System.Text.Encoding.UTF8); // We want this to be Default which is ANSI
#endif
        }

        /*
         * Method:  StringToStream (overload)
         * 
         * Take a string and convert it into a Stream.
         * Takes an alternate encoding type
         */
        static internal Stream StringToStream(string value, System.Text.Encoding encoding)
        {
            MemoryStream m = new MemoryStream();
            TextWriter w = new StreamWriter(m, encoding); // HIGHCHAR: StringToStream helper accepts encoding from caller.

            w.Write(value);
            w.Flush();
            m.Seek(0, SeekOrigin.Begin);
            return m;
        }
    }
}
