// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;


namespace net.r_eg.IeXod.UnitTests
{
    sealed internal class StreamHelpers
    {
        /// <summary>
        /// Take a string and convert it to a StreamReader.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static internal StreamReader StringToStreamReader(string value)
        {
            MemoryStream m = new MemoryStream();
            TextWriter w = new StreamWriter(m, System.Text.Encoding.Default);

            w.Write(value);
            w.Flush();
            m.Seek(0, SeekOrigin.Begin);

            return new StreamReader(m);
        }
    }
}
