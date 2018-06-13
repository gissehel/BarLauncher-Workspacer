using Wox.Plugin;
using Wox.Workspacer.Core.Service;
using Wox.Workspacer.DomainModel;

namespace Wox.Workspacer.Service
{
    public class QueryService : IQueryService
    {
        public WoxQuery GetWoxQuery(Query pluginQuery)
        {
            var searchTerms = pluginQuery.Search.Split(' ');
            return new WoxQuery
            {
                InternalQuery = pluginQuery,
                RawQuery = pluginQuery.RawQuery,
                Search = pluginQuery.Search,
                SearchTerms = searchTerms,
                Command = pluginQuery.RawQuery.Split(' ')[0],
            };
        }
    }
}