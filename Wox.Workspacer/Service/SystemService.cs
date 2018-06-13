using System;
using System.Diagnostics;
using System.IO;
using Wox.Workspacer.Core.Service;

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

        public string GetExportPath() => ApplicationDataPath;

        public string GetUID() => string.Format("{0:yyyyMMdd-HHmmss-fff}", DateTime.Now);

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
    }
}