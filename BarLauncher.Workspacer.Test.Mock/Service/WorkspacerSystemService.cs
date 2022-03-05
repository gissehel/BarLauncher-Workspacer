using System;
using System.Collections.Generic;
using BarLauncher.EasyHelper.Core.Service;
using BarLauncher.Workspacer.Lib.Core.Service;

namespace BarLauncher.Workspacer.Test.Mock.Service
{
    public class WorkspacerSystemServiceMock : IWorkspacerSystemService
    {
        public string ApplicationDataPath => SystemService.ApplicationDataPath;
        public string DatabasePath => ApplicationDataPath;


        public WorkspacerSystemServiceMock(ISystemService systemService)
        {
            SystemService = systemService;
        }

        public List<KeyValuePair<string, string>> CommandLineStarted { get; private set; } = new List<KeyValuePair<string, string>>();

        private DateTime? _now = null;

        public DateTime Now
        {
            get => _now ?? DateTime.Now;
            set => _now = value;
        }

        public string DatabaseName => "TestDB";

        public ISystemService SystemService { get; }

        public void StartCommandLine(string command, string arguments)
        {
            CommandLineStarted.Add(new KeyValuePair<string, string>(command, arguments));
        }

        public string GetExportPath() => @".\ExportDirectory";

        public string GetUID() => "UID";

        private Dictionary<string, bool> _directories = new Dictionary<string, bool>();

        public void CreateDirectoryIfNotExists(string path)
        {
            _directories[path] = true;
        }

        public bool DirectoryExists(string path) => _directories.ContainsKey(path) && _directories[path];

        public IEnumerable<string> GetDirectories(string name)
        {
            var parentDirectory = name;
            if (!parentDirectory.EndsWith("\\"))
            {
                parentDirectory += "\\";
            }
            foreach (var directory in _directories.Keys)
            {
                if (_directories[directory] && directory.ToLower().StartsWith(parentDirectory.ToLower()))
                {
                    var subDirectory = directory.Substring(parentDirectory.Length, directory.Length - parentDirectory.Length);
                    if (!subDirectory.Contains("\\"))
                    {
                        yield return subDirectory;
                    }
                }
            }
        }

        public void MoveDirectory(string source, string destination)
        {
            if (DirectoryExists(source))
            {
                if (!(DirectoryExists(destination)))
                {
                    _directories.Remove(source);
                    _directories[destination] = true;
                }
            }
        }
    }
}