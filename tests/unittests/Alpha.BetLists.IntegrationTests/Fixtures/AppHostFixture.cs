using System;
using System.IO;
using MvcIntegrationTestFramework.Hosting;

namespace Alpha.BetLists.IntegrationTests.Fixtures
{
    public class AppHostFixture : IDisposable
    {
        private const string projectName = "src\\Alpha.BetLists.Site";
        private string projectPath;

        public AppHostFixture()
        {
            projectPath = GetProjectPath(projectName);
            CopyConfigFile(projectPath);

            AppHost.LoadAllBinaries = true;
            AppHost = AppHost.Simulate(projectName);
        }

        public AppHost AppHost { get; }

        public void Dispose()
        {
            AppHost.Dispose();
            File.Copy("Web.Original.config", Path.Combine(projectPath, "Web.config"), true);
        }

        private static string GetProjectPath(string projectName)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            while (Path.GetPathRoot(baseDirectory) != baseDirectory)
            {
                baseDirectory = baseDirectory.Substring(0, baseDirectory.LastIndexOf("\\", StringComparison.Ordinal));
                var mvcPath = Path.Combine(baseDirectory, projectName);

                if (Directory.Exists(mvcPath))
                {
                    return mvcPath;
                }
            }

            return null;
        }

        private void CopyConfigFile(string projectPath)
        {
            if (!string.IsNullOrEmpty(projectPath))
            {
                File.Copy("Web.Integration.config", Path.Combine(projectPath, "Web.config"), true);
            }
        }
    }
}
