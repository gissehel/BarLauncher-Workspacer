using System.Collections.Generic;
using Wox.Workspacer.DomainModel;

namespace Wox.Workspacer.Core.Service
{
    public interface IWorkspacerRepoRepository
    {
        void Init();

        string GetPath(string name);

        void SetPath(string name, string path);

        IEnumerable<WorkspacerRepo> GetRepos();
    }
}