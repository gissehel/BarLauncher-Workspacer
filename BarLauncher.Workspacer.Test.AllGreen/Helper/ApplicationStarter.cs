using FluentDataAccess;
using System;
using System.IO;
using System.Reflection;
using BarLauncher.EasyHelper;
using BarLauncher.EasyHelper.Test.Mock.Service;
using BarLauncher.Workspacer.Lib.Core.Service;
using BarLauncher.Workspacer.Lib.Service;
using BarLauncher.Workspacer.Test.Mock.Service;

namespace BarLauncher.Workspacer.Test.AllGreen.Helper
{
    public class ApplicationStarter
    {
        public BarLauncherContextServiceMock BarLauncherContextService { get; set; }

        public QueryServiceMock QueryService { get; set; }

        public SystemServiceMock SystemService { get; set; }

        public WorkspacerSystemServiceMock WorkspacerSystemService { get; set; }

        public WorkspacerResultFinder BarLauncherResultFinder { get; set; }

        public IWorkspacerService WorkspacerService { get; set; }

        private string TestName { get; set; }

        public string TestPath => SystemService.ApplicationDataPath;

        public void Init(string testName)
        {
            TestName = testName;
            QueryServiceMock queryService = new QueryServiceMock();
            BarLauncherContextServiceMock barLauncherContextService = new BarLauncherContextServiceMock(queryService);
            SystemServiceMock systemService = new SystemServiceMock();
            WorkspacerSystemServiceMock workspacerSystemService = new WorkspacerSystemServiceMock(systemService);
            IDataAccessService dataAccessService = DataAccessSQLite.GetService(workspacerSystemService);
            IWorkspacerConfigurationRepository workspacerConfigurationRepository = new WorkspacerConfigurationRepository(dataAccessService);
            IWorkspacerRepoRepository workspacerRepoRepository = new WorkspacerRepoRepository(dataAccessService);
            IWorkspacerService workspacerService = new WorkspacerService(dataAccessService, workspacerConfigurationRepository, workspacerRepoRepository, systemService, workspacerSystemService);
            WorkspacerResultFinder workspacerResultFinder = new WorkspacerResultFinder(barLauncherContextService, workspacerService);

            systemService.ApplicationDataPath = GetApplicationDataPath();

            BarLauncherContextService = barLauncherContextService;
            QueryService = queryService;
            SystemService = systemService;
            WorkspacerSystemService = workspacerSystemService;
            WorkspacerService = workspacerService;
            BarLauncherResultFinder = workspacerResultFinder;

            BarLauncherContextService.AddQueryFetcher("work", BarLauncherResultFinder);
        }

        public void Start()
        {
            WorkspacerService.Init();
            BarLauncherResultFinder.Init();
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