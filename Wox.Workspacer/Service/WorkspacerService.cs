using Wox.Workspacer.Core.Service;
using Wox.Workspacer.DomainModel;

namespace Wox.Workspacer.Service
{
    public class WorkspacerService : IWorkspacerService
    {
        public WorkspacerConfiguration GetConfiguration() => WorkspacerConfigurationRepository.GetConfiguration();

        private IDataAccessService DataAccessService { get; set; }
        private IWorkspacerConfigurationRepository WorkspacerConfigurationRepository { get; set; }

        public WorkspacerService(IDataAccessService dataAccessService, IWorkspacerConfigurationRepository workspacerConfigurationRepository)
        {
            DataAccessService = dataAccessService;
            WorkspacerConfigurationRepository = workspacerConfigurationRepository;
        }

        public void Init()
        {
            DataAccessService.Init();
            WorkspacerConfigurationRepository.Init();
        }

        public void SaveConfiguration(WorkspacerConfiguration configuration) => WorkspacerConfigurationRepository.SaveConfiguration(configuration);
    }
}