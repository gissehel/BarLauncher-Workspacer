using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Wox.Workspacer.Core.Service;

namespace Wox.Workspacer.Service
{
    public class DataAccessQuery : IDataAccessQuery
    {
        public SQLiteConnection SQLiteConnection { get; set; }
        public string Query { get; set; }

        private List<Action<SQLiteParameterCollection>> ParameterActions { get; set; } = new List<Action<SQLiteParameterCollection>>();

        public DataAccessQuery(SQLiteConnection sqliteConnection, string query)
        {
            SQLiteConnection = sqliteConnection;
            Query = query;
        }

        public int Execute()
        {
            using (var command = GetCommand())
            {
                return command.ExecuteNonQuery();
            }
        }

        public IDataAccessQueryWithResult<T> Returning<T>() where T : new()
        {
            return new DataAccessQueryWithResult<T>(this, () => new T());
        }

        public IDataAccessQueryWithResult<T> Returning<T>(Func<T> entityFactory)
        {
            return new DataAccessQueryWithResult<T>(this, entityFactory);
        }

        public IDataAccessQueryWithIndexedResult ReturningWithIndex()
        {
            return new DataAccessQueryWithIndexedResult(this);
        }

        public IDataAccessQuery WithParameter(string name, string value)
        {
            ParameterActions.Add(p => p.Add("@" + name, System.Data.DbType.String).Value = value);
            return this;
        }

        public IDataAccessQuery WithParameter(string name, long value)
        {
            ParameterActions.Add(p => p.Add("@" + name, System.Data.DbType.Int64).Value = value);
            return this;
        }

        public IDataAccessQuery WithParameter(string name, bool value)
        {
            ParameterActions.Add(p => p.Add("@" + name, System.Data.DbType.Boolean).Value = value);
            return this;
        }

        public SQLiteCommand GetCommand()
        {
            var command = new SQLiteCommand(Query, SQLiteConnection);
            foreach (var parameterAction in ParameterActions)
            {
                parameterAction(command.Parameters);
            }
            return command;
        }
    }
}