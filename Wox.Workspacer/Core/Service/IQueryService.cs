using Wox.Plugin;
using Wox.Workspacer.DomainModel;

namespace Wox.Workspacer.Core.Service
{
    public interface IQueryService
    {
        WoxQuery GetWoxQuery(Query pluginQuery);
    }
}