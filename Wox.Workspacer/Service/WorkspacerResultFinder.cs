using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wox.Workspacer.Core.Service;
using Wox.Workspacer.DomainModel;

namespace Wox.Workspacer.Service
{
    public class WorkspacerResultFinder : IWoxResultFinder
    {
        private IWoxContextService WoxContextService { get; set; }

        public IEnumerable<WoxResult> GetResults(WoxQuery query)
        {
            return new List<WoxResult> { new WoxResult { Title = "Dummy", SubTitle = "Dummy" } };
        }

        public WorkspacerResultFinder(IWoxContextService woxContextService)
        {
            WoxContextService = woxContextService;
        }
    }
}