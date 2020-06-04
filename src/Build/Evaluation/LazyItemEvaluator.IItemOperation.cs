// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Immutable;

namespace net.r_eg.IeXod.Evaluation
{
    internal partial class LazyItemEvaluator<P, I, M, D>
    {
        internal interface IItemOperation
        {
            void Apply(ImmutableList<ItemData>.Builder listBuilder, ImmutableHashSet<string> globsToIgnore);
        }
    }
}