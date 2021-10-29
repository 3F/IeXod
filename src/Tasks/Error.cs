// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace net.r_eg.IeXod.Tasks
{
    /// <summary>
    /// Task that simply emits an error. Engine will add project file path and line/column
    /// information.
    /// </summary>
    public sealed class Error : TaskExtension
    {
        /// <summary>
        /// Error message
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Error code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Relevant file if any.
        /// If none is provided, the file containing the Error 
        /// task will be used.
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// Error help keyword
        /// </summary>
        public string HelpKeyword { get; set; }

        /// <summary>
        /// Main task method
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            Log.LogError(null, Code, HelpKeyword, File, 0, 0, 0, 0, Text ?? TaskResources.GetString("ErrorAndWarning.EmptyMessage"));

            // careful to return false. Otherwise the build would continue.
            return false;
        }
    }
}
