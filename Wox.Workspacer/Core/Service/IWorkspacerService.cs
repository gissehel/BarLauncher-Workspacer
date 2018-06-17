using System.Collections.Generic;
using Wox.Workspacer.DomainModel;

namespace Wox.Workspacer.Core.Service
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
    }
}