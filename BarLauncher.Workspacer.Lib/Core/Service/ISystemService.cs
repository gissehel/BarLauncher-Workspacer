using FluentDataAccess;
using System;
using System.Collections.Generic;

namespace BarLauncher.Workspacer.Lib.Core.Service
{
    public interface IWorkspacerSystemService : IDataAccessConfigurationByPath
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