// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace net.r_eg.IeXod.Framework
{
    /// <summary>
    /// This interface defines the events raised by the build engine.
    /// Loggers use this interface to subscribe to the events they
    /// are interested in receiving.
    /// </summary>
    public interface IEventSource3 : IEventSource2
    {
        /// <summary>
        /// Should evaluation events include generated metaprojects?
        /// </summary>
        void IncludeEvaluationMetaprojects();

        /// <summary>
        /// Should evaluation events include profiling information?
        /// </summary>
        void IncludeEvaluationProfiles();

        /// <summary>
        /// Should task events include task inputs?
        /// </summary>
        void IncludeTaskInputs();
    }
}
