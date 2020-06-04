// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) 2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Reflection;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Tasks;
using net.r_eg.IeXod.Utilities;
using System.Resources;
using net.r_eg.IeXod.Shared;
using Xunit;

namespace net.r_eg.IeXod.UnitTests
{
    sealed public class ResGenDependencies_Tests
    {
        [Theory]
        [MemberData(nameof(GenerateResource_Tests.Utilities.UsePreserializedResourceStates), MemberType = typeof(GenerateResource_Tests.Utilities))]

        public void DirtyCleanScenario(bool useMSBuildResXReader)
        {
            ResGenDependencies cache = new ResGenDependencies();

            string resx = CreateSampleResx();
            string stateFile = FileUtilities.GetTemporaryFile();

            try
            {
                // A newly created cache is not dirty.
                Assert.False(cache.IsDirty);

                // Getting a file that wasn't in the cache is a write operation.
                cache.GetResXFileInfo(resx, useMSBuildResXReader);
                Assert.True(cache.IsDirty);

                // Writing the file to disk should make the cache clean.
                cache.SerializeCache(stateFile, /* Log */ null);
                Assert.False(cache.IsDirty);

                // Deserialize from disk. Result should not be dirty.
                cache = ResGenDependencies.DeserializeCache(stateFile, true, /* Log */ null);
                Assert.False(cache.IsDirty);

                // Asking for a file that's in the cache should not dirty the cache.
                cache.GetResXFileInfo(resx, useMSBuildResXReader);
                Assert.False(cache.IsDirty);

                // Changing UseSourcePath to false should dirty the cache.
                cache.UseSourcePath = false;
                Assert.True(cache.IsDirty);
            }
            finally
            {
                File.Delete(resx);
                File.Delete(stateFile);
            }
        }

        /// <summary>
        /// Create a sample resx file on disk. Caller is responsible for deleting.
        /// </summary>
        /// <returns></returns>
        private string CreateSampleResx()
        {
            string resx = FileUtilities.GetTemporaryFile();
            File.Delete(resx);
            Stream fileToSend = Assembly.GetExecutingAssembly().GetManifestResourceStream("net.r_eg.IeXod.Tasks.UnitTests.SampleResx");
            using (FileStream f = new FileStream(resx, FileMode.CreateNew))
            {
                byte[] buffer = new byte[2048];
                int bytes;
                while ((bytes = fileToSend.Read(buffer, 0, 2048)) > 0)
                {
                    f.Write(buffer, 0, bytes);
                }
                fileToSend.Close();
            }
            return resx;
        }
    }
}
