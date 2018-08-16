using FluentDataAccess.Core.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Wox.EasyHelper;
using Wox.EasyHelper.Core.Service;
using Wox.Workspacer.Core.Service;

namespace Wox.Workspacer.Service
{
    public class WorkspacerSystemService : IWorkspacerSystemService
    {
        private string ApplicationName => SystemService.ApplicationName;
        private string ApplicationDataPath => SystemService.ApplicationDataPath;

        public WorkspacerSystemService(ISystemService systemService)
        {
            SystemService = systemService;
        }

        public DateTime Now => DateTime.Now;

        public string DatabaseName => ApplicationName;

        public ISystemService SystemService { get; }

        string IDataAccessConfigurationService.ApplicationDataPath => SystemService.ApplicationDataPath;

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
                if (!(DirectoryExists(destination)))
                {
                    Directory.Move(source, destination);
                }
            }
        }
    }
}