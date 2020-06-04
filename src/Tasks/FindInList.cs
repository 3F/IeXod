﻿// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Shared;

namespace net.r_eg.IeXod.Tasks
{
    /// <summary>
    /// A task that finds an item with the specified itemspec, if present,
    /// in the provided list.
    /// </summary>
    public class FindInList : TaskExtension
    {
        private ITaskItem[] _list;

        /// <summary>
        /// The list to search through
        /// </summary>
        [Required]
        public ITaskItem[] List
        {
            get
            {
                ErrorUtilities.VerifyThrowArgumentNull(_list, nameof(List));
                return _list;
            }
            set => _list = value;
        }

        /// <summary>
        /// Whether to match against just the file part of the itemspec,
        /// or the whole itemspec (the default)
        /// </summary>
        public bool MatchFileNameOnly { get; set; }

        /// <summary>
        /// The first matching item found in the list, if any
        /// </summary>
        [Output]
        public ITaskItem ItemFound { get; set; }

        /// <summary>
        /// The itemspec to try to find
        /// </summary>
        [Required]
        public string ItemSpecToFind { get; set; }

        /// <summary>
        /// Whether or not to match case sensitively
        /// </summary>
        public bool CaseSensitive { get; set; }

        /// <summary>
        /// Whether or not to return the last match, instead of 
        /// the first one
        /// </summary>
        public bool FindLastMatch { get; set; }

        /// <summary>
        /// Entry point
        /// </summary>
        public override bool Execute()
        {
            StringComparison comparison = CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            if (!FindLastMatch)
            {
                // Walk forwards
                foreach (ITaskItem item in List)
                {
                    if (IsMatchingItem(comparison, item))
                    {
                        return true;
                    }
                }
            }
            else
            {
                // Walk backwards
                for (int i = List.Length - 1; i >= 0; i--)
                {
                    if (IsMatchingItem(comparison, List[i]))
                    {
                        return true;
                    }
                }
            }

            // Not found
            return true;
        }

        /// <summary>
        /// Examines the item to see if it matches what we are looking for.
        /// If it does, returns true.
        /// </summary>
        private bool IsMatchingItem(StringComparison comparison, ITaskItem item)
        {
            try
            {
                var path = FileUtilities.FixFilePath(item.ItemSpec);
                string filename = (MatchFileNameOnly ? Path.GetFileName(path) : path);

                if (String.Equals(filename, ItemSpecToFind, comparison))
                {
                    ItemFound = item;
                    Log.LogMessageFromResources(MessageImportance.Low, "FindInList.Found", path);
                    return true;
                }
            }
            catch (ArgumentException ex)
            {
                // Just log this: presumably this item spec is not intended to be
                // a file path
                Log.LogMessageFromResources(MessageImportance.Low, "FindInList.InvalidPath", item.ItemSpec, ex.Message);
            }
            return false;
        }
    }
}
