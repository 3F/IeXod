﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace net.r_eg.IeXod.Tasks.Deployment.Bootstrapper
{
    /// <summary>
    /// Handles and stores XML validation events.
    /// </summary>
    internal class XmlValidationResults
    {
        private readonly List<string> _validationErrors = new List<string>();
        private readonly List<string> _validationWarnings = new List<string>();

        /// <summary>
        /// Constructor which includes the path to the file being validated.
        /// </summary>
        /// <param name="filePath">The file which is being validated.</param>
        public XmlValidationResults(string filePath)
        {
            FilePath = filePath;
        }

        /// <summary>
        /// Gets a string containing the name of the file being validated.
        /// </summary>
        /// <value>The name of the file being validated.</value>
        public string FilePath { get; }

        /// <summary>
        /// The delegate which will handle validation events.
        /// </summary>
        public void SchemaValidationEventHandler(object sender, System.Xml.Schema.ValidationEventArgs e)
        {
            if (e.Severity == System.Xml.Schema.XmlSeverityType.Error)
            {
                _validationErrors.Add(e.Message);
            }
            else
            {
                _validationWarnings.Add(e.Message);
            }
        }

        /// <summary>
        /// Gets all of the validation errors of the file being validated.
        /// </summary>
        /// <value>An array of type string, containing all of the validation errors.</value>
        public string[] ValidationErrors => _validationErrors.ToArray();

        /// <summary>
        /// Gets a value indicating if there were no validation errors or warnings.
        /// </summary>
        /// <value>true if there were no validation errors or warnings; otherwise false.  The default value is false.</value>
        public bool ValidationPassed => _validationErrors.Count == 0 && _validationWarnings.Count == 0;

        /// <summary>
        /// Gets all of the validation warnings of the file being validated.
        /// </summary>
        /// <value>An array of type string, containing all of the validation warnings.</value>
        public string[] ValidationWarnings => _validationWarnings.ToArray();
    }
}
