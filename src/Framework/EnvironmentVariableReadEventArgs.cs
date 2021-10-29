// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace net.r_eg.IeXod.Framework
{
    /// <summary>
    /// Arguments for the environment variable read event.
    /// </summary>
    [Serializable]
    public class EnvironmentVariableReadEventArgs : BuildMessageEventArgs
    {
        /// <summary>
        /// Initializes an instance of the EnvironmentVariableReadEventArgs class.
        /// </summary>
        public EnvironmentVariableReadEventArgs()
        {
        }

        /// <summary>
        /// Initializes an instance of the EnvironmentVariableReadEventArgs class.
        /// </summary>
        /// <param name="environmentVariableName">The name of the environment variable that was read.</param>
        public EnvironmentVariableReadEventArgs(
            string environmentVariableName,
            string message,
            string helpKeyword = null,
            string senderName = null,
            MessageImportance importance = MessageImportance.Low) : base(message, helpKeyword, senderName, importance)
        {
            this.EnvironmentVariableName = environmentVariableName;
        }

        /// <summary>
        /// The name of the environment variable that was read.
        /// </summary>
        public string EnvironmentVariableName { get; set; }
    }
}
