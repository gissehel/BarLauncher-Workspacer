using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Wox.EasyHelper;
using Wox.Workspacer.Core.Service;
using Wox.Workspacer.DomainModel;
using Wox.Workspacer.Tool;

namespace Wox.Workspacer.Service
{
    public class WorkspacerService : IWorkspacerService

    {
        private IDataAccessService DataAccessService { get; set; }
        private IWorkspacerConfigurationRepository WorkspacerConfigurationRepository { get; set; }
        private IWorkspacerRepoRepository WorkspacerRepoRepository { get; set; }
        private ISystemService SystemService { get; set; }

        public WorkspacerService(IDataAccessService dataAccessService, IWorkspacerConfigurationRepository workspacerConfigurationRepository, IWorkspacerRepoRepository workspacerRepoRepository, ISystemService systemService)
        {
            DataAccessService = dataAccessService;
            WorkspacerConfigurationRepository = workspacerConfigurationRepository;
            WorkspacerRepoRepository = workspacerRepoRepository;
            SystemService = systemService;
        }

        public void Init()
        {
            DataAccessService.Init();
            WorkspacerConfigurationRepository.Init();
            WorkspacerRepoRepository.Init();
        }

        private string _luncherPattern = null;

        private string LauncherPattern => _luncherPattern ?? (_luncherPattern = GetConfiguration().Launcher);

        public WorkspacerConfiguration GetConfiguration() => WorkspacerConfigurationRepository.GetConfiguration();

        public void SaveConfiguration(WorkspacerConfiguration configuration)
        {
            _luncherPattern = configuration.Launcher;
            WorkspacerConfigurationRepository.SaveConfiguration(configuration);
        }

        public List<WorkspacerRepo> GetRepos() => WorkspacerRepoRepository.GetRepos().ToList();

        public string GetPathByName(string name) => WorkspacerRepoRepository.GetPath(name);

        public void SetPathByName(string name, string path)
        {
            SystemService.CreateDirectoryIfNotExists(path);
            WorkspacerRepoRepository.SetPath(name, path);
        }

        public void OpenDir(string path)
        {
            var fullCommand = LauncherPattern.Replace("%1", path);
            StartCommand(fullCommand);
        }

        private void StartCommand(string fullCommand)
        {
            if (fullCommand.StartsWith("\""))
            {
                var endingCommandPosition = fullCommand.IndexOf('"', 1);
                if (endingCommandPosition >= 1)
                {
                    string command = fullCommand.Substring(1, endingCommandPosition - 1);
                    string arguments = string.Empty;
                    if (fullCommand.Length > endingCommandPosition + 1)
                    {
                        arguments = fullCommand.Substring(endingCommandPosition + 2, fullCommand.Length - endingCommandPosition - 2);
                    }
                    SystemService.StartCommandLine(command, arguments);
                }
            }
            else
            {
                var endingCommandPosition = fullCommand.IndexOf(' ', 1);
                if (endingCommandPosition >= 0)
                {
                    string command = fullCommand.Substring(0, endingCommandPosition);
                    string arguments = string.Empty;
                    if (fullCommand.Length > endingCommandPosition + 1)
                    {
                        arguments = fullCommand.Substring(endingCommandPosition + 1, fullCommand.Length - endingCommandPosition - 1);
                    }
                    SystemService.StartCommandLine(command, arguments);
                }
            }
        }

        public string CreateDir(string repoPath, string value)
        {
            var directoryName = "{0}-{1}".FormatWith(SystemService.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), value.ToSlug("_"));
            string workspacePath = Path.Combine(repoPath, directoryName);

            SystemService.CreateDirectoryIfNotExists(workspacePath);
            OpenDir(workspacePath);

            return workspacePath;
        }

        public IEnumerable<string> GetWorspaces(string actualPath)
        {
            return SystemService.GetDirectories(actualPath).OrderBy(x => x);
        }

        public void Archive(string actualPath, string workspace)
        {
            string fullWorkspacePath = Path.Combine(actualPath, workspace);
            if (SystemService.DirectoryExists(fullWorkspacePath))
            {
                string archiveDirectory = Path.Combine(actualPath, "0__ARCHIVE__");
                string archivedWorkspace = Path.Combine(archiveDirectory, workspace);
                SystemService.CreateDirectoryIfNotExists(archiveDirectory);

                string archivedTargetWorkspace = archivedWorkspace;
                long count = 0;
                while (SystemService.DirectoryExists(archivedTargetWorkspace))
                {
                    count++;
                    archivedTargetWorkspace = "{0}-{1:N5}".FormatWith(archivedWorkspace, count);
                }

                SystemService.MoveDirectory(fullWorkspacePath, archivedTargetWorkspace);

                OpenDir(archivedTargetWorkspace);
            }
        }
    }
}