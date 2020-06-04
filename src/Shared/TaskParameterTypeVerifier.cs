// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using net.r_eg.IeXod.Framework;
using System.Reflection;
using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod.BackEnd
{
    /// <summary>
    /// Provide a class which can verify the correct type for both input and output parameters.
    /// </summary>
    internal static class TaskParameterTypeVerifier
    {
        /// <summary>
        /// Is the parameter type a valid scalar input value
        /// </summary>
        internal static bool IsValidScalarInputParameter(Type parameterType)
        {
            bool result = (parameterType.GetTypeInfo().IsValueType || parameterType == typeof(string) || parameterType == typeof(ITaskItem));
            return result;
        }

        /// <summary>
        /// Is the passed in parameterType a valid vector input parameter
        /// </summary>
        internal static bool IsValidVectorInputParameter(Type parameterType)
        {
            bool result = (parameterType.IsArray && parameterType.GetElementType().GetTypeInfo().IsValueType) ||
                        parameterType == typeof(string[]) ||
                        parameterType == typeof(ITaskItem[]);
            return result;
        }

        /// <summary>
        /// Is the passed in value type assignable to an ITask or Itask[] object
        /// </summary>
        internal static bool IsAssignableToITask(Type parameterType)
        {
            bool result = typeof(ITaskItem[]).GetTypeInfo().IsAssignableFrom(parameterType.GetTypeInfo()) ||    /* ITaskItem array or derived type, or */
                          typeof(ITaskItem).IsAssignableFrom(parameterType);                                    /* ITaskItem or derived type */
            return result;
        }

        /// <summary>
        /// Is the passed parameter a valid value type output parameter
        /// </summary>
        internal static bool IsValueTypeOutputParameter(Type parameterType)
        {
            bool result = (parameterType.IsArray && parameterType.GetElementType().GetTypeInfo().IsValueType) ||    /* array of value types, or */
                          parameterType == typeof(string[]) ||                                                      /* string array, or */
                          parameterType.GetTypeInfo().IsValueType ||                                                /* value type, or */
                          parameterType == typeof(string);                                                          /* string */
            return result;
        }

        /// <summary>
        /// Is the parameter type a valid scalar or value type input parameter
        /// </summary>
        internal static bool IsValidInputParameter(Type parameterType)
        {
            return IsValidScalarInputParameter(parameterType) || IsValidVectorInputParameter(parameterType);
        }

        /// <summary>
        /// Is the parameter type a valid scalar or value type output parameter
        /// </summary>
        internal static bool IsValidOutputParameter(Type parameterType)
        {
            return IsValueTypeOutputParameter(parameterType) || IsAssignableToITask(parameterType);
        }
    }
}