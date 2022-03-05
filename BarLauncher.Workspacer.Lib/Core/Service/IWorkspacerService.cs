using System.Collections.Generic;
using BarLauncher.Workspacer.Lib.DomainModel;

namespace BarLauncher.Workspacer.Lib.Core.Service
{
    public interface IWorkspacerService
    {
        void Init();

        WorkspacerConfiguration GetConfiguration();

        void SaveConfiguration(WorkspacerConfiguration configuration);

        List<WorkspacerRepo> GetRepos();

        string GetPathByName(string name);

        void SetPathByName(string name, string path);

        void OpenDir(string path);

        string CreateDir(string repoPath, string value);

        IEnumerable<string> GetWorspaces(string actualPath);

        void Archive(string actualPath, string workspace);

        void Dispose();
    }
}