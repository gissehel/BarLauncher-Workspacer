using AllGreen.Lib;
using System.Collections.Generic;
using Wox.Workspacer.AllGreen.Helper;

namespace Wox.Workspacer.AllGreen.Fixture
{
    public class Wox_results_fixture : FixtureBase<WorkspacerContext>
    {
        public class Result
        {
            public string Title { get; set; }

            public string SubTitle { get; set; }
        }

        public override IEnumerable<object> OnQuery()
        {
            foreach (var result in Context.ApplicationStarter.WoxContextService.Results)
            {
                yield return new Result
                {
                    Title = result.Title,
                    SubTitle = result.SubTitle,
                };
            }
        }
    }
}