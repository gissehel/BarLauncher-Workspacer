using AllGreen.Lib;
using BarLauncher.Workspacer.Test.AllGreen.Fixture;
using BarLauncher.Workspacer.Test.AllGreen.Helper;

namespace BarLauncher.Workspacer.Test.AllGreen.Test
{
    public class Create_workspace_test : TestBase<WorkspacerContext>
    {
        public override void DoTest() =>
            StartTest()

            .IsRunnable()

            .Include<Prepare_common_context>()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Write_query(@"work create"))
            .EndUsing()

            .UsingList<Bar_launcher_results_fixture>()
            .With<Bar_launcher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check(@"work create data", @"Create a new workspace in the data repo")
            .Check(@"work create erk", @"Create a new workspace in the erk repo")
            .EndUsing()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Append__on_query(@" a"))
            .DoCheck(f => f.The_current_query_is(), "work create a")
            .EndUsing()

            .UsingList<Bar_launcher_results_fixture>()
            .With<Bar_launcher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check(@"work create data", @"Create a new workspace in the data repo")
            .EndUsing()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "work create data ")
            .EndUsing()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Append__on_query(@"log parsing"))
            .EndUsing()

             .UsingList<Bar_launcher_results_fixture>()
            .With<Bar_launcher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check(@"work create data log parsing", @"Create new workspace ""log parsing"" in repo data")
            .EndUsing()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Bar_launcher_is_displayed())
            .EndUsing()

            .Using<System_fixture>()
            .DoAccept(f => f.The_path__exists(@"C:\user\banta\data\2014-08-17-log_parsing"))
            .EndUsing()

            .UsingList<System_command_line_started_fixture>()
            .With<System_command_line_started_fixture.Result>(f => f.Command, f => f.Arguments)
            .Check(@"C:\user\banta\data\2014-07-12-task_1", "")
            .Check(@"C:\user\banta\data\2014-07-12-another_task", "")
            .Check(@"C:\user\banta\data\2014-07-19-new_task", "")
            .Check(@"C:\user\banta\data\2014-07-29-this_item_contains_the_word_shrubbery", "")
            .Check(@"C:\user\banta\data\2014-08-07-ruby", "")
            .Check(@"C:\user\banta\data\2014-08-11-this_contains_accents_like_eeac", "")
            .Check(@"C:\user\banta\data\2014-08-17-log_parsing", "")
            .EndUsing()

            .EndTest();
    }
}