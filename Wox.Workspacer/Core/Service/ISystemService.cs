using FluentDataAccess.Core.Service;
using System;
using System.Collections.Generic;

namespace Wox.Workspacer.Core.Service
{
    public interface ISystemService : IDataAccessConfigurationService
    {
        void StartCommandLine(string command, string arguments);

        // string ApplicationDataPath { get; }

        bool DirectoryExists(string path);

        void CreateDirectoryIfNotExists(string path);

        string GetExportPath();

        string GetUID();

        DateTime Now { get; }

        IEnumerable<string> GetDirectories(string name);

        void MoveDirectory(string source, string destination);
    }
}