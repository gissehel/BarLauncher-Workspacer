using FluentDataAccess;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BarLauncher.EasyHelper;
using BarLauncher.EasyHelper.Core.Service;
using BarLauncher.Workspacer.Lib.Core.Service;

namespace BarLauncher.Workspacer.Lib.Service
{
    public class WorkspacerSystemService : IWorkspacerSystemService
    {
        private string ApplicationName => SystemService.ApplicationName;
        private string ApplicationDataPath => SystemService.ApplicationDataPath;

        public WorkspacerSystemService(ISystemService systemService)
        {
            SystemService = systemService;
        }

        private string GetDatabaseName(string applicationDataPath, string applicationName) => Path.Combine(applicationDataPath, applicationName + ".sqlite");

        public WorkspacerSystemService(ISystemService systemService, params string[] oldApplicationNames)
        {
            SystemService = systemService;
            var currentDatabaseName = GetDatabaseName(ApplicationDataPath, ApplicationName);

            if (!File.Exists(currentDatabaseName))
            {
                foreach (var oldApplicationName in oldApplicationNames.AsEnumerable().Reverse())
                {
                    string oldDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), oldApplicationName);

                    var oldDatabaseName = GetDatabaseName(oldDataPath, oldApplicationName);
                    if (File.Exists(oldDatabaseName))
                    {
                        File.Move(oldDatabaseName, currentDatabaseName);
                        return;
                    }
                }
            }
        }

        public DateTime Now => DateTime.Now;

        public string DatabaseName => ApplicationName;

        public ISystemService SystemService { get; }

        public string DatabasePath => ApplicationDataPath;

        public string GetExportPath() => ApplicationDataPath;

        public string GetUID() => "{0:yyyyMMdd-HHmmss-fff}".FormatWith(DateTime.Now);

        public void StartCommandLine(string command, string arguments)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = arguments,
                    UseShellExecute = true,
                    RedirectStandardOutput = false,
                    CreateNoWindow = false,
                    WorkingDirectory = ApplicationDataPath
                }
            };

            try
            {
                proc.Start();
            }
            catch (Exception)
            {
                // TODO : Find something usefull here...
            }
        }

        public bool DirectoryExists(string path) => Directory.Exists(path);

        public void CreateDirectoryIfNotExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public IEnumerable<string> GetDirectories(string name)
        {
            var directories = Directory.GetDirectories(name);
            var length = name.Length;
            if (!name.EndsWith("\\"))
            {
                length += 1;
            }
            foreach (var directory in directories.OrderBy(x => x))
            {
                yield return directory.Substring(length);
            }
        }

        public void MoveDirectory(string source, string destination)
        {
            if (DirectoryExists(source))
            {
                if (!DirectoryExists(destination))
                {
                    Directory.Move(source, destination);
                }
            }
        }
    }
}