using BarLauncher.Workspacer.Lib.DomainModel;

namespace BarLauncher.Workspacer.Lib.Core.Service
{
    public interface IWorkspacerConfigurationRepository
    {
        void Init();

        WorkspacerConfiguration GetConfiguration();

        void SaveConfiguration(WorkspacerConfiguration configuration);
    }
}