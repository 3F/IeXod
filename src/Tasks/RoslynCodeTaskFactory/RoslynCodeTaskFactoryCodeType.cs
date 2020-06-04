// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace net.r_eg.IeXod.Tasks
{
    /// <summary>
    /// Represents the kind of code contained in the code task definition.
    /// </summary>
    internal enum RoslynCodeTaskFactoryCodeType
    {
        /// <summary>
        /// The code is a fragment and should be included within a method.
        /// </summary>
        Fragment,

        /// <summary>
        /// The code is a method and should be included within a class.
        /// </summary>
        Method,

        /// <summary>
        /// The code is a whole class and no modifications should be made to it.
        /// </summary>
        Class,
    }
}
