using net.r_eg.IeXod.Evaluation;
using Shouldly;
using Xunit;

namespace net.r_eg.IeXod.UnitTests.Evaluation
{
    public class ProjectCollection_Tests
    {
        [Fact]
        public void ProjectRootElementCache_IsDeterminedByEnvironmentVariable()
        {
            var collectionWithDefaultCache = new ProjectCollection();
            collectionWithDefaultCache.ProjectRootElementCache.ShouldBeOfType<ProjectRootElementCache>();

            const string envKey = "MsBuildUseSimpleProjectRootElementCacheConcurrency";

            using (TestEnvironment env = TestEnvironment.Create())
            {
                env.SetEnvironmentVariable(envKey, "true");
                var collectionWithSimpleCache = new ProjectCollection();
                collectionWithSimpleCache.ProjectRootElementCache.ShouldBeOfType<SimpleProjectRootElementCache>();
            }
        }
    }
}
