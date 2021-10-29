// Copyright (c) Microsoft. All rights reserved.
// Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
// Copyright (c) IeXod contributors https://github.com/3F/IeXod/graphs/contributors
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

using net.r_eg.IeXod.Construction;
using net.r_eg.IeXod.Evaluation;
using net.r_eg.IeXod.Execution;
using net.r_eg.IeXod.Framework;
using net.r_eg.IeXod.Graph;
using net.r_eg.IeXod.Logging;
using net.r_eg.IeXod.Shared;
using net.r_eg.IeXod.Shared.FileSystem;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace net.r_eg.IeXod.UnitTests
{
    /*
     * Class:   ObjectModelHelpers
     *
     * Utility methods for unit tests that work through the object model.
     *
     */
    internal static class ObjectModelHelpers
    {
        private const string msbuildNamespace = "http://schemas.microsoft.com/developer/msbuild/2003";
        private static string s_msbuildDefaultToolsVersion = MSBuildConstants.CurrentToolsVersion;
        private static string s_msbuildAssemblyVersion = MSBuildConstants.CurrentAssemblyVersion;
        private static string s_currentVisualStudioVersion = MSBuildConstants.CurrentVisualStudioVersion;

        /// <summary>
        /// Return the current Visual Studio version
        /// </summary>
        internal static string CurrentVisualStudioVersion
        {
            get
            {
                return s_currentVisualStudioVersion;
            }
        }

        /// <summary>
        /// Return the default tools version
        /// </summary>
        internal static string MSBuildDefaultToolsVersion
        {
            get
            {
                return s_msbuildDefaultToolsVersion;
            }
        }

        /// <summary>
        /// Return the current assembly version
        /// </summary>
        internal static string MSBuildAssemblyVersion
        {
            get
            {
                return s_msbuildAssemblyVersion;
            }
        }

        /// <summary>
        /// Gets an item list from the project and assert that it contains
        /// exactly one item with the supplied name.
        /// </summary>
        internal static ProjectItem AssertSingleItem(Project p, string type, string itemInclude)
        {
            ProjectItem[] items = p.GetItems(type).ToArray();
            int count = 0;
            foreach (ProjectItem item in items)
            {
                Assert.Equal(itemInclude.ToUpperInvariant(), item.EvaluatedInclude.ToUpperInvariant());
                ++count;
            }

            Assert.Equal(1, count);

            return items[0];
        }

        internal static void AssertItemEvaluationFromProject(string projectContents, string[] inputFiles, string[] expectedInclude, Dictionary<string, string>[] expectedMetadataPerItem = null, bool normalizeSlashes = false, bool makeExpectedIncludeAbsolute = false)
        {
            AssertItemEvaluationFromGenericItemEvaluator((p, c) =>
                {
                    return new Project(p, new Dictionary<string, string>(), ProjectToolsOptions.Default, c)
                        .Items
                        .Select(i => (TestItem) new ProjectItemTestItemAdapter(i))
                        .ToList();
                },
            projectContents,
            inputFiles,
            expectedInclude,
            makeExpectedIncludeAbsolute,
            expectedMetadataPerItem,
            normalizeSlashes);
        }

        internal static void AssertItemEvaluationFromGenericItemEvaluator(Func<string, ProjectCollection, IList<TestItem>> itemEvaluator, string projectContents, string[] inputFiles, string[] expectedInclude, bool makeExpectedIncludeAbsolute = false, Dictionary<string, string>[] expectedMetadataPerItem = null, bool normalizeSlashes = false)
        {
            using (var env = TestEnvironment.Create())
            using (var collection = new ProjectCollection())
            {
                var testProject = env.CreateTestProjectWithFiles(projectContents, inputFiles);
                var evaluatedItems = itemEvaluator(testProject.ProjectFile, collection);

                if (makeExpectedIncludeAbsolute)
                {
                    expectedInclude = expectedInclude.Select(i => Path.Combine(testProject.TestRoot, i)).ToArray();
                }

                if (expectedMetadataPerItem == null)
                {
                    AssertItems(expectedInclude, evaluatedItems, expectedDirectMetadata: null, normalizeSlashes: normalizeSlashes);
                }
                else
                {
                    AssertItems(expectedInclude, evaluatedItems, expectedMetadataPerItem, normalizeSlashes);
                }
            }
        }

        internal static string NormalizeSlashes(string path)
        {
            return path.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
        }

        // todo Make IItem<M> public and add these new members to it.
        internal interface TestItem
        {
            string EvaluatedInclude { get; }
            int DirectMetadataCount { get; }
            string GetMetadataValue(string key);
        }

        internal class ProjectItemTestItemAdapter : TestItem
        {
            private readonly ProjectItem _projectInstance;

            public ProjectItemTestItemAdapter(ProjectItem projectInstance)
            {
                _projectInstance = projectInstance;
            }

            public string EvaluatedInclude => _projectInstance.EvaluatedInclude;
            public int DirectMetadataCount => _projectInstance.DirectMetadataCount;
            public string GetMetadataValue(string key) => _projectInstance.GetMetadataValue(key);

            public static implicit operator ProjectItemTestItemAdapter(ProjectItem pi)
            {
                return new ProjectItemTestItemAdapter(pi);
            }
        }

        internal class ProjectItemInstanceTestItemAdapter : TestItem
        {
            private readonly ProjectItemInstance _projectInstance;

            public ProjectItemInstanceTestItemAdapter(ProjectItemInstance projectInstance)
            {
                _projectInstance = projectInstance;
            }

            public string EvaluatedInclude => _projectInstance.EvaluatedInclude;
            public int DirectMetadataCount => _projectInstance.DirectMetadataCount;
            public string GetMetadataValue(string key) => _projectInstance.GetMetadataValue(key);

            public static implicit operator ProjectItemInstanceTestItemAdapter(ProjectItemInstance pi)
            {
                return new ProjectItemInstanceTestItemAdapter(pi);
            }
        }

        internal static void AssertItems(string[] expectedItems, ICollection<ProjectItem> items, Dictionary<string, string> expectedDirectMetadata = null, bool normalizeSlashes = false)
        {
            var converteditems = items.Select(i => (TestItem) new ProjectItemTestItemAdapter(i)).ToList();
            AssertItems(expectedItems, converteditems, expectedDirectMetadata, normalizeSlashes);
        }

        /// <summary>
        /// Asserts that the list of items has the specified evaluated includes.
        /// </summary>
        internal static void AssertItems(string[] expectedItems, IList<TestItem> items, Dictionary<string, string> expectedDirectMetadata = null, bool normalizeSlashes = false)
        {
            if (expectedDirectMetadata == null)
            {
                expectedDirectMetadata = new Dictionary<string, string>();
            }

            // all items have the same metadata
            var metadata = new Dictionary<string, string>[expectedItems.Length];

            for (var i = 0; i < metadata.Length; i++)
            {
                metadata[i] = expectedDirectMetadata;
            }

            AssertItems(expectedItems, items, metadata, normalizeSlashes);
        }

        public static void AssertItems(string[] expectedItems, IList<ProjectItem> items, Dictionary<string, string>[] expectedDirectMetadataPerItem, bool normalizeSlashes = false)
        {
            var convertedItems = items.Select(i => (TestItem) new ProjectItemTestItemAdapter(i)).ToList();
            AssertItems(expectedItems, convertedItems, expectedDirectMetadataPerItem, normalizeSlashes);
        }

        public static void AssertItems(string[] expectedItems, IList<TestItem> items, Dictionary<string, string>[] expectedDirectMetadataPerItem, bool normalizeSlashes = false)
        {
            if (items.Count != 0 || expectedDirectMetadataPerItem.Length != 0)
            {
                expectedItems.ShouldNotBeEmpty();
            }

            for (var i = 0; i < expectedItems.Length; i++)
            {
                if (!normalizeSlashes)
                {
                    items[i].EvaluatedInclude.ShouldBe(expectedItems[i]);
                }
                else
                {
                    var normalizedItem = NormalizeSlashes(expectedItems[i]);
                    items[i].EvaluatedInclude.ShouldBe(normalizedItem);
                }

                AssertItemHasMetadata(expectedDirectMetadataPerItem[i], items[i]);
            }

            items.Count.ShouldBe(expectedItems.Length);

            expectedItems.Length.ShouldBe(expectedDirectMetadataPerItem.Length);
        }

        internal static void AssertItemHasMetadata(Dictionary<string, string> expected, ProjectItem item)
        {
            AssertItemHasMetadata(expected, new ProjectItemTestItemAdapter(item));
        }

        internal static void AssertItemHasMetadata(Dictionary<string, string> expected, TestItem item)
        {
            expected ??= new Dictionary<string, string>();

            item.DirectMetadataCount.ShouldBe(expected.Keys.Count);

            foreach (var key in expected.Keys)
            {
                item.GetMetadataValue(key).ShouldBe(expected[key]);

            }
        }

        /// <summary>
        /// Used to compare the contents of two arrays.
        /// </summary>
        internal static void AssertArrayContentsMatch(object[] expected, object[] actual)
        {
            if (expected == null)
            {
                Assert.Null(actual); // "Expected a null array"
                return;
            }

            Assert.NotNull(actual); // "Result should be non-null."
            Assert.Equal(expected.Length, actual.Length); // "Expected array length of <" + expected.Length + "> but was <" + actual.Length + ">.");

            // Now that we've verified they're both non-null and of the same length, compare each item in the array.
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], actual[i]); // "At index " + i + " expected " + expected[i].ToString() + " but was " + actual.ToString());
            }
        }

        /// <summary>
        /// Assert that a given file exists within the temp project directory.
        /// </summary>
        /// <param name="fileRelativePath"></param>
        internal static void AssertFileExistsInTempProjectDirectory(string fileRelativePath)
        {
            AssertFileExistsInTempProjectDirectory(fileRelativePath, null);
        }

        /// <summary>
        /// Assert that a given file exists within the temp project directory.
        /// </summary>
        /// <param name="fileRelativePath"></param>
        /// <param name="message">Can be null.</param>
        internal static void AssertFileExistsInTempProjectDirectory(string fileRelativePath, string message)
        {
            if (message == null)
            {
                message = fileRelativePath + " doesn't exist, but it should.";
            }

            Assert.True(FileSystems.Default.FileExists(Path.Combine(TempProjectDir, fileRelativePath)), message);
        }

        /// <summary>
        /// Does certain replacements in a string representing the project file contents.
        /// This makes it easier to write unit tests because the author doesn't have
        /// to worry about escaping double-quotes, etc.
        /// </summary>
        /// <param name="projectFileContents"></param>
        /// <returns></returns>
        internal static string CleanupFileContents(string projectFileContents)
        {
            // Replace reverse-single-quotes with double-quotes.
            projectFileContents = projectFileContents.Replace("`", "\"");

            // Place the correct MSBuild namespace into the <Project> tag.
            projectFileContents = projectFileContents.Replace("msbuildnamespace", msbuildNamespace);
            projectFileContents = projectFileContents.Replace("msbuilddefaulttoolsversion", s_msbuildDefaultToolsVersion);
            projectFileContents = projectFileContents.Replace("msbuildassemblyversion", s_msbuildAssemblyVersion);

            return projectFileContents;
        }

        public static string Cleanup(this string aString)
        {
            return CleanupFileContents(aString);
        }

        /// <summary>
        /// Normalizes all the whitespace in an xml string so that two documents that
        /// differ only in whitespace can be easily compared to each other for sameness.
        /// </summary>
        internal static string NormalizeXmlWhitespace(string xml)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xml);

            // Normalize all the whitespace by writing the Xml document out to a
            // string, with PreserveWhitespace=false.
            xmldoc.PreserveWhitespace = false;

            StringBuilder sb = new StringBuilder(xml.Length);
            var writerSettings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Encoding = Encoding.UTF8,
                Indent = true
            };

            using (var writer = XmlWriter.Create(sb, writerSettings))
            {
                xmldoc.WriteTo(writer);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Create an MSBuild project file on disk and return the full path to it.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        internal static string CreateTempFileOnDisk(string fileContents, params object[] args)
        {
            return CreateTempFileOnDiskNoFormat(string.Format(fileContents, args));
        }

        /// <summary>
        /// Create an MSBuild project file on disk and return the full path to it.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        internal static string CreateTempFileOnDiskNoFormat(string fileContents)
        {
            string projectFilePath = FileUtilities.GetTemporaryFile();

            File.WriteAllText(projectFilePath, CleanupFileContents(fileContents));

            return projectFilePath;
        }

        internal static ProjectRootElement CreateInMemoryProjectRootElement(string projectContents, ProjectCollection collection = null, bool preserveFormatting = true)
        {
            var cleanedProject = CleanupFileContents(projectContents);

            return ProjectRootElement.Create(
                XmlReader.Create(new StringReader(cleanedProject)),
                collection ?? new ProjectCollection(),
                preserveFormatting);
        }

        /// <summary>
        /// Create a project in memory. Load up the given XML.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        internal static Project CreateInMemoryProject(string xml)
        {
            return CreateInMemoryProject(xml, new ConsoleLogger());
        }

        /// <summary>
        /// Create a project in memory. Load up the given XML.
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        internal static Project CreateInMemoryProject(string xml, ILogger logger /* May be null */)
        {
            return CreateInMemoryProject(new ProjectCollection(), xml, logger);
        }

        /// <summary>
        /// Create an in-memory project and attach it to the passed-in engine.
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="xml"></param>
        /// <param name="logger">May be null</param>
        /// <returns></returns>
        internal static Project CreateInMemoryProject(ProjectCollection e, string xml, ILogger logger /* May be null */)
        {
            return CreateInMemoryProject(e, xml, logger, null);
        }

        /// <summary>
        /// Create an in-memory project and attach it to the passed-in engine.
        /// </summary>
        /// <param name="logger">May be null</param>
        /// <param name="toolsVersion">May be null</param>
        internal static Project CreateInMemoryProject
            (
            ProjectCollection projectCollection,
            string xml,
            ILogger logger /* May be null */,
            string toolsVersion /* may be null */
            )
        {
            XmlReaderSettings readerSettings = new XmlReaderSettings {DtdProcessing = DtdProcessing.Ignore};

            Project project = new Project
                (
                XmlReader.Create(new StringReader(CleanupFileContents(xml)), readerSettings),
                null,
                new ProjectToolsOptions(toolsVersion),
                projectCollection
                );

            Guid guid = Guid.NewGuid();
            project.FullPath = Path.Combine(TempProjectDir, "Temporary" + guid.ToString("N") + ".csproj");
            project.ReevaluateIfNecessary();

            if (logger != null)
            {
                project.ProjectCollection.RegisterLogger(logger);
            }

            return project;
        }

        /// <summary>
        /// Creates a project in memory and builds the default targets.  The build is
        /// expected to succeed.
        /// </summary>
        /// <param name="projectContents"></param>
        /// <returns></returns>
        internal static MockLogger BuildProjectExpectSuccess
            (
            string projectContents,
            ITestOutputHelper testOutputHelper = null
            )
        {
            MockLogger logger = new MockLogger(testOutputHelper);
            BuildProjectExpectSuccess(projectContents, logger);
            return logger;
        }

        internal static void BuildProjectExpectSuccess
            (
            string projectContents,
            params ILogger[] loggers
            )
        {
            Project project = CreateInMemoryProject(projectContents, logger: null); // logger is null so we take care of loggers ourselves
            project.Build(loggers).ShouldBeTrue();
        }

        /// <summary>
        /// Creates a project in memory and builds the default targets.  The build is
        /// expected to fail.
        /// </summary>
        /// <param name="projectContents"></param>
        /// <returns></returns>
        internal static MockLogger BuildProjectExpectFailure
            (
            string projectContents
            )
        {
            MockLogger logger = new MockLogger();
            BuildProjectExpectFailure(projectContents, logger);

            return logger;
        }

        internal static void BuildProjectExpectFailure
            (
            string projectContents,
            ILogger logger
           )
        {
            Project project = CreateInMemoryProject(projectContents, logger);

            bool success = project.Build(logger);
            Assert.False(success); // "Build succeeded, but shouldn't have.  See test output (Attachments in Azure Pipelines) for details"
        }

        /// <summary>
        /// This helper method compares the final project contents with the expected
        /// value.
        /// </summary>
        /// <param name="project"></param>
        /// <param name="newExpectedProjectContents"></param>
        internal static void CompareProjectContents
            (
            Project project,
            string newExpectedProjectContents
            )
        {
            // Get the new XML for the project, normalizing the whitespace.
            string newActualProjectContents = project.Xml.RawXml;

            // Replace single-quotes with double-quotes, and normalize whitespace.
            newExpectedProjectContents = NormalizeXmlWhitespace(CleanupFileContents(newExpectedProjectContents));

            // Compare the actual XML with the expected XML.
            Console.WriteLine("================================= EXPECTED ===========================================");
            Console.WriteLine(newExpectedProjectContents);
            Console.WriteLine();
            Console.WriteLine("================================== ACTUAL ============================================");
            Console.WriteLine(newActualProjectContents);
            Console.WriteLine();
            Assert.Equal(newExpectedProjectContents, newActualProjectContents); // "Project XML does not match expected XML.  See 'Standard Out' tab for details."
        }


        private static string s_tempProjectDir;

        /// <summary>
        /// Creates and returns a unique path under temp
        /// </summary>
        internal static string TempProjectDir
        {
            get
            {
                if (s_tempProjectDir == null)
                {
                    s_tempProjectDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));

                    Directory.CreateDirectory(s_tempProjectDir);
                }

                return s_tempProjectDir;
            }
        }

        /// <summary>
        /// Deletes the directory %TEMP%\TempDirForMSBuildUnitTests, and all its contents.
        /// </summary>
        internal static void DeleteTempProjectDirectory()
        {
            DeleteDirectory(TempProjectDir);
        }

        /// <summary>
        /// Deletes the directory and all its contents.
        /// </summary>
        internal static void DeleteDirectory(string dir)
        {
            // Manually deleting all children, but intentionally leaving the
            // Temp project directory behind due to locking issues which were causing
            // failures in main on Amd64-WOW runs.

            // retries to deal with occasional locking issues where the file / directory can't be deleted to initially
            for (int retries = 0; retries < 5; retries++)
            {
                try
                {
                    if (FileSystems.Default.DirectoryExists(dir))
                    {
                        foreach (string directory in Directory.GetDirectories(dir))
                        {
                            Directory.Delete(directory, true);
                        }

                        foreach (string file in Directory.GetFiles(dir))
                        {
                            File.Delete(file);
                        }
                    }

                    break;
                }
                catch (Exception ex)
                {
                    if (retries < 4)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    else
                    {
                        // All the retries have failed. We will now fail with the
                        // actual problem now instead of with some more difficult-to-understand
                        // issue later.
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Creates a file in the %TEMP%\TempDirForMSBuildUnitTests directory, after cleaning
        /// up the file contents (replacing single-back-quote with double-quote, etc.).
        /// Silently OVERWRITES existing file.
        /// </summary>
        internal static string CreateFileInTempProjectDirectory(string fileRelativePath, string fileContents, Encoding encoding = null)
        {
            Assert.False(string.IsNullOrEmpty(fileRelativePath));
            string fullFilePath = Path.Combine(TempProjectDir, fileRelativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(fullFilePath));

            // retries to deal with occasional locking issues where the file can't be written to initially
            for (int retries = 0; retries < 5; retries++)
            {
                try
                {
                    if (encoding == null)
                    {
                        // This method uses UTF-8 encoding without a Byte-Order Mark (BOM)
                        // https://msdn.microsoft.com/en-us/library/ms143375(v=vs.110).aspx#Remarks
                        File.WriteAllText(fullFilePath, CleanupFileContents(fileContents));
                    }
                    else
                    {
                        // If it is necessary to include a UTF-8 identifier, such as a byte order mark, at the beginning of a file,
                        // use the WriteAllText(String,?String,?Encoding) method overload with UTF8 encoding.
                        File.WriteAllText(fullFilePath, CleanupFileContents(fileContents), encoding);
                    }
                    break;
                }
                catch (Exception ex)
                {
                    if (retries < 4)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    else
                    {
                        // All the retries have failed. We will now fail with the
                        // actual problem now instead of with some more difficult-to-understand
                        // issue later.
                        throw;
                    }
                }
            }

            return fullFilePath;
        }

        /// <summary>
        /// Builds a project file from disk, and asserts if the build does not succeed.
        /// </summary>
        /// <param name="projectFileRelativePath"></param>
        /// <returns></returns>
        internal static void BuildTempProjectFileExpectSuccess(string projectFileRelativePath, MockLogger logger)
        {
            BuildTempProjectFileWithTargetsExpectSuccess(projectFileRelativePath, null, null, logger);
        }

        /// <summary>
        /// Builds a project file from disk, and asserts if the build does not succeed.
        /// </summary>
        internal static void BuildTempProjectFileWithTargetsExpectSuccess(string projectFileRelativePath, string[] targets, IDictionary<string, string> additionalProperties, MockLogger logger)
        {
            BuildTempProjectFileWithTargets(projectFileRelativePath, targets, additionalProperties, logger)
                .ShouldBeTrue("Build failed.  See test output (Attachments in Azure Pipelines) for details");
        }

        /// <summary>
        /// Builds a project file from disk, and asserts if the build succeeds.
        /// </summary>
        internal static void BuildTempProjectFileExpectFailure(string projectFileRelativePath, MockLogger logger)
        {
            BuildTempProjectFileWithTargets(projectFileRelativePath, null, null, logger)
                .ShouldBeFalse("Build unexpectedly succeeded.  See test output (Attachments in Azure Pipelines) for details");
        }

        /// <summary>
        /// Loads a project file from disk
        /// </summary>
        /// <param name="fileRelativePath"></param>
        /// <returns></returns>
        internal static Project LoadProjectFileInTempProjectDirectory(string projectFileRelativePath)
        {
            return LoadProjectFileInTempProjectDirectory(projectFileRelativePath, false /* don't touch project*/);
        }

        /// <summary>
        /// Loads a project file from disk
        /// </summary>
        /// <param name="fileRelativePath"></param>
        /// <returns></returns>
        internal static Project LoadProjectFileInTempProjectDirectory(string projectFileRelativePath, bool touchProject)
        {
            string projectFileFullPath = Path.Combine(TempProjectDir, projectFileRelativePath);

            ProjectCollection projectCollection = new ProjectCollection();

            Project project = new Project(projectFileFullPath, null, null, projectCollection);

            if (touchProject)
            {
                File.SetLastWriteTime(projectFileFullPath, DateTime.Now);
            }

            return project;
        }

        /// <summary>
        /// Builds a project file from disk, and asserts if the build does not succeed.
        /// </summary>
        /// <param name="projectFileRelativePath"></param>
        /// <param name="targets"></param>
        /// <param name="additionalProperties">Can be null.</param>
        /// <param name="logger"></param>
        /// <returns></returns>
        internal static bool BuildTempProjectFileWithTargets
        (
            string projectFileRelativePath,
            string[] targets,
            IDictionary<string, string> globalProperties,
            ILogger logger
        )
        {
            // Build the default targets.
            List<ILogger> loggers = new List<ILogger>(1);
            loggers.Add(logger);

            if (string.Equals(Path.GetExtension(projectFileRelativePath), ".sln"))
            {
                string projectFileFullPath = Path.Combine(TempProjectDir, projectFileRelativePath);
                BuildRequestData data = new BuildRequestData(projectFileFullPath, globalProperties ?? new Dictionary<string, string>(), null, targets, null);
                BuildParameters parameters = new BuildParameters();
                parameters.Loggers = loggers;
                BuildResult result = BuildManager.DefaultBuildManager.Build(parameters, data);
                return result.OverallResult == BuildResultCode.Success;
            }
            else
            {
                Project project = LoadProjectFileInTempProjectDirectory(projectFileRelativePath);

                if (globalProperties != null)
                {
                    // add extra properties
                    foreach (KeyValuePair<string, string> globalProperty in globalProperties)
                    {
                        project.SetGlobalProperty(globalProperty.Key, globalProperty.Value);
                    }
                }

                return project.Build(targets, loggers);
            }
        }

        /// <summary>
        /// Delete any files in the list that currently exist.
        /// </summary>
        /// <param name="files"></param>
        internal static void DeleteTempFiles(string[] files)
        {
            for (int i = 0; i < files.Length; i++)
            {
                if (FileSystems.Default.FileExists(files[i])) File.Delete(files[i]);
            }
        }

        /// <summary>
        /// Returns the requested number of temporary files.
        /// </summary>
        internal static string[] GetTempFiles(int number)
        {
            return GetTempFiles(number, DateTime.Now);
        }

        /// <summary>
        /// Returns the requested number of temporary files, with the requested write time.
        /// </summary>
        internal static string[] GetTempFiles(int number, DateTime lastWriteTime)
        {
            string[] files = new string[number];

            for (int i = 0; i < number; i++)
            {
                files[i] = FileUtilities.GetTemporaryFile();
                File.SetLastWriteTime(files[i], lastWriteTime);
            }
            return files;
        }

        /// <summary>
        /// Get items of item type "i" with using the item xml fragment passed in
        /// </summary>
        internal static IList<ProjectItem> GetItemsFromFragment(string fragment, bool allItems = false)
        {
            string content = FormatProjectContentsWithItemGroupFragment(fragment);

            IList<ProjectItem> items = GetItems(content, allItems);
            return items;
        }

        internal static string GetConcatenatedItemsOfType(this Project project, string itemType, string itemSeparator = ";")
        {
            return string.Join(itemSeparator, project.Items.Where(i => i.ItemType.Equals(itemType)).Select(i => i.EvaluatedInclude));
        }

        /// <summary>
        /// Get the items of type "i" in the project provided
        /// </summary>
        internal static IList<ProjectItem> GetItems(string content, bool allItems = false)
        {
            var projectXml = ProjectRootElement.Create(XmlReader.Create(new StringReader(CleanupFileContents(content))));
            Project project = new Project(projectXml);
            IList<ProjectItem> item = Helpers.MakeList(allItems ? project.Items : project.GetItems("i"));

            return item;
        }

        internal static string FormatProjectContentsWithItemGroupFragment(string fragment)
        {
            return
                $@"
                    <Project xmlns='http://schemas.microsoft.com/developer/msbuild/2003' >
                        <ItemGroup>
                            {fragment}
                        </ItemGroup>
                    </Project>
                ";
        }

        internal static void AssertItemsMatch(string expectedItemsString, ITaskItem[] actualItems, bool orderOfItemsShouldMatch)
        {
#if !IEXOD_DISABLE_TASKS
            throw new NotImplementedException(@"logic at src\Deprecated\Engine\Shared\UnitTests\ObjectModelHelpers.cs");
#endif
        }
    }

    /// <summary>
    /// Various generic unit test helper methods
    /// </summary>
    internal static partial class Helpers
    {
        internal static string Format(this string s, params object[] formatItems)
        {
            ErrorUtilities.VerifyThrowArgumentNull(s, nameof(s));

            return string.Format(s, formatItems);
        }

        internal static string GetOSPlatformAsString()
        {
            var currentPlatformString = string.Empty;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                currentPlatformString = "WINDOWS";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                currentPlatformString = "LINUX";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                currentPlatformString = "OSX";
            }
            else
            {
                Assert.True(false, "unrecognized current platform");
            }

            return currentPlatformString;
        }

        /// <summary>
        /// Returns the count of objects returned by an enumerator
        /// </summary>
        internal static int Count(IEnumerable enumerable)
        {
            int i = 0;
            foreach (object _ in enumerable)
            {
                i++;
            }

            return i;
        }

        /// <summary>
        /// Makes a temporary list out of an enumerable
        /// </summary>
        internal static List<T> MakeList<T>(IEnumerable<T> enumerable)
        {
            List<T> list = new List<T>();
            foreach (T item in enumerable)
            {
                list.Add(item);
            }
            return list;
        }

        /// <summary>
        /// Gets the first element in the enumeration, or null if there are none
        /// </summary>
        internal static T GetFirst<T>(IEnumerable<T> enumerable)
            where T : class
        {
            T first = null;

            foreach (T element in enumerable)
            {
                first = element;
                break;
            }

            return first;
        }

        /// <summary>
        /// Gets the last element in the enumeration, or null if there are none
        /// </summary>
        internal static T GetLast<T>(IEnumerable<T> enumerable)
            where T : class
        {
            T last = null;

            foreach (T item in enumerable)
            {
                last = item;
            }

            return last;
        }

        /// <summary>
        /// Makes a temporary dictionary out of an enumerable of keyvaluepairs.
        /// </summary>
        internal static Dictionary<string, V> MakeDictionary<V>(IEnumerable<KeyValuePair<string, V>> enumerable)
        {
            Dictionary<string, V> dictionary = new Dictionary<string, V>(StringComparer.OrdinalIgnoreCase);
            foreach (KeyValuePair<string, V> item in enumerable)
            {
                dictionary.Add(item.Key, item.Value);
            }
            return dictionary;
        }

        /// <summary>
        /// Verify that the two lists are value identical
        /// </summary>
        internal static void AssertListsValueEqual<T>(IList<T> one, IList<T> two)
        {
            Assert.Equal(one.Count, two.Count);

            for (int i = 0; i < one.Count; i++)
            {
                Assert.Equal(one[i], two[i]);
            }
        }

        /// <summary>
        /// Verify that the two collections are value identical
        /// </summary>
        internal static void AssertCollectionsValueEqual<T>(ICollection<T> one, ICollection<T> two)
        {
            Assert.Equal(one.Count, two.Count);

            foreach (T item in one)
            {
                Assert.True(two.Contains(item));
            }

            foreach (T item in two)
            {
                Assert.True(one.Contains(item));
            }
        }

        internal static void AssertDictionariesEqual<K, V>(IDictionary<K, V> x, IDictionary<K, V> y, Action<KeyValuePair<K, V>, KeyValuePair<K, V>> assertPairsEqual)
        {
            if (x == null || y == null)
            {
                Assert.True(x == null && y == null);
                return;
            }

            Assert.Equal(x.Count, y.Count);

            for (var i = 0; i < x.Count; i++)
            {
                var xPair = x.ElementAt(i);
                var yPair = y.ElementAt(i);

                assertPairsEqual(xPair, yPair);
            }
        }

        internal static void AssertDictionariesEqual(IDictionary<string, string> x, IDictionary<string, string> y)
        {
            AssertDictionariesEqual(x, y,
                (xPair, yPair) =>
                {
                    Assert.Equal(xPair.Key, yPair.Key);
                    Assert.Equal(xPair.Value, yPair.Value);
                });
        }

        internal static void ShouldBeSameIgnoringOrder<K, V>(this IDictionary<K, V> a, IReadOnlyDictionary<K, V> b)
        {
            a.ShouldBeSubsetOf(b);
            b.ShouldBeSubsetOf(a);
            a.Count.ShouldBe(b.Count);
        }

        internal static void ShouldBeSameIgnoringOrder<K>(this IEnumerable<K> a, IEnumerable<K> b)
        {
            a.ShouldBeSubsetOf(b);
            b.ShouldBeSubsetOf(a);
            a.Count().ShouldBe(b.Count());
        }

        internal static void ShouldBeSetEquivalentTo<K>(this IEnumerable<K> a, IEnumerable<K> b)
        {
            a.ShouldBeSubsetOf(b);
            b.ShouldBeSubsetOf(a);
        }

        /// <summary>
        /// Verify that the two enumerables are value identical
        /// </summary>
        internal static void AssertEnumerationsValueEqual<T>(IEnumerable<T> one, IEnumerable<T> two)
        {
            List<T> listOne = new List<T>();
            List<T> listTwo = new List<T>();

            foreach (T item in one)
            {
                listOne.Add(item);
            }

            foreach (T item in two)
            {
                listTwo.Add(item);
            }

            AssertCollectionsValueEqual(listOne, listTwo);
        }

        /// <summary>
        /// Build a project with the provided content in memory.
        /// Assert that it succeeded, and return the mock logger with the output.
        /// </summary>
        internal static MockLogger BuildProjectWithNewOMExpectSuccess(string content, Dictionary<string, string> globalProperties = null)
        {
            MockLogger logger;
            bool result;
            BuildProjectWithNewOM(content, out logger, out result, false, globalProperties);
            Assert.True(result);

            return logger;
        }

        /// <summary>
        /// Build a project in memory using the new OM
        /// </summary>
        private static void BuildProjectWithNewOM(string content, out MockLogger logger, out bool result, bool allowTaskCrash, Dictionary<string, string> globalProperties = null)
        {
            // Replace the crazy quotes with real ones
            content = ObjectModelHelpers.CleanupFileContents(content);

            Project project = new Project(XmlReader.Create(new StringReader(content)), globalProperties, toolsOptions: ProjectToolsOptions.Default);
            logger = new MockLogger();
            logger.AllowTaskCrashes = allowTaskCrash;
            List<ILogger> loggers = new List<ILogger>();
            loggers.Add(logger);
            result = project.Build(loggers);
        }

        public static MockLogger BuildProjectContentUsingBuildManagerExpectResult(string content, BuildResultCode expectedResult)
        {
            var logger = new MockLogger();

            var result = BuildProjectContentUsingBuildManager(content, logger);

            result.OverallResult.ShouldBe(expectedResult);

            return logger;
        }

        public static BuildResult BuildProjectContentUsingBuildManager(string content, MockLogger logger, BuildParameters parameters = null)
        {
            // Replace the crazy quotes with real ones
            content = ObjectModelHelpers.CleanupFileContents(content);

            using (var env = TestEnvironment.Create())
            {
                var testProject = env.CreateTestProjectWithFiles(content.Cleanup());

                return BuildProjectFileUsingBuildManager(testProject.ProjectFile, logger, parameters);
            }
        }

        public static BuildResult BuildProjectFileUsingBuildManager(string projectFile, MockLogger logger = null, BuildParameters parameters = null)
        {
            using (var buildManager = new BuildManager())
            {
                parameters = parameters ?? new BuildParameters();

                if (logger != null)
                {
                    parameters.Loggers = parameters.Loggers == null
                        ? new[] {logger}
                        : parameters.Loggers.Concat(new[] {logger});
                }

                var request = new BuildRequestData(
                    projectFile,
                    new Dictionary<string, string>(),
                    MSBuildConstants.CurrentToolsVersion,
                    new string[] {},
                    null);

                var result = buildManager.Build(
                    parameters,
                    request);

                return result;
            }
        }

        /// <summary>
        /// Build a project with the provided content in memory.
        /// Assert that it fails, and return the mock logger with the output.
        /// </summary>
        internal static MockLogger BuildProjectWithNewOMExpectFailure(string content, bool allowTaskCrash)
        {
            MockLogger logger;
            bool result;
            BuildProjectWithNewOM(content, out logger, out result, allowTaskCrash);
            Assert.False(result);
            return logger;
        }

        /// <summary>
        /// Compare the expected project XML to actual project XML, after doing a little normalization
        /// of quotations/whitespace.
        /// </summary>
        /// <param name="newExpectedProjectContents"></param>
        /// <param name="newActualProjectContents"></param>
        internal static void CompareProjectXml(string newExpectedProjectContents, string newActualProjectContents)
        {
            // Replace single-quotes with double-quotes, and normalize whitespace.
            newExpectedProjectContents =
                ObjectModelHelpers.NormalizeXmlWhitespace(
                    ObjectModelHelpers.CleanupFileContents(newExpectedProjectContents));

            // Compare the actual XML with the expected XML.
            if (newExpectedProjectContents != newActualProjectContents)
            {
                Console.WriteLine("================================= EXPECTED ===========================================");
                Console.WriteLine(newExpectedProjectContents);
                Console.WriteLine();
                Console.WriteLine("================================== ACTUAL ============================================");
                Console.WriteLine(newActualProjectContents);
                Console.WriteLine();
                Assert.Equal(newExpectedProjectContents, newActualProjectContents); // "Project XML does not match expected XML.  See 'Standard Out' tab for details."
            }
        }

        /// <summary>
        /// Verify that the saved project content matches the provided content
        /// </summary>
        internal static void VerifyAssertProjectContent(string expected, Project project)
        {
            VerifyAssertProjectContent(expected, project.Xml);
        }

        /// <summary>
        /// Verify that the saved project content matches the provided content
        /// </summary>
        internal static void VerifyAssertProjectContent(string expected, ProjectRootElement project, bool ignoreFirstLineOfActual = true)
        {
            VerifyAssertLineByLine(expected, project.RawXml, ignoreFirstLineOfActual);
        }

        /// <summary>
        /// Verify that the expected content matches the actual content
        /// </summary>
        internal static void VerifyAssertLineByLine(string expected, string actual)
        {
            VerifyAssertLineByLine(expected, actual, false /* do not ignore first line */);
        }

        /// <summary>
        /// Write the given <see cref="projectContents"/> in a new temp directory and create the given <see cref="files"/> relative to the project
        /// </summary>
        /// <returns>the path to the temp root directory that contains the project and files</returns>
        internal static string CreateProjectInTempDirectoryWithFiles(string projectContents, string[] files, out string createdProjectFile, out string[] createdFiles, string relativePathFromRootToProject = ".")
        {
            var root = GetTempDirectoryWithGuid();
            Directory.CreateDirectory(root);

            var projectDir = Path.Combine(root, relativePathFromRootToProject);
            Directory.CreateDirectory(projectDir);

            createdProjectFile = Path.Combine(projectDir, "build.proj");
            File.WriteAllText(createdProjectFile, ObjectModelHelpers.CleanupFileContents(projectContents));

            createdFiles = CreateFilesInDirectory(root, files);

            return root;
        }

        private static string GetTempDirectoryWithGuid()
        {
            return Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Creates a bunch of temporary files with the specified names and returns
        /// their full paths (so they can ultimately be cleaned up)
        /// </summary>
        internal static string[] CreateFiles(params string[] files)
        {
            string directory = GetTempDirectoryWithGuid();
            Directory.CreateDirectory(directory);

            return CreateFilesInDirectory(directory, files);
        }

        /// <summary>
        /// Creates a bunch of temporary files in the given directory with the specified names and returns
        /// their full paths (so they can ultimately be cleaned up)
        /// </summary>
        internal static string[] CreateFilesInDirectory(string rootDirectory, params string[] files)
        {
            if (files == null)
            {
                return null;
            }

            Assert.True(FileSystems.Default.DirectoryExists(rootDirectory), $"Directory {rootDirectory} does not exist");

            var result = new string[files.Length];

            for (var i = 0; i < files.Length; i++)
            {
                // On Unix there is the risk of creating one file with '\' in its name instead of directories.
                // Therefore split the arguments into path fragments and recompose the path.
                var fileFragments = SplitPathIntoFragments(files[i]);
                var rootDirectoryFragments = SplitPathIntoFragments(rootDirectory);
                var pathFragments = rootDirectoryFragments.Concat(fileFragments);

                var fullPath = Path.Combine(pathFragments.ToArray());

                var directoryName = Path.GetDirectoryName(fullPath);

                Directory.CreateDirectory(directoryName);
                Assert.True(FileSystems.Default.DirectoryExists(directoryName));

                File.WriteAllText(fullPath, string.Empty);
                Assert.True(FileSystems.Default.FileExists(fullPath));

                result[i] = fullPath;
            }

            return result;
        }

        internal delegate TransientTestFile CreateProjectFileDelegate(
            TestEnvironment env,
            int projectNumber,
            int[] projectReferences = null,
            Dictionary<string, string[]> projectReferenceTargets = null,
            string defaultTargets = null,
            string extraContent = null);

        internal static TransientTestFile CreateProjectFile(
            TestEnvironment env,
            int projectNumber,
            int[] projectReferences = null,
            Dictionary<string, string[]> projectReferenceTargets = null,
            string defaultTargets = null,
            string extraContent = null
            )
        {
            var sb = new StringBuilder(64);

            sb.Append(
                defaultTargets == null
                    ? "<Project>"
                    : $"<Project DefaultTargets=\"{defaultTargets}\">");

            sb.Append("<ItemGroup>");

            if (projectReferences != null)
            {
                foreach (int projectReference in projectReferences)
                {
                    sb.AppendFormat("<ProjectReference Include=\"{0}.proj\" />", projectReference);
                }
            }

            if (projectReferenceTargets != null)
            {
                foreach (KeyValuePair<string, string[]> pair in projectReferenceTargets)
                {
                    sb.AppendFormat("<ProjectReferenceTargets Include=\"{0}\" Targets=\"{1}\" />", pair.Key, string.Join(";", pair.Value));
                }
            }

            sb.Append("</ItemGroup>");

            
            foreach (var defaultTarget in (defaultTargets ?? string.Empty).Split(MSBuildConstants.SemicolonChar, StringSplitOptions.RemoveEmptyEntries))
            {
                sb.Append($"<Target Name='{defaultTarget}'/>");
            }

            sb.Append(extraContent ?? string.Empty);

            sb.Append("</Project>");

            return env.CreateFile(projectNumber + ".proj", sb.ToString());
        }

        internal static ProjectGraph CreateProjectGraph(
            TestEnvironment env,
            IDictionary<int, int[]> dependencyEdges,
            IDictionary<int, string> extraContentPerProjectNumber,
            string extraContentForAllNodes = null)
        {
            return CreateProjectGraph(
                env: env,
                dependencyEdges: dependencyEdges,
                globalProperties: null,
                createProjectFile: (environment, projectNumber, references, projectReferenceTargets, defaultTargets, extraContent) =>
                {
                    extraContent = extraContentPerProjectNumber != null && extraContentPerProjectNumber.TryGetValue(projectNumber, out var content)
                        ? content
                        : string.Empty;

                    extraContent += extraContentForAllNodes ?? string.Empty;

                    return CreateProjectFile(
                        environment,
                        projectNumber,
                        references,
                        projectReferenceTargets,
                        defaultTargets,
                        extraContent.Cleanup());
                });
        }

        internal static ProjectGraph CreateProjectGraph(
            TestEnvironment env,
            // direct dependencies that the kvp.key node has on the nodes represented by kvp.value
            IDictionary<int, int[]> dependencyEdges,
            IDictionary<string, string> globalProperties = null,
            CreateProjectFileDelegate createProjectFile = null,
            IEnumerable<int> entryPoints = null,
            ProjectCollection projectCollection = null)
        {
            createProjectFile = createProjectFile ?? CreateProjectFile;

            var nodes = new Dictionary<int, (bool IsRoot, string ProjectPath)>();

            // add nodes with dependencies
            foreach (var nodeDependencies in dependencyEdges)
            {
                var parent = nodeDependencies.Key;

                if (!nodes.ContainsKey(parent))
                {
                    var file = createProjectFile(env, parent, nodeDependencies.Value);
                    nodes[parent] = (IsRoot(parent), file.Path);
                }
            }

            // add what's left, nodes without dependencies
            foreach (var nodeDependencies in dependencyEdges)
            {
                if (nodeDependencies.Value == null)
                {
                    continue;
                }

                foreach (var reference in nodeDependencies.Value)
                {
                    if (!nodes.ContainsKey(reference))
                    {
                        var file = createProjectFile(env, reference);
                        nodes[reference] = (false, file.Path);
                    }
                }
            }

            var entryProjectFiles = entryPoints != null
                            ? nodes.Where(n => entryPoints.Contains(n.Key)).Select(n => n.Value.ProjectPath)
                            : nodes.Where(n => n.Value.IsRoot).Select(n => n.Value.ProjectPath);

            return new ProjectGraph(
                entryProjectFiles,
                globalProperties ?? new Dictionary<string, string>(),
                projectCollection ?? env.CreateProjectCollection()
                    .Collection
                );

            bool IsRoot(int node)
            {
                foreach (var nodeDependencies in dependencyEdges)
                {
                    if (nodeDependencies.Value != null && nodeDependencies.Value.Contains(node))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        private static string[] SplitPathIntoFragments(string path)
        {
            // Both Path.AltDirectorSeparatorChar and Path.DirectorySeparator char return '/' on OSX,
            // which renders them useless for the following case where I want to split a path that may contain either separator
            var splits = path.Split(MSBuildConstants.ForwardSlashBackslash);

            // if the path is rooted then the first split is either empty (Unix) or 'c:' (Windows)
            // in this case the root must be restored back to '/' (Unix) or 'c:\' (Windows)
            if (Path.IsPathRooted(path))
            {
                splits[0] = Path.GetPathRoot(path);
            }

            return splits;
        }

        /// <summary>
        /// Deletes a bunch of files, including their containing directories
        /// if they become empty
        /// </summary>
        internal static void DeleteFiles(params string[] paths)
        {
            foreach (string path in paths)
            {
                if (FileSystems.Default.FileExists(path))
                {
                    File.Delete(path);
                }

                string directory = Path.GetDirectoryName(path);
                if (FileSystems.Default.DirectoryExists(directory) && (Directory.GetFileSystemEntries(directory).Length == 0))
                {
                    Directory.Delete(directory);
                }
            }
        }

        /// <summary>
        /// Given two methods accepting no parameters and returning none, verifies they
        /// both throw, and throw the same exception type.
        /// </summary>
        internal static void VerifyAssertThrowsSameWay(Action method1, Action method2)
        {
            Exception ex1 = null;
            Exception ex2 = null;

            try
            {
                method1();
            }
            catch (Exception ex)
            {
                ex1 = ex;
            }

            try
            {
                method2();
            }
            catch (Exception ex)
            {
                ex2 = ex;
            }

            if (ex1 == null && ex2 == null)
            {
                Assert.True(false, "Neither threw");
            }

            Assert.NotNull(ex1); // "First method did not throw, second: {0}", ex2 == null ? "" : ex2.GetType() + ex2.Message);
            Assert.NotNull(ex2); // "Second method did not throw, first: {0}", ex1 == null ? "" : ex1.GetType() + ex1.Message);
            Assert.Equal(ex1.GetType(), ex2.GetType()); // "Both methods threw but the first threw {0} '{1}' and the second threw {2} '{3}'", ex1.GetType(), ex1.Message, ex2.GetType(), ex2.Message);

            Console.WriteLine("COMPARE EXCEPTIONS:\n\n#1: {0}\n\n#2: {1}", ex1.Message, ex2.Message);
        }

        /// <summary>
        /// Verify method throws invalid operation exception.
        /// </summary>
        internal static void VerifyAssertThrowsInvalidOperation(Action method)
        {
            Assert.Throws<InvalidOperationException>(method);
        }

        /// <summary>
        /// Verify that the expected content matches the actual content
        /// </summary>
        internal static void VerifyAssertLineByLine(string expected, string actual, bool ignoreFirstLineOfActual, ITestOutputHelper testOutput = null)
        {
            Action<string> LogLine = testOutput == null ? (Action<string>) Console.WriteLine : testOutput.WriteLine;

            string[] actualLines = SplitIntoLines(actual);

            if (ignoreFirstLineOfActual)
            {
                // Remove the first line of the actual content we got back,
                // since it's just the xml declaration, which we don't care about
                string[] temporary = new string[actualLines.Length - 1];

                for (int i = 0; i < temporary.Length; i++)
                {
                    temporary[i] = actualLines[i + 1];
                }

                actualLines = temporary;
            }

            string[] expectedLines = SplitIntoLines(expected);

            bool expectedAndActualDontMatch = false;
            for (int i = 0; i < Math.Min(actualLines.Length, expectedLines.Length); i++)
            {
                if (expectedLines[i] != actualLines[i])
                {
                    expectedAndActualDontMatch = true;
                    LogLine("<   " + expectedLines[i] + "\n>   " + actualLines[i] + "\n");
                }
            }

            if (actualLines.Length == expectedLines.Length && expectedAndActualDontMatch)
            {
                string output = "\r\n#################################Expected#################################\n" + string.Join("\r\n", expectedLines);
                output += "\r\n#################################Actual#################################\n" + string.Join("\r\n", actualLines);

                Assert.True(false, output);
            }

            if (actualLines.Length > expectedLines.Length)
            {
                LogLine("\n#################################Expected#################################\n" + string.Join("\n", expectedLines));
                LogLine("#################################Actual#################################\n" + string.Join("\n", actualLines));

                Assert.True(false, "Expected content was shorter, actual had this extra line: '" + actualLines[expectedLines.Length] + "'");
            }
            else if (actualLines.Length < expectedLines.Length)
            {
                LogLine("\n#################################Expected#################################\n" + string.Join("\n", expectedLines));
                LogLine("#################################Actual#################################\n" + string.Join("\n", actualLines));

                Assert.True(false, "Actual content was shorter, expected had this extra line: '" + expectedLines[actualLines.Length] + "'");
            }
        }

        /// <summary>
        /// Clear the dirty flag of a ProjectRootElement by saving to a dummy writer.
        /// </summary>
        internal static void ClearDirtyFlag(ProjectRootElement project)
        {
            project.Save(new StringWriter());
            Assert.False(project.HasUnsavedChanges);
        }

        /// <summary>
        /// Gets a command that can be used by an Exec task to sleep for the specified amount of time.
        /// </summary>
        /// <param name="timeSpan">A <see cref="TimeSpan"/> representing the amount of time to sleep.</param>
        internal static string GetSleepCommand(TimeSpan timeSpan)
        {
            return string.Format(
                GetSleepCommandTemplate(),
                NativeMethodsShared.IsWindows
                    ? timeSpan.TotalMilliseconds // powershell can't handle floating point seconds, so give it milliseconds
                    : timeSpan.TotalSeconds);
        }

        /// <summary>
        /// Gets a command template that can be used by an Exec task to sleep for the specified amount of time. The string has to be formatted with the number of seconds to sleep
        /// </summary>
        internal static string GetSleepCommandTemplate()
        {
            return
                NativeMethodsShared.IsWindows
                    ? "@powershell -NoLogo -NoProfile -command &quot;Start-Sleep -Milliseconds {0}&quot; &gt;nul"
                    : "sleep {0}";
        }



        /// <summary>
        /// Break the provided string into an array, on newlines
        /// </summary>
        private static string[] SplitIntoLines(string content)
        {
            string[] result = content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            return result;
        }

        /// <summary>
        /// Used for file matching tests
        /// MSBuild does not accept forward slashes on rooted paths, so those are returned unchanged
        /// </summary>
        internal static string ToForwardSlash(string path) =>
            Path.IsPathRooted(path)
                ? path
                : path.ToSlash();

        internal class ElementLocationComparerIgnoringType : IEqualityComparer<ElementLocation>
        {
            public bool Equals(ElementLocation x, ElementLocation y)
            {
                if (x == null)
                {
                    return y == null;
                }

                if (x.Line != y.Line || x.Column != y.Column)
                {
                    return false;
                }

                if (!string.Equals(x.File, y.File, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                return true;
            }

            public int GetHashCode(ElementLocation obj)
            {
                return obj.Line.GetHashCode() ^ obj.Column.GetHashCode() ^ obj.File.GetHashCode();
            }
        }

        internal class BuildManagerSession : IDisposable
        {
            private readonly TestEnvironment _env;
            private readonly BuildManager _buildManager;

            public MockLogger Logger { get; set; }


            public BuildManagerSession(
                TestEnvironment env,
                BuildParameters buildParametersPrototype = null,
                bool enableNodeReuse = false,
                bool shutdownInProcNode = true,
                IEnumerable<BuildManager.DeferredBuildMessage> deferredMessages = null)
            {
                _env = env;

                Logger = new MockLogger(_env.Output);
                var loggers = new[] {Logger};

                var actualBuildParameters = buildParametersPrototype?.Clone() ?? new BuildParameters();

                actualBuildParameters.Loggers = actualBuildParameters.Loggers == null
                    ? loggers
                    : actualBuildParameters.Loggers.Concat(loggers).ToArray();

                actualBuildParameters.ShutdownInProcNodeOnBuildFinish = shutdownInProcNode;
                actualBuildParameters.EnableNodeReuse = enableNodeReuse;

                _buildManager = new BuildManager();
                _buildManager.BeginBuild(actualBuildParameters, deferredMessages);
            }

            public BuildResult BuildProjectFile(string projectFile, string[] entryTargets = null)
            {
                var buildResult = _buildManager.BuildRequest(
                    new BuildRequestData(projectFile,
                        new Dictionary<string, string>(),
                        MSBuildConstants.CurrentToolsVersion,
                        entryTargets ?? new string[0],
                        null));

                return buildResult;
            }

            public void Dispose()
            {
                _buildManager.EndBuild();
                _buildManager.Dispose();
            }
        }
    }
}
