// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace net.r_eg.IeXod.Collections
{
    /// <summary>
    /// Enumerable that uses a provided Converter delegate to
    /// convert each item from a backing enumerator as it is returned.
    /// </summary>
    /// <typeparam name="TFrom">Type of underlying enumerator</typeparam>
    /// <typeparam name="TTo">Type returned</typeparam>
    internal class ConvertingEnumerable<TFrom, TTo> : IEnumerable<TTo>
    {
        /// <summary>
        /// Enumerable behind this one
        /// </summary>
        private readonly IEnumerable<TFrom> _backingEnumerable;

        /// <summary>
        /// Converter delegate used on each item in the backing enumerable as it is returned
        /// </summary>
        private readonly Func<TFrom, TTo> _converter;

        /// <summary>
        /// Constructor
        /// </summary>
        internal ConvertingEnumerable(IEnumerable<TFrom> backingEnumerable, Func<TFrom, TTo> converter)
        {
            _backingEnumerable = backingEnumerable;
            _converter = converter;
        }

        /// <summary>
        /// Gets the converting enumerator
        /// </summary>
        public IEnumerator<TTo> GetEnumerator()
        {
            return new ConvertingEnumerator<TFrom, TTo>(_backingEnumerable.GetEnumerator(), _converter);
        }

        /// <summary>
        /// IEnumerable version of GetEnumerator
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Enumerable that uses a provided Converter delegate to
        /// convert each item from a backing enumerator as it is returned.
        /// </summary>
        /// <typeparam name="TFrom2">Type of underlying enumerator</typeparam>
        /// <typeparam name="TTo2">Type returned</typeparam>
        private struct ConvertingEnumerator<TFrom2, TTo2> : IEnumerator<TTo2>
        {
            /// <summary>
            /// Enumerator behind this one
            /// </summary>
            private readonly IEnumerator<TFrom2> _backingEnumerator;

            /// <summary>
            /// Converter delegate used on each item in the backing enumerator as it is returned
            /// </summary>
            private readonly Func<TFrom2, TTo2> _converter;

            /// <summary>
            /// Constructor
            /// </summary>
            internal ConvertingEnumerator(IEnumerator<TFrom2> backingEnumerator, Func<TFrom2, TTo2> converter)
            {
                _backingEnumerator = backingEnumerator;
                _converter = converter;
            }

            /// <summary>
            /// Get the current element, converted
            /// </summary>
            public TTo2 Current
            {
                get
                {
                    TFrom2 current = _backingEnumerator.Current;

                    return _converter(current);
                }
            }

            /// <summary>
            /// Get the current element, converted
            /// </summary>
            Object IEnumerator.Current => Current;

            /// <summary>
            /// Move to the next element
            /// </summary>
            public bool MoveNext()
            {
                return _backingEnumerator.MoveNext();
            }

            /// <summary>
            /// Reset the enumerator
            /// </summary>
            public void Reset()
            {
                _backingEnumerator.Reset();
            }

            /// <summary>
            /// Dispose of the enumerator
            /// </summary>
            public void Dispose()
            {
                _backingEnumerator.Dispose();
            }
        }
    }
}
