using System.Collections.Generic;
using net.r_eg.IeXod.Evaluation;
using net.r_eg.IeXod.Evaluation.Context;

namespace net.r_eg.IeXod.Definition
{
    /// <summary>
    ///     Common <see cref="Project" /> constructor arguments.
    /// </summary>
    public class ProjectOptions
    {
        /// <summary>
        /// Global properties to evaluate with.
        /// </summary>
        public IDictionary<string, string> GlobalProperties { get; set; }

        /// <summary>
        /// Options around toolset, sdk resolvers, and related. May be null.
        /// </summary>
        public ProjectToolsOptions ToolsOptions { get; set; }

        /// <summary>
        /// The <see cref="ProjectCollection"/> the project is added to. Default is <see cref="ProjectCollection.GlobalProjectCollection"/>/>
        /// </summary>
        public ProjectCollection ProjectCollection { get; set; }

        /// <summary>
        /// The <see cref="ProjectLoadSettings"/> to use for evaluation.
        /// </summary>
        public ProjectLoadSettings LoadSettings { get; set; } = ProjectLoadSettings.Default;

        /// <summary>
        /// The <see cref="EvaluationContext"/> to use for evaluation.
        /// </summary>
        public EvaluationContext EvaluationContext { get; set; }
    }
}
