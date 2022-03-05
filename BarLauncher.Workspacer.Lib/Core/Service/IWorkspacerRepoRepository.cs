using System.Collections.Generic;
using BarLauncher.Workspacer.Lib.DomainModel;

namespace BarLauncher.Workspacer.Lib.Core.Service
{
    public interface IWorkspacerRepoRepository
    {
        void Init();

        string GetPath(string name);

        void SetPath(string name, string path);

        IEnumerable<WorkspacerRepo> GetRepos();
    }
}