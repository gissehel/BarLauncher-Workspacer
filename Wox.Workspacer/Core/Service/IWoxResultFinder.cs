using System.Collections.Generic;
using Wox.Workspacer.DomainModel;

namespace Wox.Workspacer.Core.Service
{
    public interface IWoxResultFinder
    {
        IEnumerable<WoxResult> GetResults(WoxQuery query);
    }
}