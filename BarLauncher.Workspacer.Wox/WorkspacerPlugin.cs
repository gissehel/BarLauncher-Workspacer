using FluentDataAccess;
using BarLauncher.EasyHelper.Wox;
using BarLauncher.EasyHelper.Core.Service;
using BarLauncher.EasyHelper.Service;
using BarLauncher.Workspacer.Lib.Service;

namespace BarLauncher.Workspacer.Wox
{
    public class WorkspacerPlugin : WoxPlugin
    {
        public override IBarLauncherResultFinder PrepareContext()
        {
            var systemService = new SystemService("BarLauncher-Workspacer");
            var workspacerSystemService = new WorkspacerSystemService(systemService, "Wox.Workspacer");
            var dataAccessService = DataAccessSQLite.GetService(workspacerSystemService);
            var workspacerConfigurationRepository = new WorkspacerConfigurationRepository(dataAccessService);
            var workspacerRepoRepository = new WorkspacerRepoRepository(dataAccessService);
            var workspacerService = new WorkspacerService(dataAccessService, workspacerConfigurationRepository, workspacerRepoRepository, systemService, workspacerSystemService);
            var workspacerResultFinder = new WorkspacerResultFinder(BarLauncherContextService, workspacerService);

            return workspacerResultFinder;
        }
    }
}