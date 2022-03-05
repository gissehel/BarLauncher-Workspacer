using AllGreen.Lib;
using System.Collections.Generic;
using System.Linq;
using BarLauncher.Workspacer.Test.AllGreen.Helper;

namespace BarLauncher.Workspacer.Test.AllGreen.Fixture
{
    public class Command_line_started_fixture : FixtureBase<WorkspacerContext>
    {
        public class Result
        {
            public string Command { get; set; }
            public string Arguments { get; set; }
        }

        public override IEnumerable<object> OnQuery()
        {
            var result = Context
                .ApplicationStarter
                .SystemService
                .CommandLineStarted
                .Select(cl => new Result
                {
                    Command = cl.Command,
                    Arguments = cl.Arguments
                }).ToList();
            return result.Cast<object>();
        }
    }
}