using Wox.Workspacer.DomainModel;

namespace Wox.Workspacer.Core.Service
{
    public interface IWorkspacerConfigurationRepository
    {
        void Init();

        WorkspacerConfiguration GetConfiguration();

        void SaveConfiguration(WorkspacerConfiguration configuration);
    }
}