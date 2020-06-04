// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Shared;
using net.r_eg.IeXod.Utilities;
using System;
using System.Collections.Generic;

namespace net.r_eg.IeXod.Tasks
{
    /// <summary>
    /// Task to assign a reasonable "Link" metadata to the provided items.
    /// </summary>
    public class AssignLinkMetadata : TaskExtension
    {
        /// <summary>
        /// The set of items to assign metadata to
        /// </summary>
        public ITaskItem[] Items { get; set; }

        /// <summary>
        /// The set of items to which the Link metadata has been set
        /// </summary>
        [Output]
        public ITaskItem[] OutputItems { get; set; }

        /// <summary>
        /// Sets "Link" metadata on any item where the project file in which they 
        /// were defined is different from the parent project file to a sane default: 
        /// the relative directory compared to the defining file.  
        /// 
        /// Does NOT overwrite Link metadata if it's already defined. 
        /// </summary>
        public override bool Execute()
        {
            var outputItems = new List<ITaskItem>();

            if (Items != null)
            {
                foreach (ITaskItem item in Items)
                {
                    try
                    {
                        string definingProject = item.GetMetadata(FileUtilities.ItemSpecModifiers.DefiningProjectFullPath);
                        string definingProjectDirectory = item.GetMetadata(FileUtilities.ItemSpecModifiers.DefiningProjectDirectory);
                        string fullPath = item.GetMetadata(FileUtilities.ItemSpecModifiers.FullPath);

                        if (
                                String.IsNullOrEmpty(item.GetMetadata("Link"))
                                && !String.IsNullOrEmpty(definingProject)
                                && fullPath.StartsWith(definingProjectDirectory, StringComparison.OrdinalIgnoreCase)
                            )
                        {
                            string link = fullPath.Substring(definingProjectDirectory.Length);
                            ITaskItem outputItem = new TaskItem(item);
                            outputItem.SetMetadata("Link", link);

                            outputItems.Add(outputItem);
                        }
                    }
                    catch (InvalidOperationException e)
                    {
                        // can happen if the item is not a proper path
                        Log.LogWarningFromException(e);
                    }
                }
            }

            OutputItems = outputItems.ToArray();
            return !Log.HasLoggedErrors;
        }
    }
}
