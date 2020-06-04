// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod.Tasks
{
    /// <summary>
    /// Task that emits an error given a resource string. Engine will add project file path and line/column
    /// information.
    /// </summary>
    public sealed class ErrorFromResources : TaskExtension
    {
        /// <summary>
        /// Resource from which error message is extracted
        /// </summary>
        [Required]
        public string Resource { get; set; }

        /// <summary>
        /// Optional arguments to use when formatting the error message
        /// </summary>
        public string[] Arguments { get; set; }

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
        /// Log the requested error message.
        /// </summary>
        public override bool Execute()
        {
            try
            {
                string message = ResourceUtilities.ExtractMessageCode(false /* all codes */, Log.FormatResourceString(Resource, Arguments), out string errorCode);

                // If the user specifies a code, that should override. 
                Code = Code ?? errorCode;

                Log.LogError(null, Code, HelpKeyword, File, 0, 0, 0, 0, message);
            }
            catch (Exception e)
            {
                if (ExceptionHandling.IsCriticalException(e))
                {
                    throw;
                }

                Log.LogErrorWithCodeFromResources("ErrorFromResources.LogErrorFailure", Resource, e.Message);
            }

            // Effectively 'false', since by every codepath, some sort of error is getting logged.
            return !Log.HasLoggedErrors;
        }
    }
}
