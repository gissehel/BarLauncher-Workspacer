using System.Linq;
using Wox.Workspacer.DomainModel;

namespace Wox.Workspacer.Mock.Service
{
    public class QueryServiceMock
    {
        public WoxQuery GetWoxQuery(string internalQuery)
        {
            var elements = internalQuery.Trim(' ').Split(' ').Where(term => term.Length > 0).ToArray();
            var searchTerms = elements.Skip(1).ToArray();
            var rawQuery = string.Join(" ", elements);
            var search = string.Join(" ", searchTerms);

            return new WoxQuery
            {
                InternalQuery = internalQuery,
                RawQuery = rawQuery,
                Search = search,
                SearchTerms = searchTerms,
                Command = elements.Length > 0 ? elements[0] : string.Empty,
            };
        }
    }
}