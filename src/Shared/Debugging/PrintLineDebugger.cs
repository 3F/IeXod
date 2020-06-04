﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Utilities;
using CommonWriterType = System.Action<string, string, System.Collections.Generic.IEnumerable<string>>;

namespace net.r_eg.IeXod.Shared.Debugging
{
    /// <summary>
    ///     A class to help with printline debugging in difficult environments like CI, or when debugging msbuild through other
    ///     tools like VS or CLI.
    ///     See example usages in PrintLineDebugger_Tests
    /// </summary>
    internal class PrintLineDebugger : IDisposable
    {
        internal enum NodeMode
        {
            CentralNode,
            OutOfProcNode,
            OutOfProcTaskHostNode
        }

        private static readonly Lazy<PropertyInfo> CommonWriterProperty = new Lazy<PropertyInfo>(
            () =>
            {
                var commonWriterType = typeof(ITask).Assembly.GetType("net.r_eg.IeXod.Shared.Debugging.CommonWriter", true, false);

                var propertyInfo = commonWriterType.GetProperty("Writer", BindingFlags.Public | BindingFlags.Static);

                ErrorUtilities.VerifyThrowInternalNull(propertyInfo, nameof(propertyInfo));

                return propertyInfo;
            });

        public static Lazy<PrintLineDebugger> Default =
            new Lazy<PrintLineDebugger>(() => Create(null, null, false));

        public static Lazy<PrintLineDebugger> DefaultWithProcessInfo =
            new Lazy<PrintLineDebugger>(() => Create(null, null, true));

        private static readonly Lazy<NodeMode> ProcessNodeMode = new Lazy<NodeMode>(
            () =>
            {
                return ScanNodeMode(Environment.CommandLine);

                NodeMode ScanNodeMode(string input)
                {
                    var match = Regex.Match(input, @"/nodemode:(?<nodemode>[12\s])(\s|$)", RegexOptions.IgnoreCase);

                    if (!match.Success)
                    {
                        return NodeMode.CentralNode;
                    }
                    var nodeMode = match.Groups["nodemode"].Value;

                    Trace.Assert(!string.IsNullOrEmpty(nodeMode));

                    switch (nodeMode)
                    {
                        case "1":
                            return NodeMode.OutOfProcNode;
                        case "2":
                            return NodeMode.OutOfProcTaskHostNode;
                        default:
                            throw new NotImplementedException();
                    }
                }
            });

        private readonly string _id;

        private readonly CommonWriterType _writerSetByThisInstance;

        public static string ProcessInfo
            =>
                $"{ProcessNodeMode.Value}_PID={Process.GetCurrentProcess() .Id}({Process.GetCurrentProcess() .ProcessName})x{(Environment.Is64BitProcess ? "64" : "86")}"
            ;

        public PrintLineDebugger(string id, CommonWriterType writer)
        {
            _id = id ?? string.Empty;

            if (writer != null)
            {
                SetWriter(writer);

                // we wrap the original writer with a locking writer in SetWriter, so get the actual writer that was set
                _writerSetByThisInstance = GetStaticWriter();
            }
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        public static CommonWriterType GetStaticWriter()
        {
            return (CommonWriterType) CommonWriterProperty.Value.GetValue(null, null);
        }

        // this setter is not thread safe because the assumption is that a writer is set once for the duration of the process (or multiple times from different tests which do not run in parallel).
        public static void SetWriter(CommonWriterType writer)
        {
#if DEBUG
            var currentWriter = GetStaticWriter();

            if (currentWriter != null)
            {
                ErrorUtilities.ThrowInternalError("Cannot set a new writer over an old writer. Remove the old one first");
            }

            // wrap with a lock so multi threaded logging does not break messages apart
            CommonWriterProperty.Value.SetValue(null, (CommonWriterType) LockWrappedWriter);

            void LockWrappedWriter(string id, string callsite, IEnumerable<string> message)
            {
                lock (writer)
                {
                    writer.Invoke(id, callsite, message);
                }
            }
#endif
        }

        public static void UnsetWriter()
        {
            var currentWriter = GetStaticWriter();

            if (currentWriter == null)
            {
                ErrorUtilities.ThrowInternalError("Cannot unset an already null writer");
            }

            CommonWriterProperty.Value.SetValue(null, null);
        }

        public static PrintLineDebugger Create(
            CommonWriterType writer = null,
            string id = null,
            bool prependProcessInfo = false)
        {
            return new PrintLineDebugger(
                prependProcessInfo
                    ? $"{ProcessInfo}_{id}"
                    : id,
                writer);
        }

        public CommonWriterType GetWriter()
        {
            return _writerSetByThisInstance ?? GetStaticWriter();
        }

        public void Log(
            string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
#if DEBUG
            var writer = GetWriter();

            writer?.Invoke(_id, CallsiteString(sourceFilePath, memberName, sourceLineNumber), new[] {message});
#endif
        }

        public void Log(
            IEnumerable<string> args,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
#if DEBUG
            var writer = GetWriter();

            writer?.Invoke(_id, CallsiteString(sourceFilePath, memberName, sourceLineNumber), args);
#endif
        }

        private static string CallsiteString(string sourceFilePath, string memberName, int sourceLineNumber)
        {
            return $"@{Path.GetFileNameWithoutExtension(sourceFilePath)}.{memberName}({sourceLineNumber})";
        }

        private void ReleaseUnmanagedResources()
        {
            if (_writerSetByThisInstance != null)
            {
                var staticWriter = GetStaticWriter();

                if (staticWriter != _writerSetByThisInstance)
                {
                    ErrorUtilities.ThrowInternalError($"The writer from this {nameof(PrintLineDebugger)} instance differs from the static writer.");
                }

                UnsetWriter();
            }
        }

        ~PrintLineDebugger()
        {
            ReleaseUnmanagedResources();
        }
    }
}
