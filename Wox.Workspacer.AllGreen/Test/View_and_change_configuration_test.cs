using AllGreen.Lib;
using Wox.Workspacer.AllGreen.Fixture;
using Wox.Workspacer.AllGreen.Helper;

namespace Wox.Workspacer.AllGreen.Test
{
    public class View_and_change_configuration_test : TestBase<WorkspacerContext>
    {
        public override void DoTest() =>
            StartTest()

            .IsRunnable()

            .Include<Prepare_common_context>()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Display_wox())
            .DoAction(f => f.Write_query("work con"))
            .DoCheck(f => f.The_current_query_is(), "work con")
            .EndUsing()

            .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("work config KEY VALUE", "View/Change workspacer configuration")
            .EndUsing()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .DoCheck(f => f.The_current_query_is(), "work config ")
            .EndUsing()

            .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("work config OpenDirCommand", "OpenDirCommand=\"%1\"")
            .EndUsing()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Append__on_query("k"))
            .DoCheck(f => f.The_current_query_is(), "work config k")
            .EndUsing()

            .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .EndUsing()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Write_query("work config irc"))
            .DoCheck(f => f.The_current_query_is(), "work config irc")
            .EndUsing()

            .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("work config OpenDirCommand", "OpenDirCommand=\"%1\"")
            .EndUsing()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Write_query("work config OpenDirComman"))
            .DoCheck(f => f.The_current_query_is(), "work config OpenDirComman")
            .EndUsing()

            .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("work config OpenDirCommand", "OpenDirCommand=\"%1\"")
            .EndUsing()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Append__on_query("d"))
            .DoCheck(f => f.The_current_query_is(), "work config OpenDirCommand")
            .EndUsing()

            .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("work config OpenDirCommand", "Select OpenDirCommand=\"%1\"")
            .EndUsing()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Wox_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "work config OpenDirCommand=\"%1\"")
            .EndUsing()

            .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("work config OpenDirCommand", "The current value for OpenDirCommand is \"%1\"")
            .EndUsing()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Wox_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "work config OpenDirCommand=\"%1\"")
            .DoAction(f => f.Write_query("work config OpenDirCommand=\"C:\\Program Files\\TotalCommander\\TOTALCMD64.EXE\" /T /R=\"%1\""))
            .EndUsing()

            .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("work config OpenDirCommand", "Set the value for OpenDirCommand to \"C:\\Program Files\\TotalCommander\\TOTALCMD64.EXE\" /T /R=\"%1\"")
            .EndUsing()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Write_query("work config OpenDirComman"))
            .DoCheck(f => f.The_current_query_is(), "work config OpenDirComman")
            .EndUsing()

            .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("work config OpenDirCommand", "OpenDirCommand=\"C:\\Program Files\\TotalCommander\\TOTALCMD64.EXE\" /T /R=\"%1\"")
            .EndUsing()

            .EndTest();
    }
}