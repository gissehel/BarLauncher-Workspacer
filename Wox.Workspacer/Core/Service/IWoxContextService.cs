namespace Wox.Workspacer.Core.Service
{
    public interface IWoxContextService
    {
        void ChangeQuery(string query);

        string ActionKeyword { get; }

        string Seperater { get; }
    }
}