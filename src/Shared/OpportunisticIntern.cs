﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
#if !CLR2COMPATIBILITY
using System.Collections.Concurrent;
#endif
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using net.r_eg.IeXod.Shared;
using net.r_eg.IeXod.Utilities;

namespace net.r_eg.IeXod
{
    /// <summary>
    /// This class is used to selectively intern strings. It should be used at the point of new string creation.
    /// For example,
    ///
    ///     string interned = OpportunisticIntern.Intern(String.Join(",",someStrings));
    ///
    /// This class uses heuristics to decide whether it will be efficient to intern a string or not. There is no
    /// guarantee that a string will intern.
    ///
    /// The thresholds and sizes were determined by experimentation to give the best number of bytes saved
    /// at reasonable elapsed time cost.
    /// </summary>
    internal static class OpportunisticIntern
    {
        private static readonly bool s_useSimpleConcurrency = Traits.Instance.UseSimpleInternConcurrency;

        /// <summary>
        /// The size of the small mru list.
        /// </summary>
        private static readonly int s_smallMruSize = AssignViaEnvironment("MSBUILDSMALLINTERNSIZE", 50);

        /// <summary>
        /// The size of the large mru list.
        /// </summary>
        private static readonly int s_largeMruSize = AssignViaEnvironment("MSBUILDLARGEINTERNSIZE", 100);

        /// <summary>
        /// The size of the huge mru list.
        /// </summary>
        private static readonly int s_hugeMruSize = AssignViaEnvironment("MSBUILDHUGEINTERNSIZE", 100);

        /// <summary>
        /// The smallest size a string can be to be considered small.
        /// </summary>
        private static readonly int s_smallMruThreshold = AssignViaEnvironment("MSBUILDSMALLINTERNTHRESHOLD", 50);

        /// <summary>
        /// The smallest size a string can be to be considered large.
        /// </summary>
        private static readonly int s_largeMruThreshold = AssignViaEnvironment("MSBUILDLARGEINTERNTHRESHOLD", 70);

        /// <summary>
        /// The smallest size a string can be to be considered huge.
        /// </summary>
        private static readonly int s_hugeMruThreshold = AssignViaEnvironment("MSBUILDHUGEINTERNTHRESHOLD", 200);

        /// <summary>
        /// The smallest size a string can be to be ginormous.
        /// 8K for large object heap.
        /// </summary>
        private static readonly int s_ginormousThreshold = AssignViaEnvironment("MSBUILDGINORMOUSINTERNTHRESHOLD", 8000);

        /// <summary>
        /// Manages the separate MRU lists.
        /// </summary>
        private static BucketedPrioritizedStringList s_si = new BucketedPrioritizedStringList(/*gatherStatistics*/ false, s_smallMruSize, s_largeMruSize, s_hugeMruSize, s_smallMruThreshold, s_largeMruThreshold, s_hugeMruThreshold, s_ginormousThreshold, s_useSimpleConcurrency);

        #region Statistics
        /// <summary>
        /// What if Mru lists were infinitely long?
        /// </summary>
        private static BucketedPrioritizedStringList s_whatIfInfinite;

        /// <summary>
        /// What if we doubled the size of the Mru lists?
        /// </summary>
        private static BucketedPrioritizedStringList s_whatIfDoubled;

        /// <summary>
        /// What if we halved the size of the Mru lists?
        /// </summary>
        private static BucketedPrioritizedStringList s_whatIfHalved;

        /// <summary>
        /// What if the size of Mru lists was zero? (We still intern tiny strings in this case)
        /// </summary>
        private static BucketedPrioritizedStringList s_whatIfZero;
        #endregion

        #region IInternable
        /// <summary>
        /// Define the methods needed to intern something.
        /// </summary>
        internal interface IInternable
        {
            /// <summary>
            /// The length of the target.
            /// </summary>
            int Length { get; }

            /// <summary>
            /// Indexer into the target. Presumed to be fast.
            /// </summary>
            char this[int index] { get; }

            /// <summary>
            /// Convert target to string. Presumed to be slow (and will be called just once).
            /// </summary>
            string ExpensiveConvertToString();

            /// <summary>
            /// Compare target to string. Assumes string is of equal or smaller length than target.
            /// </summary>
            bool StartsWithStringByOrdinalComparison(string other);

            /// <summary>
            /// Reference compare target to string. If target is non-string this should return false.
            /// </summary>
            bool ReferenceEquals(string other);
        }
        #endregion

        /// <summary>
        /// Assign an int from an environment variable. If its not present, use the default.
        /// </summary>
        internal static int AssignViaEnvironment(string env, int @default)
        {
            string threshold = Environment.GetEnvironmentVariable(env);
            if (!string.IsNullOrEmpty(threshold))
            {
                if (int.TryParse(threshold, out int result))
                {
                    return result;
                }
            }

            return @default;
        }

        /// <summary>
        /// Turn on statistics gathering.
        /// </summary>
        internal static void EnableStatisticsGathering()
        {
            // Statistics include several 'what if' scenarios such as doubling the size of the MRU lists.
            s_si = new BucketedPrioritizedStringList(/*gatherStatistics*/ true, s_smallMruSize, s_largeMruSize, s_hugeMruSize, s_smallMruThreshold, s_largeMruThreshold, s_hugeMruThreshold, s_ginormousThreshold, s_useSimpleConcurrency);
            s_whatIfInfinite = new BucketedPrioritizedStringList(/*gatherStatistics*/ true, int.MaxValue, int.MaxValue, int.MaxValue, s_smallMruThreshold, s_largeMruThreshold, s_hugeMruThreshold, s_ginormousThreshold, s_useSimpleConcurrency);
            s_whatIfDoubled = new BucketedPrioritizedStringList(/*gatherStatistics*/ true, s_smallMruSize * 2, s_largeMruSize * 2, s_hugeMruSize * 2, s_smallMruThreshold, s_largeMruThreshold, s_hugeMruThreshold, s_ginormousThreshold, s_useSimpleConcurrency);
            s_whatIfHalved = new BucketedPrioritizedStringList(/*gatherStatistics*/ true, s_smallMruSize / 2, s_largeMruSize / 2, s_hugeMruSize / 2, s_smallMruThreshold, s_largeMruThreshold, s_hugeMruThreshold, s_ginormousThreshold, s_useSimpleConcurrency);
            s_whatIfZero = new BucketedPrioritizedStringList(/*gatherStatistics*/ true, 0, 0, 0, s_smallMruThreshold, s_largeMruThreshold, s_hugeMruThreshold, s_ginormousThreshold, s_useSimpleConcurrency);
        }

        /// <summary>
        /// Intern the given internable.
        /// </summary>
        internal static string InternableToString<T>(T candidate) where T : IInternable
        {
            if (s_whatIfInfinite != null)
            {
                s_whatIfInfinite.InterningToString(candidate);
                s_whatIfDoubled.InterningToString(candidate);
                s_whatIfHalved.InterningToString(candidate);
                s_whatIfZero.InterningToString(candidate);
            }

            string result = s_si.InterningToString(candidate);
#if DEBUG
            string expected = candidate.ExpensiveConvertToString();
            if (!String.Equals(result, expected))
            {
                ErrorUtilities.ThrowInternalError("Interned string {0} should have been {1}", result, expected);
            }
#endif
            return result;
        }

        /// <summary>
        /// Potentially Intern the given string builder.
        /// </summary>
        internal static string StringBuilderToString(StringBuilder candidate)
        {
            return InternableToString(new StringBuilderInternTarget(candidate));
        }

        /// <summary>
        /// Potentially Intern the given char array.
        /// </summary>
        internal static string CharArrayToString(char[] candidate, int count)
        {
            return InternableToString(new CharArrayInternTarget(candidate, count));
        }

        /// <summary>
        /// Potentially Intern the given char array.
        /// </summary>
        internal static string CharArrayToString(char[] candidate, int startIndex, int count)
        {
            return InternableToString(new CharArrayInternTarget(candidate, startIndex, count));
        }

        /// <summary>
        /// Potentially Intern the given string.
        /// </summary>
        /// <param name="candidate">The string to intern.</param>
        /// <returns>The interned string, or the same string if it could not be interned.</returns>
        internal static string InternStringIfPossible(string candidate)
        {
            return InternableToString(new StringInternTarget(candidate));
        }

        /// <summary>
        /// Report statistics about interning. Don't call unless GatherStatistics has been called beforehand.
        /// </summary>
        internal static void ReportStatistics()
        {
            s_si.ReportStatistics("Main");
            s_whatIfInfinite.ReportStatistics("if Infinite");
            s_whatIfDoubled.ReportStatistics("if Doubled");
            s_whatIfHalved.ReportStatistics("if Halved");
            s_whatIfZero.ReportStatistics("if Zero");
            Console.WriteLine(" * Even for MRU size of zero there will still be some intern hits because of the tiny ");
            Console.WriteLine("   string matching (eg. 'true')");
        }

        #region IInternable Implementations
        /// <summary>
        /// A wrapper over StringBuilder.
        /// </summary>
        internal struct StringBuilderInternTarget : IInternable
        {
            /// <summary>
            /// The held StringBuilder
            /// </summary>
            private readonly StringBuilder _target;

            /// <summary>
            /// Pointless comment about constructor.
            /// </summary>
            internal StringBuilderInternTarget(StringBuilder target)
            {
                _target = target;
            }

            /// <summary>
            /// The length of the target.
            /// </summary>
            public int Length => _target.Length;

            /// <summary>
            /// Indexer into the target. Presumed to be fast.
            /// </summary>
            public char this[int index] => _target[index];

            /// <summary>
            /// Never reference equals to string.
            /// </summary>
            public bool ReferenceEquals(string other) => false;

            /// <summary>
            /// Convert target to string. Presumed to be slow (and will be called just once).
            /// </summary>
            public string ExpensiveConvertToString()
            {
                // PERF NOTE: This will be an allocation hot-spot because the StringBuilder is finally determined to
                // not be internable. There is still only one conversion of StringBuilder into string it has just
                // moved into this single spot.
                return _target.ToString();
            }

            /// <summary>
            /// Compare target to string. Assumes string is of equal or smaller length than target.
            /// </summary>
            public bool StartsWithStringByOrdinalComparison(string other)
            {
#if DEBUG
                ErrorUtilities.VerifyThrow(other.Length <= _target.Length, "should be at most as long as target");
#endif
                int length = other.Length;

                // Backwards because the end of the string is (by observation of Australian Government build) more likely to be different earlier in the loop.
                // For example, C:\project1, C:\project2
                for (int i = length - 1; i >= 0; --i)
                {
                    if (_target[i] != other[i])
                    {
                        return false;
                    }
                }

                return true;
            }

            /// <summary>
            /// Don't use this function. Use ExpensiveConvertToString
            /// </summary>
            public override string ToString() => throw new InvalidOperationException();
        }

        /// <summary>
        /// A wrapper over char[].
        /// </summary>
        internal struct CharArrayInternTarget : IInternable
        {
            /// <summary>
            /// Start index for the string
            /// </summary>
            private readonly int _startIndex;

            /// <summary>
            /// The held array
            /// </summary>
            private readonly char[] _target;

            /// <summary>
            /// Pointless comment about constructor.
            /// </summary>
            internal CharArrayInternTarget(char[] target, int count)
                : this(target, 0, count)
            {
            }

            /// <summary>
            /// Pointless comment about constructor.
            /// </summary>
            internal CharArrayInternTarget(char[] target, int startIndex, int count)
            {
#if DEBUG
                if (startIndex + count > target.Length)
                {
                    ErrorUtilities.ThrowInternalError("wrong length");
                }
#endif
                _target = target;
                _startIndex = startIndex;
                Length = count;
            }

            /// <summary>
            /// The length of the target.
            /// </summary>
            public int Length { get; }

            /// <summary>
            /// Indexer into the target. Presumed to be fast.
            /// </summary>
            public char this[int index]
            {
                get
                {
                    if (index > _startIndex + Length - 1 || index < 0)
                    {
                        ErrorUtilities.ThrowInternalError("past end");
                    }

                    return _target[index + _startIndex];
                }
            }

            /// <summary>
            /// Convert target to string. Presumed to be slow (and will be called just once).
            /// </summary>
            public bool ReferenceEquals(string other)
            {
                return false;
            }

            /// <summary>
            /// Convert target to string. Presumed to be slow (and will be called just once).
            /// </summary>
            public string ExpensiveConvertToString()
            {
                // PERF NOTE: This will be an allocation hot-spot because the char[] is finally determined to
                // not be internable. There is still only one conversion of char[] into string it has just
                // moved into this single spot.
                return new string(_target, _startIndex, Length);
            }

            /// <summary>
            /// Compare target to string. Assumes string is of equal or smaller length than target.
            /// </summary>
            public bool StartsWithStringByOrdinalComparison(string other)
            {
#if DEBUG
                ErrorUtilities.VerifyThrow(other.Length <= Length, "should be at most as long as target");
#endif
                // Backwards because the end of the string is (by observation of Australian Government build) more likely to be different earlier in the loop.
                // For example, C:\project1, C:\project2
                for (int i = other.Length - 1; i >= 0; --i)
                {
                    if (_target[i + _startIndex] != other[i])
                    {
                        return false;
                    }
                }

                return true;
            }

            /// <summary>
            /// Don't use this function. Use ExpensiveConvertToString
            /// </summary>
            public override string ToString()
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Wrapper over a string.
        /// </summary>
        internal struct StringInternTarget : IInternable
        {
            /// <summary>
            /// Stores the wrapped string.
            /// </summary>
            private readonly string _target;

            /// <summary>
            /// Constructor of the class
            /// </summary>
            /// <param name="target">The string to wrap</param>
            internal StringInternTarget(string target)
            {
                ErrorUtilities.VerifyThrowArgumentLength(target, nameof(target));
                _target = target;
            }

            /// <summary>
            /// Gets the length of the target string.
            /// </summary>
            public int Length => _target.Length;

            /// <summary>
            /// Gets the n character in the target string.
            /// </summary>
            /// <param name="index">Index of the character to gather.</param>
            /// <returns>The character in the position marked by index.</returns>
            public char this[int index] => _target[index];

            /// <summary>
            /// Returns the target which is already a string.
            /// </summary>
            /// <returns>The target string.</returns>
            public string ExpensiveConvertToString() => _target;

            /// <summary>
            /// Compare target to string. Assumes string is of equal or smaller length than target.
            /// </summary>
            /// <param name="other">The string to compare with the target.</param>
            /// <returns>True if target starts with <paramref name="other"/>, false otherwise.</returns>
            public bool StartsWithStringByOrdinalComparison(string other) => _target.StartsWith(other, StringComparison.Ordinal);

            /// <summary>
            /// Verifies if the reference of the target string is the same of the given string.
            /// </summary>
            /// <param name="other">The string reference to compare to.</param>
            /// <returns>True if both references are equal, false otherwise.</returns>
            public bool ReferenceEquals(string other) => ReferenceEquals(_target, other);
        }

        /// <summary>
        /// Wrapper over a substring of a string.
        /// </summary>
        internal struct SubstringInternTarget : IInternable
        {
            /// <summary>
            /// Stores the wrapped string.
            /// </summary>
            private readonly string _target;

            /// <summary>
            /// Start index of the substring within the wrapped string.
            /// </summary>
            private readonly int _startIndex;

            /// <summary>
            /// Constructor of the class
            /// </summary>
            /// <param name="target">The string to wrap.</param>
            /// <param name="startIndex">Start index of the substring within <paramref name="target"/>.</param>
            /// <param name="length">Length of the substring.</param>
            internal SubstringInternTarget(string target, int startIndex, int length)
            {
#if DEBUG
                if (startIndex + length > target.Length)
                {
                    ErrorUtilities.ThrowInternalError("wrong length");
                }
#endif
                _target = target;
                _startIndex = startIndex;
                Length = length;
            }

            /// <summary>
            /// Gets the length of the target substring.
            /// </summary>
            public int Length { get; }

            /// <summary>
            /// Gets the n character in the target substring.
            /// </summary>
            /// <param name="index">Index of the character to gather.</param>
            /// <returns>The character in the position marked by index.</returns>
            public char this[int index] => _target[index + _startIndex];

            /// <summary>
            /// Returns the target substring as a string.
            /// </summary>
            /// <returns>The substring.</returns>
            public string ExpensiveConvertToString() => _target.Substring(_startIndex, Length);

            /// <summary>
            /// Compare target substring to a string. Assumes string is of equal or smaller length than the target substring.
            /// </summary>
            /// <param name="other">The string to compare with the target substring.</param>
            /// <returns>True if target substring starts with <paramref name="other"/>, false otherwise.</returns>
            public bool StartsWithStringByOrdinalComparison(string other) => (String.CompareOrdinal(_target, _startIndex, other, 0, other.Length) == 0);

            /// <summary>
            /// Never reference equals to string.
            /// </summary>
            public bool ReferenceEquals(string other) => false;
        }

        #endregion

        /// <summary>
        /// Manages a set of mru lists that hold strings in varying size ranges.
        /// </summary>
        private class BucketedPrioritizedStringList
        {
            /// <summary>
            /// The small string Mru list.
            /// </summary>
            private readonly PrioritizedStringList _smallMru;

            /// <summary>
            /// The large string Mru list.
            /// </summary>
            private readonly PrioritizedStringList _largeMru;

            /// <summary>
            /// The huge string Mru list.
            /// </summary>
            private readonly PrioritizedStringList _hugeMru;

            /// <summary>
            /// Three most recently used strings over 8K.
            /// </summary>
            private readonly LinkedList<WeakReference> _ginormous = new LinkedList<WeakReference>();

            /// <summary>
            /// The smallest size a string can be to be considered small.
            /// </summary>
            private readonly int _smallMruThreshold;

            /// <summary>
            /// The smallest size a string can be to be considered large.
            /// </summary>
            private readonly int _largeMruThreshold;

            /// <summary>
            /// The smallest size a string can be to be considered huge.
            /// </summary>
            private readonly int _hugeMruThreshold;

            /// <summary>
            /// The smallest size a string can be to be ginormous.
            /// </summary>
            private readonly int _ginormousThreshold;

            private readonly bool _useSimpleConcurrency;

#if !CLR2COMPATIBILITY
            // ConcurrentDictionary starts with capacity 31 but we're usually adding far more than that. Make a better first capacity guess to reduce
            // ConcurrentDictionary having to take all internal locks to upgrade its bucket list. Note that the number should be prime per the
            // comments on the code at https://referencesource.microsoft.com/#mscorlib/system/Collections/Concurrent/ConcurrentDictionary.cs,122 
            // Also note default lock count is Environment.ProcessorCount from the same code.
            private const int InitialCapacity = 2053;
            private readonly ConcurrentDictionary<string, string> _internedStrings = new ConcurrentDictionary<string, string>(Environment.ProcessorCount, InitialCapacity, StringComparer.Ordinal);
#endif

            #region Statistics
            /// <summary>
            /// Whether or not to gather statistics
            /// </summary>
            private readonly bool _gatherStatistics;

            /// <summary>
            /// Number of times interning worked.
            /// </summary>
            private int _internHits;

            /// <summary>
            /// Number of times interning didn't work.
            /// </summary>
            private int _internMisses;

            /// <summary>
            /// Number of times interning wasn't attempted.
            /// </summary>
            private int _internRejects;

            /// <summary>
            /// Total number of strings eliminated by interning.
            /// </summary>
            private int _internEliminatedStrings;

            /// <summary>
            /// Total number of chars eliminated across all strings.
            /// </summary>
            private int _internEliminatedChars;

            /// <summary>
            /// Number of times the ginourmous string hit.
            /// </summary>
            private int _ginormousHits;

            /// <summary>
            /// Number of times the ginourmous string missed.
            /// </summary>
            private int _ginormousMisses;

            /// <summary>
            /// Chars interned for ginormous range.
            /// </summary>
            private int _ginormousCharsSaved;

            /// <summary>
            /// Whether or not to track ginormous strings.
            /// </summary>
            private readonly bool _dontTrack;

            /// <summary>
            /// The time spent interning.
            /// </summary>
            private readonly Stopwatch _stopwatch;

            /// <summary>
            /// Strings which did not intern
            /// </summary>
            private readonly Dictionary<string, int> _missedStrings;

            /// <summary>
            /// Strings which we didn't attempt to intern
            /// </summary>
            private readonly Dictionary<string, int> _rejectedStrings;

            /// <summary>
            /// Number of ginormous strings to keep
            /// By observation of Auto7, there are about three variations of the huge solution config blob
            /// There aren't really any other strings of this size, but make it 10 to be sure. (There will barely be any misses)
            /// </summary>
            private const int GinormousSize = 10;

            #endregion

            /// <summary>
            /// Construct.
            /// </summary>
            internal BucketedPrioritizedStringList(bool gatherStatistics, int smallMruSize, int largeMruSize, int hugeMruSize, int smallMruThreshold, int largeMruThreshold, int hugeMruThreshold, int ginormousThreshold, bool useSimpleConcurrency)
            {
                if (smallMruSize == 0 && largeMruSize == 0 && hugeMruSize == 0)
                {
                    _dontTrack = true;
                }

                _smallMru = new PrioritizedStringList(smallMruSize);
                _largeMru = new PrioritizedStringList(largeMruSize);
                _hugeMru = new PrioritizedStringList(hugeMruSize);
                _smallMruThreshold = smallMruThreshold;
                _largeMruThreshold = largeMruThreshold;
                _hugeMruThreshold = hugeMruThreshold;
                _ginormousThreshold = ginormousThreshold;
                _useSimpleConcurrency = useSimpleConcurrency;

                for (int i = 0; i < GinormousSize; i++)
                {
                    _ginormous.AddFirst(new WeakReference(string.Empty));
                }

                _gatherStatistics = gatherStatistics;
                if (gatherStatistics)
                {
                    _stopwatch = new Stopwatch();
                    _missedStrings = new Dictionary<string, int>(StringComparer.Ordinal);
                    _rejectedStrings = new Dictionary<string, int>(StringComparer.Ordinal);
                }
            }

            /// <summary>
            /// Intern the given internable.
            /// </summary>
            internal string InterningToString<T>(T candidate) where T : IInternable
            {
                if (candidate.Length == 0)
                {
                    // As in the case that a property or itemlist has evaluated to empty.
                    return string.Empty;
                }

                if (_gatherStatistics)
                {
                    return InternWithStatistics(candidate);
                }
                else
                {
                    TryIntern(candidate, out string result);
                    return result;
                }
            }

            /// <summary>
            /// Report statistics to the console.
            /// </summary>
            internal void ReportStatistics(string heading)
            {
                string title = "Opportunistic Intern (" + heading + ")";
                Console.WriteLine("\n{0}{1}{0}", new string('=', 41 - (title.Length / 2)), title);
                Console.WriteLine("||{0,50}|{1,20:N0}|{2,8}|", "Intern Hits", _internHits, "hits");
                Console.WriteLine("||{0,50}|{1,20:N0}|{2,8}|", "Intern Misses", _internMisses, "misses");
                Console.WriteLine("||{0,50}|{1,20:N0}|{2,8}|", "Intern Rejects (as shorter than " + s_smallMruThreshold + " bytes)", _internRejects, "rejects");
                Console.WriteLine("||{0,50}|{1,20:N0}|{2,8}|", "Eliminated Strings*", _internEliminatedStrings, "strings");
                Console.WriteLine("||{0,50}|{1,20:N0}|{2,8}|", "Eliminated Chars", _internEliminatedChars, "chars");
                Console.WriteLine("||{0,50}|{1,20:N0}|{2,8}|", "Estimated Eliminated Bytes", _internEliminatedChars * 2, "bytes");
                Console.WriteLine("Elimination assumes that strings provided were unique objects.");
                Console.WriteLine("|---------------------------------------------------------------------------------|");
                KeyValuePair<int, int> held = _smallMru.Statistics();
                Console.WriteLine("||{0,50}|{1,20:N0}|{2,8}|", "Small Strings MRU Size", s_smallMruSize, "strings");
                Console.WriteLine("||{0,50}|{1,20:N0}|{2,8}|", "Small Strings (>=" + _smallMruThreshold + " chars) Held", held.Key, "strings");
                Console.WriteLine("||{0,50}|{1,20:N0}|{2,8}|", "Small Estimated Bytes Held", held.Value * 2, "bytes");
                Console.WriteLine("|---------------------------------------------------------------------------------|");
                held = _largeMru.Statistics();
                Console.WriteLine("||{0,50}|{1,20:N0}|{2,8}|", "Large Strings MRU Size", s_largeMruSize, "strings");
                Console.WriteLine("||{0,50}|{1,20:N0}|{2,8}|", "Large Strings  (>=" + _largeMruThreshold + " chars) Held", held.Key, "strings");
                Console.WriteLine("||{0,50}|{1,20:N0}|{2,8}|", "Large Estimated Bytes Held", held.Value * 2, "bytes");
                Console.WriteLine("|---------------------------------------------------------------------------------|");
                held = _hugeMru.Statistics();
                Console.WriteLine("||{0,50}|{1,20:N0}|{2,8}|", "Huge Strings MRU Size", s_hugeMruSize, "strings");
                Console.WriteLine("||{0,50}|{1,20:N0}|{2,8}|", "Huge Strings  (>=" + _hugeMruThreshold + " chars) Held", held.Key, "strings");
                Console.WriteLine("||{0,50}|{1,20:N0}|{2,8}|", "Huge Estimated Bytes Held", held.Value * 2, "bytes");
                Console.WriteLine("|---------------------------------------------------------------------------------|");
                Console.WriteLine("||{0,50}|{1,20:N0}|{2,8}|", "Ginormous Strings MRU Size", GinormousSize, "strings");
                Console.WriteLine("||{0,50}|{1,20:N0}|{2,8}|", "Ginormous (>=" + _ginormousThreshold + " chars)  Hits", _ginormousHits, "hits");
                Console.WriteLine("||{0,50}|{1,20:N0}|{2,8}|", "Ginormous Misses", _ginormousMisses, "misses");
                Console.WriteLine("||{0,50}|{1,20:N0}|{2,8}|", "Ginormous Chars Saved", _ginormousCharsSaved, "chars");
                Console.WriteLine("|---------------------------------------------------------------------------------|");

                // There's no point in reporting the ginormous string because it will have evaporated by now.
                Console.WriteLine("||{0,50}|{1,20:N0}|{2,8}|", "Time Spent Interning", _stopwatch.ElapsedMilliseconds, "ms");
                Console.WriteLine("{0}{0}", new string('=', 41));

                IEnumerable<string> topMissingString =
                    _missedStrings
                    .OrderByDescending(kv => kv.Value * kv.Key.Length)
                    .Take(15)
                    .Where(kv => kv.Value > 1)
                    .Select(kv => string.Format(CultureInfo.InvariantCulture, "({1} instances x each {2} chars = {3}KB wasted)\n{0}", kv.Key, kv.Value, kv.Key.Length, (kv.Value - 1) * kv.Key.Length * 2 / 1024));

                Console.WriteLine("##########Top Missed Strings:  \n{0} ", string.Join("\n==============\n", topMissingString.ToArray()));
                Console.WriteLine();

                IEnumerable<string> topRejectedString =
                    _rejectedStrings
                    .OrderByDescending(kv => kv.Value * kv.Key.Length)
                    .Take(15)
                    .Where(kv => kv.Value > 1)
                    .Select(kv => string.Format(CultureInfo.InvariantCulture, "({1} instances x each {2} chars = {3}KB wasted)\n{0}", kv.Key, kv.Value, kv.Key.Length, (kv.Value - 1) * kv.Key.Length * 2 / 1024));

                Console.WriteLine("##########Top Rejected Strings: \n{0} ", string.Join("\n==============\n", topRejectedString.ToArray()));
            }

            private bool TryInternHardcodedString<T>(T candidate, string str, ref string interned) where T : IInternable
            {
                Debug.Assert(candidate.Length == str.Length);

                if (candidate.StartsWithStringByOrdinalComparison(str))
                {
                    interned = str;
                    return true;
                }
                return false;
            }

            /// <summary>
            /// Try to intern the string.
            /// Return true if an interned value could be returned.
            /// Return false if it was added to the intern list, but wasn't there already.
            /// Return null if it didn't meet the length criteria for any of the buckets. Interning was rejected
            /// </summary>
            private bool? TryIntern<T>(T candidate, out string interned) where T : IInternable
            {
                int length = candidate.Length;
                interned = null;

                // First, try the hard coded intern strings.
                // Each of the hard-coded small strings below showed up in a profile run with considerable duplication in memory.
                if (!_dontTrack)
                {
                    if (length == 2)
                    {
                        if (candidate[1] == '#')
                        {
                            if (candidate[0] == 'C')
                            {
                                interned = "C#";
                                return true;
                            }

                            if (candidate[0] == 'F')
                            {
                                interned = "F#";
                                return true;
                            }
                        }

                        if (candidate[0] == 'V' && candidate[1] == 'B')
                        {
                            interned = "VB";
                            return true;
                        }
                    }
                    else if (length == 4)
                    {
                        if (TryInternHardcodedString(candidate, "TRUE", ref interned) ||
                            TryInternHardcodedString(candidate, "True", ref interned) ||
                            TryInternHardcodedString(candidate, "Copy", ref interned) ||
                            TryInternHardcodedString(candidate, "true", ref interned) ||
                            TryInternHardcodedString(candidate, "v4.0", ref interned))
                        {
                            return true;
                        }
                    }
                    else if (length == 5)
                    {
                        if (TryInternHardcodedString(candidate, "FALSE", ref interned) ||
                            TryInternHardcodedString(candidate, "false", ref interned) ||
                            TryInternHardcodedString(candidate, "Debug", ref interned) ||
                            TryInternHardcodedString(candidate, "Build", ref interned) ||
                            TryInternHardcodedString(candidate, "Win32", ref interned))
                        {
                            return true;
                        }
                    }
                    else if (length == 6)
                    {
                        if (TryInternHardcodedString(candidate, "''!=''", ref interned) ||
                            TryInternHardcodedString(candidate, "AnyCPU", ref interned))
                        {
                            return true;
                        }
                    }
                    else if (length == 7)
                    {
                        if (TryInternHardcodedString(candidate, "Library", ref interned) ||
                            TryInternHardcodedString(candidate, "MSBuild", ref interned) ||
                            TryInternHardcodedString(candidate, "Release", ref interned))
                        {
                            return true;
                        }
                    }
                    // see net.r_eg.IeXod.BackEnd.BuildRequestConfiguration.CreateUniqueGlobalProperty
                    else if (length > MSBuildConstants.MSBuildDummyGlobalPropertyHeader.Length &&
                            candidate.StartsWithStringByOrdinalComparison(MSBuildConstants.MSBuildDummyGlobalPropertyHeader))
                    {
                        // don't want to leak unique strings into the cache
                        interned = candidate.ExpensiveConvertToString();
                        return null;
                    }
                    else if (length == 24)
                    {
                        if (TryInternHardcodedString(candidate, "ResolveAssemblyReference", ref interned))
                        {
                            return true;
                        }
                    }
                    else if (length > _ginormousThreshold)
                    {
                        lock (_ginormous)
                        {
                            LinkedListNode<WeakReference> current = _ginormous.First;

                            while (current != null)
                            {
                                if (current.Value.Target is string last && last.Length == candidate.Length && candidate.StartsWithStringByOrdinalComparison(last))
                                {
                                    interned = last;
                                    _ginormousHits++;
                                    _ginormousCharsSaved += last.Length;

                                    _ginormous.Remove(current);
                                    _ginormous.AddFirst(current);

                                    return true;
                                }

                                current = current.Next;
                            }

                            _ginormousMisses++;
                            interned = candidate.ExpensiveConvertToString();

                            LinkedListNode<WeakReference> lastNode = _ginormous.Last;
                            _ginormous.RemoveLast();
                            _ginormous.AddFirst(lastNode);
                            lastNode.Value.Target = interned;

                            return false;
                        }
                    }
#if !CLR2COMPATIBILITY
                    else if (_useSimpleConcurrency)
                    {
                        var stringified = candidate.ExpensiveConvertToString();
                        interned = _internedStrings.GetOrAdd(stringified, stringified);
                        return true;
                    }
#endif
                    else if (length >= _hugeMruThreshold)
                    {
                        lock (_hugeMru)
                        {
                            return _hugeMru.TryGet(candidate, out interned);
                        }
                    }
                    else if (length >= _largeMruThreshold)
                    {
                        lock (_largeMru)
                        {
                            return _largeMru.TryGet(candidate, out interned);
                        }
                    }
                    else if (length >= _smallMruThreshold)
                    {
                        lock (_smallMru)
                        {
                            return _smallMru.TryGet(candidate, out interned);
                        }
                    }
                }

                interned = candidate.ExpensiveConvertToString();
                return null;
            }

            /// <summary>
            /// Version of Intern that gathers statistics
            /// </summary>
            private string InternWithStatistics<T>(T candidate) where T : IInternable
            {
                lock (_missedStrings)
                {
                    _stopwatch.Start();
                    bool? interned = TryIntern(candidate, out string result);
                    _stopwatch.Stop();

                    if (interned.HasValue && !interned.Value)
                    {
                        // Could not intern.
                        _internMisses++;

                        _missedStrings.TryGetValue(result, out int priorCount);
                        _missedStrings[result] = priorCount + 1;

                        return result;
                    }
                    else if (interned == null)
                    {
                        // Decided not to attempt interning
                        _internRejects++;

                        _rejectedStrings.TryGetValue(result, out int priorCount);
                        _rejectedStrings[result] = priorCount + 1;

                        return result;
                    }

                    _internHits++;
                    if (!candidate.ReferenceEquals(result))
                    {
                        // Reference changed so 'candidate' is now released and should save memory.
                        _internEliminatedStrings++;
                        _internEliminatedChars += candidate.Length;
                    }

                    return result;
                }
            }

            /// <summary>
            /// A singly linked list of strings where the most recently accessed string is at the top.
            /// Size expands up to a fixed number of strings.
            /// </summary>
            private class PrioritizedStringList
            {
                /// <summary>
                /// Maximum size of the mru list.
                /// </summary>
                private readonly int _size;

                /// <summary>
                /// Head of the mru list.
                /// </summary>
                private Node _mru;

                /// <summary>
                /// Construct an Mru list with a fixed maximum size.
                /// </summary>
                internal PrioritizedStringList(int size)
                {
                    _size = size;
                }

                /// <summary>
                /// Try to get one element from the list. Upon leaving the function 'candidate' will be at the head of the Mru list.
                /// This function is not thread-safe.
                /// </summary>
                internal bool TryGet<T>(T candidate, out string interned) where T : IInternable
                {
                    if (_size == 0)
                    {
                        interned = candidate.ExpensiveConvertToString();
                        return false;
                    }

                    int length = candidate.Length;
                    Node secondPrior = null;
                    Node prior = null;
                    Node head = _mru;
                    bool found = false;
                    int itemCount = 0;

                    while (head != null && !found)
                    {
                        if (head.Value.Length == length)
                        {
                            if (candidate.StartsWithStringByOrdinalComparison(head.Value))
                            {
                                found = true;
                            }
                        }

                        if (!found)
                        {
                            secondPrior = prior;
                            prior = head;
                            head = head.Next;
                        }

                        itemCount++;
                    }

                    if (found)
                    {
                        // Move it to the top and return the interned version.
                        if (prior != null)
                        {
                            if (!candidate.ReferenceEquals(head.Value))
                            {
                                // Wasn't at the top already, so move it there.
                                prior.Next = head.Next;
                                head.Next = _mru;
                                _mru = head;
                                interned = _mru.Value;
                                return true;
                            }
                            else
                            {
                                // But don't move it up if there is reference equality so that multiple calls to Intern don't redundantly emphasize a string.
                                interned = head.Value;
                                return true;
                            }
                        }
                        else
                        {
                            // Found the item in the top spot. No need to move anything.
                            interned = _mru.Value;
                            return true;
                        }
                    }
                    else
                    {
                        // Not found. Create a new entry and place it at the top.
                        Node old = _mru;
                        _mru = new Node(candidate.ExpensiveConvertToString()) { Next = old };

                        // Cache miss. Use this opportunity to discard any element over the max size.
                        if (itemCount >= _size && secondPrior != null)
                        {
                            secondPrior.Next = null;
                        }

                        interned = _mru.Value;
                        return false;
                    }
                }

                /// <summary>
                /// Returns the number of strings held and the total number of chars held.
                /// </summary>
                internal KeyValuePair<int, int> Statistics()
                {
                    Node head = _mru;
                    int chars = 0;
                    int strings = 0;
                    while (head != null)
                    {
                        chars += head.Value.Length;
                        strings++;
                        head = head.Next;
                    }

                    return new KeyValuePair<int, int>(strings, chars);
                }

                /// <summary>
                /// Singly linked list node.
                /// </summary>
                private class Node
                {
                    /// <summary>
                    /// Construct a Node
                    /// </summary>
                    internal Node(string value)
                    {
                        Value = value;
                    }

                    /// <summary>
                    /// The next node in the list.
                    /// </summary>
                    internal Node Next { get; set; }

                    /// <summary>
                    /// The held string.
                    /// </summary>
                    internal string Value { get; }
                }
            }
        }
    }
}
