// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace net.r_eg.IeXod.Compatibility
{
    internal static class ArgTranslator
    {
        internal static object Translate(object raw, Type native)
        {
            if(native == typeof(bool)) return ToBoolean((string)raw);

            if(native == typeof(double)) return Convert.ToDouble(raw);
            if(native == typeof(float)) return Convert.ToSingle(raw);

            if(native == typeof(ulong)) return Convert.ToUInt64(raw);
            if(native == typeof(uint)) return Convert.ToUInt32(raw);
            if(native == typeof(ushort)) return Convert.ToUInt16(raw);
            if(native == typeof(byte)) return Convert.ToByte(raw);

            if(native == typeof(long)) return Convert.ToInt64(raw);
            if(native == typeof(int)) return Convert.ToInt32(raw);
            if(native == typeof(short)) return Convert.ToInt16(raw);
            if(native == typeof(sbyte)) return Convert.ToSByte(raw);

            if(native == typeof(IntPtr)) return new IntPtr(Convert.ToInt64(raw));
            if(native == typeof(UIntPtr)) return new UIntPtr(Convert.ToUInt64(raw));

            if(native == typeof(Version)) return new Version((string)raw);

            return raw;
        }

        private static bool ToBoolean(string s)
        {
            switch(s?.ToLowerInvariant())
            {
                case "true":
                case "1":
                case "yes":
                case "y":
                {
                    return true;
                }
            }
            return false;
        }
    }
}
