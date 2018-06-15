using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wox.Plugin;
using Wox.Workspacer.Core.Service;
using Wox.Workspacer.Service;

namespace Wox.Workspacer
{
    public class WorkspacerPlugin : IPlugin
    {
        private IQueryService QueryService { get; set; }
        private IResultService ResultService { get; set; }

        private IWoxResultFinder WorkspacerResultFinder { get; set; }

        public void Init(PluginInitContext context)
        {
            IWoxContextService woxContextService = new WoxContextService(context);
            QueryService = new QueryService();
            ResultService = new ResultService();
            var workspacerService = new WorkspacerService();
            WorkspacerResultFinder = new WorkspacerResultFinder(woxContextService, workspacerService);
        }

        public List<Result> Query(Query query)
        {
            var woxQuery = QueryService.GetWoxQuery(query);
            var results = WorkspacerResultFinder.GetResults(woxQuery);
            return ResultService.MapResults(results);
        }
    }
}