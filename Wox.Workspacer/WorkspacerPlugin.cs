using FluentDataAccess.Service;
using Wox.EasyHelper;
using Wox.Workspacer.Service;

namespace Wox.Workspacer
{
    public class WorkspacerPlugin : PluginBase<WorkspacerResultFinder>
    {
        public override WorkspacerResultFinder PrepareContext()
        {
            var systemService = new SystemService("Wox.Workspacer");
            var dataAccessService = new DataAccessService(systemService);
            var workspacerConfigurationRepository = new WorkspacerConfigurationRepository(dataAccessService);
            var workspacerRepoRepository = new WorkspacerRepoRepository(dataAccessService);
            var workspacerService = new WorkspacerService(dataAccessService, workspacerConfigurationRepository, workspacerRepoRepository, systemService);
            var workspacerResultFinder = new WorkspacerResultFinder(WoxContextService, workspacerService);

            workspacerService.Init();
            return workspacerResultFinder;
        }
    }
}