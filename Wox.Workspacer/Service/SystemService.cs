using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Wox.Workspacer.Core.Service;
using Wox.Workspacer.Tool;

namespace Wox.Workspacer.Service
{
    public class SystemService : ISystemService
    {
        private string ApplicationName { get; set; }
        private string _applicationDataPath = null;

        public SystemService(string applicationName)
        {
            ApplicationName = applicationName;
        }

        public string ApplicationDataPath => _applicationDataPath ?? (_applicationDataPath = GetApplicationDataPath());

        public DateTime Now => DateTime.Now;

        public string GetExportPath() => ApplicationDataPath;

        public string GetUID() => "{0:yyyyMMdd-HHmmss-fff}".FormatWith(DateTime.Now);

        private string GetApplicationDataPath()
        {
            var appDataPathParent = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appDataPath = Path.Combine(appDataPathParent, ApplicationName);
            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }
            return appDataPath;
        }

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
    }
}