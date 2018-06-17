using System;
using System.Collections.Generic;

namespace Wox.Workspacer.Core.Service
{
    public interface ISystemService
    {
        void StartCommandLine(string command, string arguments);

        string ApplicationDataPath { get; }

        void CreateDirectoryIfNotExists(string path);

        string GetExportPath();

        string GetUID();

        DateTime Now { get; }

        IEnumerable<string> GetDirectories(string name);
    }
}