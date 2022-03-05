using AllGreen.Lib;
using BarLauncher.Workspacer.Test.AllGreen.Fixture;
using BarLauncher.Workspacer.Test.AllGreen.Helper;

namespace BarLauncher.Workspacer.Test.AllGreen.Test
{
    public class Create_and_view_repo_test : TestBase<WorkspacerContext>
    {
        public override void DoTest() =>
            StartTest()

            .IsRunnable()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Start_the_bar())
            .DoAction(f => f.Display_bar_launcher())
            .DoCheck(f => f.The_current_query_is(), "")
            .EndUsing()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Write_query("work list"))
            .EndUsing()

            .UsingList<Bar_launcher_results_fixture>()
            .With<Bar_launcher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .EndUsing()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Write_query("work name"))
            .EndUsing()

            .UsingList<Bar_launcher_results_fixture>()
            .With<Bar_launcher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .EndUsing()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Write_query("work name data"))
            .EndUsing()

            .UsingList<Bar_launcher_results_fixture>()
            .With<Bar_launcher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("work name data", "There is no repo named data yet")
            .EndUsing()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Write_query(@"work name data C:\user\banta\data"))
            .EndUsing()

            .UsingList<Bar_launcher_results_fixture>()
            .With<Bar_launcher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check(@"work name data C:\user\banta\data", @"Set repo name data to path C:\user\banta\data")
            .EndUsing()

            .Using<System_fixture>()
            .DoReject(f => f.The_path__exists(@"C:\user\banta\data"))
            .EndUsing()

            .UsingList<System_command_line_started_fixture>()
            .With<System_command_line_started_fixture.Result>(f => f.Command)
            .EndUsing()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Bar_launcher_is_displayed())
            .EndUsing()

            .Using<System_fixture>()
            .DoAccept(f => f.The_path__exists(@"C:\user\banta\data"))
            .EndUsing()

            .UsingList<System_command_line_started_fixture>()
            .With<System_command_line_started_fixture.Result>(f => f.Command)
            .EndUsing()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query(@"work li"))
            .EndUsing()

            .UsingList<Bar_launcher_results_fixture>()
            .With<Bar_launcher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("work list", "List all the available repositories")
            .EndUsing()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "work list ")
            .EndUsing()

            .UsingList<Bar_launcher_results_fixture>()
            .With<Bar_launcher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("work list data", @"Go to C:\user\banta\data")
            .EndUsing()

            .UsingList<System_command_line_started_fixture>()
            .With<System_command_line_started_fixture.Result>(f => f.Command)
            .EndUsing()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Bar_launcher_is_displayed())
            .EndUsing()

            .UsingList<System_command_line_started_fixture>()
            .With<System_command_line_started_fixture.Result>(f => f.Command)
            .Check(@"C:\user\banta\data")
            .EndUsing()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query(@"work name erk C:\Storage\With a space\erk"))
            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Bar_launcher_is_displayed())
            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query(@"work list"))
            .EndUsing()

            .UsingList<Bar_launcher_results_fixture>()
            .With<Bar_launcher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("work list data", @"Go to C:\user\banta\data")
            .Check("work list erk", @"Go to C:\Storage\With a space\erk")
            .EndUsing()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Select_line(2))
            .DoReject(f => f.Bar_launcher_is_displayed())
            .EndUsing()

            .UsingList<System_command_line_started_fixture>()
            .With<System_command_line_started_fixture.Result>(f => f.Command)
            .Check(@"C:\user\banta\data")
            .Check(@"C:\Storage\With a space\erk")
            .EndUsing()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query("work config OpenDirCommand=\"C:\\Program Files\\TotalCommander\\TOTALCMD64.EXE\" /T /R=\"%1\""))
            .DoAction(f => f.Select_line(1))
            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query(@"work list"))
            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Bar_launcher_is_displayed())
            .EndUsing()

            .UsingList<System_command_line_started_fixture>()
            .With<System_command_line_started_fixture.Result>(f => f.Command, f => f.Arguments)
            .Check(@"C:\user\banta\data", "")
            .Check(@"C:\Storage\With a space\erk", "")
            .Check(@"C:\Program Files\TotalCommander\TOTALCMD64.EXE", "/T /R=\"C:\\user\\banta\\data\"")
            .EndUsing()

            .EndTest();
    }
}