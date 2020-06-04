﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//-----------------------------------------------------------------------
// </copyright>
// <summary>A packet which contains information needed for a node to configure itself for build.</summary>
//-----------------------------------------------------------------------

namespace net.r_eg.IeXod.BackEnd
{
    internal sealed class LoggingNodeConfiguration : ITranslatable
    {
        private bool _includeEvaluationMetaprojects;
        private bool _includeEvaluationProfiles;
        private bool _includeTaskInputs;

        public bool IncludeEvaluationMetaprojects => _includeEvaluationMetaprojects;

        public bool IncludeEvaluationProfiles => _includeEvaluationProfiles;

        public bool IncludeTaskInputs => _includeTaskInputs;

        public LoggingNodeConfiguration()
        {
        }

        public LoggingNodeConfiguration(bool includeEvaluationMetaprojects, bool includeEvaluationProfiles, bool includeTaskInputs)
        {
            _includeEvaluationMetaprojects = includeEvaluationMetaprojects;
            _includeEvaluationProfiles = includeEvaluationProfiles;
            _includeTaskInputs = includeTaskInputs;
        }

        void ITranslatable.Translate(ITranslator translator)
        {
            translator.Translate(ref _includeEvaluationMetaprojects);
            translator.Translate(ref _includeEvaluationProfiles);
            translator.Translate(ref _includeTaskInputs);
        }
    }
}
