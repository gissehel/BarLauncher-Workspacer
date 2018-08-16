using FluentDataAccess.Core.Service;
using FluentDataAccess.Service;
using System;
using System.IO;
using System.Reflection;
using Wox.EasyHelper;
using Wox.EasyHelper.Test.Mock.Service;
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

        public WorkspacerSystemServiceMock WorkspacerSystemService { get; set; }

        public WorkspacerResultFinder WoxResultFinder { get; set; }

        public IWorkspacerService WorkspacerService { get; set; }

        private string TestName { get; set; }

        public string TestPath => SystemService.ApplicationDataPath;

        public void Init(string testName)
        {
            TestName = testName;
            QueryServiceMock queryService = new QueryServiceMock();
            WoxContextServiceMock woxContextService = new WoxContextServiceMock(queryService);
            SystemServiceMock systemService = new SystemServiceMock();
            WorkspacerSystemServiceMock workspacerSystemService = new WorkspacerSystemServiceMock(systemService);
            IDataAccessService dataAccessService = new DataAccessService(workspacerSystemService);
            IWorkspacerConfigurationRepository workspacerConfigurationRepository = new WorkspacerConfigurationRepository(dataAccessService);
            IWorkspacerRepoRepository workspacerRepoRepository = new WorkspacerRepoRepository(dataAccessService);
            IWorkspacerService workspacerService = new WorkspacerService(dataAccessService, workspacerConfigurationRepository, workspacerRepoRepository, systemService, workspacerSystemService);
            WorkspacerResultFinder workspacerResultFinder = new WorkspacerResultFinder(woxContextService, workspacerService);

            systemService.ApplicationDataPath = GetApplicationDataPath();

            WoxContextService = woxContextService;
            QueryService = queryService;
            SystemService = systemService;
            WorkspacerSystemService = workspacerSystemService;
            WorkspacerService = workspacerService;
            WoxResultFinder = workspacerResultFinder;

            WoxContextService.AddQueryFetcher("work", WoxResultFinder);
        }

        public void Start()
        {
            WorkspacerService.Init();
            WoxResultFinder.Init();
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
            var path = Path.Combine(Path.Combine(thisAssemblyDirectory, "AllGreen"), "AG_{0:yyyyMMdd-HHmmss-fff}_{1}".FormatWith(DateTime.Now, TestName));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
}