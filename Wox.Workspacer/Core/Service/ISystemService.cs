using FluentDataAccess.Core.Service;
using System;
using System.Collections.Generic;

namespace Wox.Workspacer.Core.Service
{
    public interface IWorkspacerSystemService : IDataAccessConfigurationService
    {
        bool DirectoryExists(string path);

        void CreateDirectoryIfNotExists(string path);

        string GetExportPath();

        string GetUID();

        DateTime Now { get; }

        IEnumerable<string> GetDirectories(string name);

        void MoveDirectory(string source, string destination);
    }
}