using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Wox.Workspacer.Core.Service;

namespace Wox.Workspacer.Service
{
    public class DataAccessQueryWithResult<T> : IDataAccessQueryWithResult<T>
    {
        private DataAccessQuery DataAccessQuery { get; set; }

        private Func<T> EntityFactory { get; set; }

        private Dictionary<string, Action<SQLiteDataReader, int, T>> ReaderActionsByName { get; set; } = new Dictionary<string, Action<SQLiteDataReader, int, T>>();

        public DataAccessQueryWithResult(DataAccessQuery dataAccessQuery, Func<T> entityFactory)
        {
            DataAccessQuery = dataAccessQuery;
            EntityFactory = entityFactory;
        }

        public IEnumerable<T> Execute()
        {
            using (var command = DataAccessQuery.GetCommand())
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        T entity = EntityFactory();
                        foreach (var nameAndReaderAction in ReaderActionsByName)
                        {
                            var name = nameAndReaderAction.Key;
                            var readerAction = nameAndReaderAction.Value;
                            var position = reader.GetOrdinal(name);
                            readerAction(reader, position, entity);
                        }
                        yield return entity;
                    }
                }
            }
        }

        public IDataAccessQueryWithResult<T> Reading(string name, Action<T, string> actionForString)
        {
            ReaderActionsByName[name] = (reader, position, entity) => actionForString(entity, reader.GetString(position));
            return this;
        }

        public IDataAccessQueryWithResult<T> Reading(string name, Action<T, long> actionForLong)
        {
            ReaderActionsByName[name] = (reader, position, entity) => actionForLong(entity, reader.GetInt64(position));
            return this;
        }

        public IDataAccessQueryWithResult<T> Reading(string name, Action<T, bool> actionForBool)
        {
            ReaderActionsByName[name] = (reader, position, entity) => actionForBool(entity, reader.GetBoolean(position));
            return this;
        }
    }
}