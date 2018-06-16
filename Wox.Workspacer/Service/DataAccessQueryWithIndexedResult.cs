using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Wox.Workspacer.Core.Service;

namespace Wox.Workspacer.Service
{
    internal class DataAccessQueryWithIndexedResult : IDataAccessQueryWithIndexedResult
    {
        private DataAccessQuery DataAccessQuery { get; set; }

        private Dictionary<string, Action<SQLiteDataReader, int, int>> ReaderActionsByName { get; set; } = new Dictionary<string, Action<SQLiteDataReader, int, int>>();

        public DataAccessQueryWithIndexedResult(DataAccessQuery dataAccessQuery)
        {
            DataAccessQuery = dataAccessQuery;
        }

        public int Execute()
        {
            using (var command = DataAccessQuery.GetCommand())
            {
                using (var reader = command.ExecuteReader())
                {
                    int index = 0;
                    while (reader.Read())
                    {
                        foreach (var nameAndReaderAction in ReaderActionsByName)
                        {
                            var name = nameAndReaderAction.Key;
                            var readerAction = nameAndReaderAction.Value;
                            var position = reader.GetOrdinal(name);
                            readerAction(reader, position, index);
                        }
                        index++;
                    }
                    return index;
                }
            }
        }

        public IDataAccessQueryWithIndexedResult Reading(string name, Action<int, string> actionForString)
        {
            ReaderActionsByName[name] = (reader, position, index) => actionForString(index, reader.GetString(position));
            return this;
        }

        public IDataAccessQueryWithIndexedResult Reading(string name, Action<int, long> actionForLong)
        {
            ReaderActionsByName[name] = (reader, position, index) => actionForLong(index, reader.GetInt64(position));
            return this;
        }

        public IDataAccessQueryWithIndexedResult Reading(string name, Action<int, bool> actionForBool)
        {
            ReaderActionsByName[name] = (reader, position, index) => actionForBool(index, reader.GetBoolean(position));
            return this;
        }
    }
}