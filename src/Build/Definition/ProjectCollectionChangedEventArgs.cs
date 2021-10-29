﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace net.r_eg.IeXod.Evaluation
{
    /// <summary>
    /// Properties or other mutable state associated with a <see cref="ProjectCollection"/>.
    /// </summary>
    public enum ProjectCollectionChangedState
    {
        /// <summary>
        /// The <see cref="ProjectCollection.DefaultToolsVersion"/> property changed.
        /// </summary>
        DefaultToolsVersion,

        /// <summary>
        /// The toolsets changed.
        /// </summary>
        Toolsets,

        /// <summary>
        /// The loggers changed.
        /// </summary>
        Loggers,

        /// <summary>
        /// The global properties changed.
        /// </summary>
        GlobalProperties,

        /// <summary>
        /// The <see cref="ProjectCollection.IsBuildEnabled"/> property changed.
        /// </summary>
        IsBuildEnabled,

        /// <summary>
        /// The <see cref="ProjectCollection.OnlyLogCriticalEvents"/> property changed.
        /// </summary>
        OnlyLogCriticalEvents,

        /// <summary>
        /// The <see cref="ProjectCollection.HostServices"/> property changed.
        /// </summary>
        HostServices,

        /// <summary>
        /// The <see cref="ProjectCollection.DisableMarkDirty"/> property changed.
        /// </summary>
        DisableMarkDirty,

        /// <summary>
        /// The <see cref="ProjectCollection.SkipEvaluation"/> property changed.
        /// </summary>
        SkipEvaluation,
    }

    /// <summary>
    /// Event arguments for the <see cref="ProjectCollection.ProjectCollectionChanged"/> event.
    /// </summary>
    public class ProjectCollectionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectCollectionChangedEventArgs"/> class.
        /// </summary>
        internal ProjectCollectionChangedEventArgs(ProjectCollectionChangedState changedState)
        {
            Changed = changedState;
        }

        /// <summary>
        /// Gets the nature of the change made to the <see cref="ProjectCollection"/>.
        /// </summary>
        public ProjectCollectionChangedState Changed { get; private set; }
    }
}
