using System.Collections.Generic;
using Wox.Plugin;
using Wox.Workspacer.DomainModel;

namespace Wox.Workspacer.Core.Service
{
    public interface IResultService
    {
        List<Result> MapResults(IEnumerable<WoxResult> results);
    }
}