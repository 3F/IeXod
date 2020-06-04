// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Xml;
using System.Text;

namespace net.r_eg.IeXod.BuildEngine
{
    /// <summary>
    /// All the state necessary for the evaluation of conditionals so that the expression tree 
    /// is stateless and reusable
    /// </summary>
    internal struct ConditionEvaluationState
    {
        internal XmlAttribute conditionAttribute;
        internal Expander expanderToUse;
        internal Hashtable conditionedPropertiesInProject;
        internal string parsedCondition;

        internal ConditionEvaluationState(XmlAttribute conditionAttribute, Expander expanderToUse, Hashtable conditionedPropertiesInProject, string parsedCondition)
        {
            this.conditionAttribute = conditionAttribute;
            this.expanderToUse = expanderToUse;
            this.conditionedPropertiesInProject = conditionedPropertiesInProject;
            this.parsedCondition = parsedCondition;
        }
    }
}
