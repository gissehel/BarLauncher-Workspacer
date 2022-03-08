using FluentDataAccess;
using BarLauncher.EasyHelper.Flow.Launcher;
using BarLauncher.EasyHelper.Core.Service;
using BarLauncher.EasyHelper.Service;
using BarLauncher.Workspacer.Lib.Service;
using BarLauncher.EasyHelper.Flow.Launcher.Service;

namespace BarLauncher.Workspacer.Flow.Launcher
{
    public class WorkspacerPlugin : FlowLauncherPlugin
    {
        public override IBarLauncherResultFinder PrepareContext()
        {
            var systemService = new FlowLauncherSystemService("BarLauncher-Workspacer", BarLauncherContextService as BarLauncherContextService);
            var workspacerSystemService = new WorkspacerSystemService(systemService, "Wox.Workspacer");
            var dataAccessService = DataAccessSQLite.GetService(workspacerSystemService);
            var workspacerConfigurationRepository = new WorkspacerConfigurationRepository(dataAccessService);
            var workspacerRepoRepository = new WorkspacerRepoRepository(dataAccessService);
            var workspacerService = new WorkspacerService(dataAccessService, workspacerConfigurationRepository, workspacerRepoRepository, systemService, workspacerSystemService)
            {
                UseStandardDirectoryOpener=true,
            };
            var workspacerResultFinder = new WorkspacerResultFinder(BarLauncherContextService, workspacerService);

            return workspacerResultFinder;
        }
    }
}