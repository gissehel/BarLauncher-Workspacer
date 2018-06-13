namespace Wox.Workspacer.Core.Service
{
    public interface ISystemService
    {
        void StartCommandLine(string command, string arguments);

        string ApplicationDataPath { get; }

        string GetExportPath();

        string GetUID();
    }
}