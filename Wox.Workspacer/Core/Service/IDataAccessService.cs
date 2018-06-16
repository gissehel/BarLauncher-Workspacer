using System;
using System.Collections.Generic;

namespace Wox.Workspacer.Core.Service
{
    public interface IDataAccessService : IDisposable
    {
        void Init();

        IDataAccessQuery GetQuery(string query);
    }

    public interface IDataAccessQuery
    {
        IDataAccessQuery WithParameter(string name, string value);

        IDataAccessQuery WithParameter(string name, long value);

        IDataAccessQuery WithParameter(string name, bool value);

        IDataAccessQueryWithResult<T> Returning<T>() where T : new();

        IDataAccessQueryWithResult<T> Returning<T>(Func<T> entityFactory);

        IDataAccessQueryWithIndexedResult ReturningWithIndex();

        int Execute();
    }

    public interface IDataAccessQueryWithIndexedResult
    {
        IDataAccessQueryWithIndexedResult Reading(string name, Action<int, string> actionForString);

        IDataAccessQueryWithIndexedResult Reading(string name, Action<int, long> actionForLong);

        IDataAccessQueryWithIndexedResult Reading(string name, Action<int, bool> actionForBool);

        int Execute();
    }

    public interface IDataAccessQueryWithResult<T>
    {
        IDataAccessQueryWithResult<T> Reading(string name, Action<T, string> actionForString);

        IDataAccessQueryWithResult<T> Reading(string name, Action<T, long> actionForLong);

        IDataAccessQueryWithResult<T> Reading(string name, Action<T, bool> actionForBool);

        IEnumerable<T> Execute();
    }
}