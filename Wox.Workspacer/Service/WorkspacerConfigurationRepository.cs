using Wox.Workspacer.Core.Service;
using Wox.Workspacer.DomainModel;

namespace Wox.Workspacer.Service
{
    public class WorkspacerConfigurationRepository : IWorkspacerConfigurationRepository
    {
        private IDataAccessService DataAccessService { get; set; }
        private WorkspacerConfiguration WorkspacerConfiguration { get; set; }

        public WorkspacerConfigurationRepository(IDataAccessService dataAccessService)
        {
            DataAccessService = dataAccessService;
        }

        public void Init()
        {
            DataAccessService
                 .GetQuery("create table if not exists configuration (id integer primary key, key text, value text, constraint key_unique unique (key));")
                 .Execute();
        }

        private class ConfigurationItem
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        public WorkspacerConfiguration GetConfiguration()
        {
            if (WorkspacerConfiguration == null)
            {
                WorkspacerConfiguration = new WorkspacerConfiguration { Launcher = "\"%1\"" };
            }
            var items = DataAccessService
                .GetQuery("select key, value from configuration;")
                .Returning<ConfigurationItem>()
                .Reading("key", (ConfigurationItem item, string data) => item.Key = data)
                .Reading("value", (ConfigurationItem item, string data) => item.Value = data)
                .Execute();
            foreach (var item in items)
            {
                switch (item.Key)
                {
                    case nameof(WorkspacerConfiguration.Launcher):
                        WorkspacerConfiguration.Launcher = item.Value;
                        break;

                    default:
                        break;
                }
            }
            return WorkspacerConfiguration;
        }

        public void SaveConfiguration(WorkspacerConfiguration configuration)
        {
            WorkspacerConfiguration = configuration;
            SaveConfigurationItem(nameof(WorkspacerConfiguration.Launcher), WorkspacerConfiguration.Launcher);
        }

        private void SaveConfigurationItem(string name, string value)
        {
            DataAccessService
                .GetQuery("insert or replace into configuration (key, value) values (@key, @value)")
                .WithParameter("key", name)
                .WithParameter("value", value)
                .Execute()
                ;
        }
    }
}