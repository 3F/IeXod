// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Globalization;

namespace net.r_eg.IeXod.Tasks.Deployment.ManifestUtilities
{
    internal static class ConvertUtil
    {
        public static bool ToBoolean(string value)
        {
            return ToBoolean(value, false);
        }

        public static bool ToBoolean(string value, bool defaultValue)
        {
            if (!String.IsNullOrEmpty(value))
            {
                try
                {
                    return Convert.ToBoolean(value, CultureInfo.InvariantCulture);
                }
                catch (FormatException)
                {
                    Debug.Fail(String.Format(CultureInfo.CurrentCulture, "Invalid value '{0}' for {1}, returning {2}", value, typeof(bool).Name, defaultValue.ToString()));
                }
                catch (ArgumentException)
                {
                    Debug.Fail(String.Format(CultureInfo.CurrentCulture, "Invalid value '{0}' for {1}, returning {2}", value, typeof(bool).Name, defaultValue.ToString()));
                }
            }
            return defaultValue;
        }
    }
}
