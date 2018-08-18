using FluentDataAccess.Service;
using Wox.EasyHelper;
using Wox.EasyHelper.Core.Service;
using Wox.EasyHelper.Service;
using Wox.Workspacer.Service;

namespace Wox.Workspacer
{
    public class WorkspacerPlugin : WoxPlugin
    {
        public override IWoxResultFinder PrepareContext()
        {
            var systemService = new SystemService("Wox.Workspacer");
            var workspacerSystemService = new WorkspacerSystemService(systemService);
            var dataAccessService = new DataAccessService(workspacerSystemService);
            var workspacerConfigurationRepository = new WorkspacerConfigurationRepository(dataAccessService);
            var workspacerRepoRepository = new WorkspacerRepoRepository(dataAccessService);
            var workspacerService = new WorkspacerService(dataAccessService, workspacerConfigurationRepository, workspacerRepoRepository, systemService, workspacerSystemService);
            var workspacerResultFinder = new WorkspacerResultFinder(WoxContextService, workspacerService);

            return workspacerResultFinder;
        }
    }
}