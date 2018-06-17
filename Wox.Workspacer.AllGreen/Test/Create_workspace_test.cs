using AllGreen.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wox.Workspacer.AllGreen.Fixture;
using Wox.Workspacer.AllGreen.Helper;

namespace Wox.Workspacer.AllGreen.Test
{
    public class Create_workspace_test : TestBase<WorkspacerContext>
    {
        public override void DoTest() =>
            StartTest()

            .IsRunnable()

            .Include<Prepare_common_context>()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Write_query(@"work cr"))
            .EndUsing()

            .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check(@"work cr data", @"Create a new workspace in the data repo")
            .Check(@"work cr erk", @"Create a new workspace in the erk repo")
            .EndUsing()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Append__on_query(@" a"))
            .DoCheck(f => f.The_current_query_is(), "work cr a")
            .EndUsing()

            .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check(@"work cr data", @"Create a new workspace in the data repo")
            .EndUsing()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Wox_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "work cr data ")
            .EndUsing()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Append__on_query(@"log parsing"))
            .EndUsing()

             .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check(@"work cr data log parsing", @"Create new workspace ""log parsing"" in repo data")
            .EndUsing()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Wox_is_displayed())
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