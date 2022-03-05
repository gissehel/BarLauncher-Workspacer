using FluentDataAccess;
using System.Collections.Generic;
using BarLauncher.Workspacer.Lib.Core.Service;
using BarLauncher.Workspacer.Lib.DomainModel;

namespace BarLauncher.Workspacer.Lib.Service
{
    public class WorkspacerRepoRepository : IWorkspacerRepoRepository
    {
        private IDataAccessService DataAccessService { get; set; }

        public WorkspacerRepoRepository(IDataAccessService dataAccessService)
        {
            DataAccessService = dataAccessService;
        }

        public void Init() =>
            DataAccessService
            .GetQuery("create table if not exists repo (id integer primary key, name text, path text, constraint name_unique unique (name));")
            .Execute();

        public string GetPath(string name)
        {
            string path = null;
            DataAccessService
                .GetQuery("select path from repo where name=@name")
                .WithParameter("name", name)
                .ReturningWithIndex()
                .Reading("path", (index, value) => path = value)
                .Execute()
                ;
            return path;
        }

        public void SetPath(string name, string path) =>
            DataAccessService
            .GetQuery("insert or replace into repo (name, path) values (@name, @path)")
            .WithParameter("name", name)
            .WithParameter("path", path)
            .Execute()
            ;

        public IEnumerable<WorkspacerRepo> GetRepos() =>
            DataAccessService
            .GetQuery("select name, path from repo order by name")
            .Returning<WorkspacerRepo>()
            .Reading("name", (repo, value) => repo.Name = value)
            .Reading("path", (repo, value) => repo.Path = value)
            .Execute()
            ;
    }
}