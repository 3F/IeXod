// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using net.r_eg.IeXod.Framework;

namespace net.r_eg.IeXod.BuildEngine
{
    /// <summary>
    /// A cache entry representing a build result (task items + build success/failure)
    /// </summary>
    internal class BuildResultCacheEntry : BuildItemCacheEntry
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        internal BuildResultCacheEntry()
        {
        }

        /// <summary>
        /// Public constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="taskItems"></param>
        /// <param name="buildResult"></param>
        internal BuildResultCacheEntry(string name, BuildItem[] buildItems, bool buildResult)
            : base(name, buildItems)
        {
            this.buildResult = buildResult;
        }

        #endregion

        #region Properties

        private bool buildResult;

        /// <summary>
        /// Build result of this target (success, failure, skipped)
        /// </summary>
        internal bool BuildResult
        {
            get { return buildResult; }
            set { buildResult = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns true if the given cache entry contains equivalent contents
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        internal override bool IsEquivalent(CacheEntry other)
        {
            if ((other == null) || (other.GetType() != this.GetType()))
            {
                return false;
            }

            if (!base.IsEquivalent(other))
            {
                return false;
            }

            return (this.BuildResult == ((BuildResultCacheEntry)other).BuildResult);
        }
        #endregion

        #region CustomSerializationToStream
        internal override void WriteToStream(BinaryWriter writer)
        {
            base.WriteToStream(writer);
            writer.Write(buildResult);
        }

        internal override void CreateFromStream(BinaryReader reader)
        {
            base.CreateFromStream(reader);
            buildResult = reader.ReadBoolean();
        }
        #endregion
    }
}
