using System.Data.SQLite;
using Wox.Workspacer.Core.Service;

namespace Wox.Workspacer.Service
{
    public class DataAccessService : IDataAccessService

    {
        private ISystemService SystemService { get; set; }
        private string DataPath { get; set; } = ".";
        private SQLiteConnection SQLiteConnection { get; set; }

        public DataAccessService(ISystemService systemService)
        {
            SystemService = systemService;
            SQLiteConnection = null;
        }

        public void Init()
        {
            DataPath = SystemService.ApplicationDataPath;
            SQLiteConnection = new SQLiteConnection(string.Format(@"Data Source={0}\Workspacer.sqlite;Version=3;", DataPath));
            SQLiteConnection.Open();
        }

        public void Dispose()
        {
            SQLiteConnection.Close();
        }

        public IDataAccessQuery GetQuery(string query)
        {
            return new DataAccessQuery(SQLiteConnection, query);
        }
    }
}