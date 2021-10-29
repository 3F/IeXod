// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace net.r_eg.IeXod.Globbing.Visitor
{
    internal abstract class GlobVisitor
    {
        public void Visit(IMSBuildGlob glob)
        {
            var msbuildGlob = glob as MSBuildGlob;
            if (msbuildGlob != null)
            {
                VisitMSBuildGlob(msbuildGlob);
            }

            var compositGlob = glob as CompositeGlob;
            if (compositGlob != null)
            {
                VisitCompositeGlob(compositGlob);

                foreach (var globPart in compositGlob.Globs)
                {
                    Visit(globPart);
                }
            }

            var globWithGaps = glob as MSBuildGlobWithGaps;
            if (globWithGaps != null)
            {
                VisitGlobWithGaps(globWithGaps);

                Visit(globWithGaps.MainGlob);
            }
        }

        protected virtual void VisitGlobWithGaps(MSBuildGlobWithGaps globWithGaps)
        {
        }

        protected virtual void VisitCompositeGlob(CompositeGlob compositGlob)
        {
        }

        protected virtual void VisitMSBuildGlob(MSBuildGlob msbuildGlob)
        {
        }
    }
}