using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BuildSolution
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string solutionFile = @"D:\Code\WindowsFormsApp1\WindowsFormsApp1.sln";
            var properties = new Dictionary<string, string> {
                {"Configuration", "Release"}
            };

            var buildRequest = new BuildRequestData(solutionFile, properties, null, new [] { "Rebuild" }, null);
            var buildParameters = new BuildParameters();
            var buildLoggger = new InMemoryBuildLogger();
            buildParameters.Loggers = new[] { buildLoggger };
            var buildResult = BuildManager.DefaultBuildManager.Build(buildParameters, buildRequest);

            if (buildResult.OverallResult == BuildResultCode.Success)
            {
                Console.WriteLine("Build Succeeded");
            }
            else
            {
                var error = string.Empty;
                foreach (var buildError in buildLoggger.BuildErrors)
                {
                    error += buildError + Environment.NewLine;
                }
                MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        class InMemoryBuildLogger : ILogger
        {
            #region Properties

            public List<string> BuildErrors
            {
                get; private set;
            }

            public string Parameters
            {
                get; set;
            }

            public LoggerVerbosity Verbosity
            {
                get; set;
            }

            #endregion Properties

            #region Methods

            public void Initialize(IEventSource eventSource)
            {
                BuildErrors = new List<string>();
                eventSource.ErrorRaised += EventSourceOnErrorRaised;
            }

            public void Shutdown()
            {
            }

            private void EventSourceOnErrorRaised(object sender, BuildErrorEventArgs buildErrorEventArgs)
            {
                BuildErrors.Add(buildErrorEventArgs.Message);
            }

            #endregion Methods
        }
    }
}
