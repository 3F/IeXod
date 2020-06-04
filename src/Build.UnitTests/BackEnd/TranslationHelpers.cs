﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.BackEnd;

namespace net.r_eg.IeXod.UnitTests.BackEnd
{
    /// <summary>
    /// Class containing methods used to assist in testing serialization methods.
    /// </summary>
    internal static class TranslationHelpers
    {
        /// <summary>
        /// The stream backing the serialization classes.
        /// </summary>
        private static MemoryStream s_serializationStream;

        /// <summary>
        /// Gets a serializer used to write data.  Note that only one such serializer may be used from this class at a time.
        /// </summary>
        internal static ITranslator GetWriteTranslator()
        {
            s_serializationStream = new MemoryStream();
            return BinaryTranslator.GetWriteTranslator(s_serializationStream);
        }

        /// <summary>
        /// Gets a serializer used to read data.  Note that only one such serializer may be used from this class at a time,
        /// and this must be called after GetWriteTranslator() has been called.
        /// </summary>
        internal static ITranslator GetReadTranslator()
        {
            s_serializationStream.Seek(0, SeekOrigin.Begin);
            return BinaryTranslator.GetReadTranslator(s_serializationStream, null);
        }

        /// <summary>
        /// Compares two collections.
        /// </summary>
        /// <typeparam name="T">The collections element type.</typeparam>
        /// <param name="left">The left collections.</param>
        /// <param name="right">The right collections.</param>
        /// <param name="comparer">The comparer to use on each element.</param>
        /// <returns>True if the collections are equivalent.</returns>
        internal static bool CompareCollections<T>(ICollection<T> left, ICollection<T> right, IComparer<T> comparer)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if ((left == null) ^ (right == null))
            {
                return false;
            }

            if (left.Count != right.Count)
            {
                return false;
            }

            T[] leftArray = left.ToArray();
            T[] rightArray = right.ToArray();

            for (int i = 0; i < leftArray.Length; i++)
            {
                if (comparer.Compare(leftArray[i], rightArray[i]) != 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Compares two exceptions.
        /// </summary>
        internal static bool CompareExceptions(Exception left, Exception right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if ((left == null) ^ (right == null))
            {
                return false;
            }

            if (left.Message != right.Message)
            {
                return false;
            }

            if (left.StackTrace != right.StackTrace)
            {
                return false;
            }

            return CompareExceptions(left.InnerException, right.InnerException);
        }
    }
}
