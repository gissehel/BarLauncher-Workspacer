using AllGreen.Lib;
using Wox.Workspacer.AllGreen.Fixture;
using Wox.Workspacer.AllGreen.Helper;

namespace Wox.Workspacer.AllGreen.Test
{
    public class Prepare_common_context : TestBase<WorkspacerContext>
    {
        public override void DoTest() =>
            StartTest()

            .Using<Wox_bar_fixture>()

            .DoAction(f => f.Start_the_bar())
            .DoAction(f => f.Display_wox())
            .DoCheck(f => f.The_current_query_is(), "")

            .DoAction(f => f.Write_query(@"work name erk C:\Storage\With a space\erk"))
            .DoAction(f => f.Select_line(1))
            .DoAction(f => f.Display_wox())

            .DoAction(f => f.Write_query(@"work name data C:\user\banta\data"))
            .DoAction(f => f.Select_line(1))
            .DoAction(f => f.Display_wox())

            .EndUsing()

            .Using<System_fixture>()
            .DoAction(f => f.The_current_date_time_is("07/12/2014 18:11:47"))
            .EndUsing()

            .Using<Wox_bar_fixture>()

            .DoAction(f => f.Write_query(@"work cr data task 1"))
            .DoAction(f => f.Select_line(1))
            .DoAction(f => f.Display_wox())

            .EndUsing()

            .Using<Wox_bar_fixture>()

            .DoAction(f => f.Write_query(@"work cr data another task"))
            .DoAction(f => f.Select_line(1))
            .DoAction(f => f.Display_wox())

            .EndUsing()

            .Using<System_fixture>()
            .DoAction(f => f.The_current_date_time_is("07/19/2014 18:11:47"))
            .EndUsing()

            .Using<Wox_bar_fixture>()

            .DoAction(f => f.Write_query(@"work cr data new task"))
            .DoAction(f => f.Select_line(1))
            .DoAction(f => f.Display_wox())

            .EndUsing()

            .Using<System_fixture>()
            .DoAction(f => f.The_current_date_time_is("07/29/2014 18:11:47"))
            .EndUsing()

            .Using<Wox_bar_fixture>()

            .DoAction(f => f.Write_query(@"work cr data this item contains the word shrubbery"))
            .DoAction(f => f.Select_line(1))
            .DoAction(f => f.Display_wox())

            .EndUsing()

            .Using<System_fixture>()
            .DoAction(f => f.The_current_date_time_is("08/07/2014 18:11:47"))
            .EndUsing()

            .Using<Wox_bar_fixture>()

            .DoAction(f => f.Write_query(@"work cr data ruby"))
            .DoAction(f => f.Select_line(1))
            .DoAction(f => f.Display_wox())

            .EndUsing()

            .Using<System_fixture>()
            .DoAction(f => f.The_current_date_time_is("08/11/2014 18:11:47"))
            .EndUsing()

            .Using<Wox_bar_fixture>()

            .DoAction(f => f.Write_query(@"work cr data this contains accents like éèàµç"))
            .DoAction(f => f.Select_line(1))
            .DoAction(f => f.Display_wox())

            .EndUsing()

            .Using<System_fixture>()
            .DoAction(f => f.The_current_date_time_is("08/17/2014 18:11:47"))
            .EndUsing()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Write_query(""))
            .EndUsing()

            .EndTest();
    }
}