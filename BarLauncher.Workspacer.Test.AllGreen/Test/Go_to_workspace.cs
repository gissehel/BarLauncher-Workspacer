using AllGreen.Lib;
using BarLauncher.Workspacer.Test.AllGreen.Fixture;
using BarLauncher.Workspacer.Test.AllGreen.Helper;

namespace BarLauncher.Workspacer.Test.AllGreen.Test
{
    public class Go_to_workspace : TestBase<WorkspacerContext>
    {
        public override void DoTest() =>
            StartTest()

            .IsRunnable()

            .Include<Prepare_common_context>()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query("work d"))
            .EndUsing()

            .UsingList<Bar_launcher_results_fixture>()
            .With<Bar_launcher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("work cd NAME [PATTERN] [PATTERN]", "Change to a workspace directory")
            .EndUsing()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .DoCheck(f => f.The_current_query_is(), "work cd ")
            .EndUsing()

            .UsingList<Bar_launcher_results_fixture>()
            .With<Bar_launcher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("work cd data [PATTERN] [PATTERN]", "Search the data repo")
            .Check("work cd erk [PATTERN] [PATTERN]", "Search the erk repo")
            .EndUsing()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Append__on_query("a"))
            .DoCheck(f => f.The_current_query_is(), "work cd a")
            .EndUsing()

            .UsingList<Bar_launcher_results_fixture>()
            .With<Bar_launcher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("work cd data [PATTERN] [PATTERN]", "Search the data repo")
            .EndUsing()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .DoCheck(f => f.The_current_query_is(), "work cd data ")
            .EndUsing()

            .UsingList<Bar_launcher_results_fixture>()
            .With<Bar_launcher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("work cd data 2014-07-12-another_task", "Go to 2014-07-12-another_task")
            .Check("work cd data 2014-07-12-task_1", "Go to 2014-07-12-task_1")
            .Check("work cd data 2014-07-19-new_task", "Go to 2014-07-19-new_task")
            .Check("work cd data 2014-07-29-this_item_contains_the_word_shrubbery", "Go to 2014-07-29-this_item_contains_the_word_shrubbery")
            .Check("work cd data 2014-08-07-ruby", "Go to 2014-08-07-ruby")
            .Check("work cd data 2014-08-11-this_contains_accents_like_eeac", "Go to 2014-08-11-this_contains_accents_like_eeac")
            .EndUsing()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Append__on_query("rub"))
            .DoCheck(f => f.The_current_query_is(), "work cd data rub")
            .EndUsing()

            .UsingList<Bar_launcher_results_fixture>()
            .With<Bar_launcher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("work cd data 2014-07-29-this_item_contains_the_word_shrubbery", "Go to 2014-07-29-this_item_contains_the_word_shrubbery")
            .Check("work cd data 2014-08-07-ruby", "Go to 2014-08-07-ruby")
            .EndUsing()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Append__on_query(" ord"))
            .DoCheck(f => f.The_current_query_is(), "work cd data rub ord")
            .EndUsing()

            .UsingList<Bar_launcher_results_fixture>()
            .With<Bar_launcher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("work cd data 2014-07-29-this_item_contains_the_word_shrubbery", "Go to 2014-07-29-this_item_contains_the_word_shrubbery")
            .EndUsing()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Bar_launcher_is_displayed())
            .EndUsing()

            .UsingList<System_command_line_started_fixture>()
            .With<System_command_line_started_fixture.Result>(f => f.Command, f => f.Arguments)
            .Check(@"C:\user\banta\data\2014-07-12-task_1", "")
            .Check(@"C:\user\banta\data\2014-07-12-another_task", "")
            .Check(@"C:\user\banta\data\2014-07-19-new_task", "")
            .Check(@"C:\user\banta\data\2014-07-29-this_item_contains_the_word_shrubbery", "")
            .Check(@"C:\user\banta\data\2014-08-07-ruby", "")
            .Check(@"C:\user\banta\data\2014-08-11-this_contains_accents_like_eeac", "")
            .Check(@"C:\user\banta\data\2014-07-29-this_item_contains_the_word_shrubbery", "")
            .EndUsing()

            .EndTest();
    }
}