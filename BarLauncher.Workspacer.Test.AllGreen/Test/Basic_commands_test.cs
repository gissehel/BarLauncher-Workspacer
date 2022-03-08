using AllGreen.Lib;
using BarLauncher.Workspacer.Test.AllGreen.Fixture;
using BarLauncher.Workspacer.Test.AllGreen.Helper;

namespace BarLauncher.Workspacer.Test.AllGreen.Test
{
    public class Basic_commands_test : TestBase<WorkspacerContext>
    {
        public override void DoTest() =>
            StartTest()

            .IsRunnable()

            .Include<Prepare_common_context>()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query("work"))
            .DoCheck(f => f.The_current_query_is(), "work")
            .EndUsing()

            .UsingList<Bar_launcher_results_fixture>()
            .With<Bar_launcher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("work create NAME TITLE", "Create a new workspace directory in the repository NAME")
            .Check("work cd NAME [PATTERN] [PATTERN]", "Change to a workspace directory")
            .Check("work archive NAME [PATTERN] [PATTERN]", "Archive a workspace directory")
            .Check("work name NAME DIRECTORY", "Name a new repository")
            .Check("work list", "List all the available repositories")
            .Check("work config KEY VALUE", "View/Change workspacer configuration")
            .EndUsing()

            .Using<Bar_launcher_bar_fixture>()
            .DoAction(f => f.Append__on_query(" a"))
            .DoCheck(f => f.The_current_query_is(), "work a")
            .EndUsing()

            .UsingList<Bar_launcher_results_fixture>()
            .With<Bar_launcher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("work create NAME TITLE", "Create a new workspace directory in the repository NAME")
            .Check("work archive NAME [PATTERN] [PATTERN]", "Archive a workspace directory")
            .Check("work name NAME DIRECTORY", "Name a new repository")
            .EndUsing()

            .EndTest();
    }
}