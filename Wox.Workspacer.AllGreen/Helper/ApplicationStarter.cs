using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Wox.Workspacer.Core.Service;
using Wox.Workspacer.Mock.Service;
using Wox.Workspacer.Service;

namespace Wox.Workspacer.AllGreen.Helper
{
    public class ApplicationStarter
    {
        public WoxContextServiceMock WoxContextService { get; set; }

        public QueryServiceMock QueryService { get; set; }

        public SystemServiceMock SystemService { get; set; }

        public IWoxResultFinder WoxWebAppResultFinder { get; set; }

        public IWorkspacerService WorkspacerService { get; set; }

        private string TestName { get; set; }

        public string TestPath => SystemService.ApplicationDataPath;

        public void Init(string testName)
        {
            TestName = testName;
            QueryServiceMock queryService = new QueryServiceMock();
            WoxContextServiceMock woxContextService = new WoxContextServiceMock(queryService);
            SystemServiceMock systemService = new SystemServiceMock();
            IDataAccessService dataAccessService = new DataAccessService(systemService);
            IWorkspacerService workspacerService = new WorkspacerService();
            IWoxResultFinder woxWebAppResultFinder = new WorkspacerResultFinder(woxContextService, workspacerService);

            systemService.ApplicationDataPath = GetApplicationDataPath();

            WoxContextService = woxContextService;
            QueryService = queryService;
            SystemService = systemService;
            WorkspacerService = workspacerService;
            WoxWebAppResultFinder = woxWebAppResultFinder;

            WoxContextService.AddQueryFetcher("work", WoxWebAppResultFinder);
        }

        public void Start()
        {
            WorkspacerService.Init();
        }

        private static string GetThisAssemblyDirectory()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var thisAssemblyCodeBase = assembly.CodeBase;
            var thisAssemblyDirectory = Path.GetDirectoryName(new Uri(thisAssemblyCodeBase).LocalPath);

            return thisAssemblyDirectory;
        }

        private string GetApplicationDataPath()
        {
            var thisAssemblyDirectory = GetThisAssemblyDirectory();
            var path = Path.Combine(Path.Combine(thisAssemblyDirectory, "AllGreen"), string.Format("AG_{0:yyyyMMdd-HHmmss-fff}_{1}", DateTime.Now, TestName));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
}
