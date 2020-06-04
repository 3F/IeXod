﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace net.r_eg.IeXod.Tasks
{
    /// <summary>
    /// An exception thrown while parsing through an app.config.
    /// </summary>
    [Serializable]
    internal class AppConfigException :
        System.ApplicationException
    {
        /// <summary>
        /// The name of the app.config file.
        /// </summary>
        private string fileName = String.Empty;
        internal string FileName
        {
            get
            {
                return fileName;
            }
        }


        /// <summary>
        /// The line number with the error. Is initialized to zero
        /// </summary>
        private int line;
        internal int Line
        {
            get
            {
                return line;
            }
        }

        /// <summary>
        /// The column with the error. Is initialized to zero
        /// </summary>
        private int column;
        internal int Column
        {
            get
            {
                return column;
            }
        }


        /// <summary>
        /// Construct the exception.
        /// </summary>
        public AppConfigException(string message, string fileName, int line, int column, System.Exception inner) : base(message, inner)
        {
            this.fileName = fileName;
            this.line = line;
            this.column = column;
        }

        /// <summary>
        /// Construct the exception.
        /// </summary>
        protected AppConfigException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
