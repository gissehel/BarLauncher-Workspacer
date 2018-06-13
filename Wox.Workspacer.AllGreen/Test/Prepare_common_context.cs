using AllGreen.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wox.Workspacer.AllGreen.Fixture;
using Wox.Workspacer.AllGreen.Helper;

namespace Wox.Workspacer.AllGreen.Test
{
    public class Prepare_common_context : TestBase<WorkspacerContext>
    {
        public override void DoTest() =>
            StartTest()

            .Using<Wox_bar_fixture>()
            .DoAction(f=>f.Start_the_bar())
            .DoAction(f=>f.Display_wox())
            .DoCheck(f=>f.The_current_query_is(), "")
            .EndUsing()

            .EndTest();
    }
}
