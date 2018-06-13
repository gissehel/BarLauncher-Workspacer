using System.Collections.Generic;
using Wox.Workspacer.Core.Service;
using Wox.Workspacer.DomainModel;

namespace Wox.Workspacer.Mock.Service
{
    public class WoxContextServiceMock : IWoxContextService
    {
        private QueryServiceMock QueryService { get; set; }

        private Dictionary<string, IWoxResultFinder> WoxResultFinderByCommandName { get; set; } = new Dictionary<string, IWoxResultFinder>();

        public WoxContextServiceMock(QueryServiceMock queryService)
        {
            QueryService = queryService;
        }

        public void AddQueryFetcher(string commandName, IWoxResultFinder queryFetcher)
        {
            WoxResultFinderByCommandName[commandName] = queryFetcher;
        }

        public string ActionKeyword { get; set; }

        public string Seperater => " ";

        public string CurrentQuery { get; set; } = "";

        public void ChangeQuery(string query)
        {
            SetCurrentQuery(query);
        }

        public void SetQueryFromInterface(string query)
        {
            SetCurrentQuery(query);
        }

        private void SetCurrentQuery(string query)
        {
            CurrentQuery = query;
            var woxQuery = QueryService.GetWoxQuery(CurrentQuery);
            ActionKeyword = woxQuery.Command;
            if (WoxResultFinderByCommandName.ContainsKey(woxQuery.Command))
            {
                Results = WoxResultFinderByCommandName[woxQuery.Command].GetResults(woxQuery);
            }
            else
            {
                Results = new List<WoxResult>();
            }
        }

        public IEnumerable<WoxResult> Results { get; set; } = new List<WoxResult>();

        public bool WoxDisplayed { get; set; } = false;
    }
}