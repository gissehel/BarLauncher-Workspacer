using AllGreen.Lib;
using System.Linq;
using Wox.Workspacer.AllGreen.Helper;
using Wox.Workspacer.DomainModel;
using Wox.Workspacer.Tool;

namespace Wox.Workspacer.AllGreen.Fixture
{
    public class Wox_bar_fixture : FixtureBase<WorkspacerContext>
    {
        public void Start_the_bar() => Context.ApplicationStarter.Start();

        public void Write_query(string query)
        {
            AssertDisplayed();

            Context.ApplicationStarter.WoxContextService.SetQueryFromInterface(query);
        }

        public void Append__on_query(string element)
        {
            var newQuery = Context.ApplicationStarter.WoxContextService.CurrentQuery + element;
            Write_query(newQuery);
        }

        private void AssertDisplayed()
        {
            if (!Context.ApplicationStarter.WoxContextService.WoxDisplayed)
            {
                throw new AllGreenException("The Wox application is not currently displayed. It should be");
            }
        }

        public void Display_wox()
        {
            Context.ApplicationStarter.WoxContextService.WoxDisplayed = true;
        }

        private WoxResult AssertAndGetResult(int lineNumber)
        {
            var lineIndex = lineNumber - 1;
            var resultCount = Context.ApplicationStarter.WoxContextService.Results.Count();
            if (lineIndex < 0 || lineIndex >= resultCount)
            {
                throw new AllGreenException("Invalid line number [{0}], there are [{1}] result currently shown.".FormatWith(lineNumber, resultCount));
            }
            return Context.ApplicationStarter.WoxContextService.Results.ElementAt(lineIndex);
        }

        public string The_title_of_result__is(int lineNumber) => AssertAndGetResult(lineNumber).Title;

        public string The_subtitle_of_result__is(int lineNumber) => AssertAndGetResult(lineNumber).SubTitle;

        public string The_number_of_results_is() => Context.ApplicationStarter.WoxContextService.Results.Count().ToString();

        public void Select_line(int lineNumber)
        {
            AssertDisplayed();
            var lineIndex = lineNumber - 1;
            if (lineIndex < 0 || lineIndex >= Context.ApplicationStarter.WoxContextService.Results.Count())
            {
                throw new AllGreenException("Line number [{0}] is invalid, there is currently only [{1}] results shown".FormatWith(lineNumber, Context.ApplicationStarter.WoxContextService.Results.Count()));
            }

            var result = Context.ApplicationStarter.WoxContextService.Results.ElementAt(lineIndex);
            if (result.Action != null)
            {
                result.Action();
                Context.ApplicationStarter.WoxContextService.WoxDisplayed = !result.ShouldClose;
            }
        }

        public bool Wox_is_displayed()
        {
            return Context.ApplicationStarter.WoxContextService.WoxDisplayed;
        }

        public string The_current_query_is()
        {
            AssertDisplayed();
            return Context.ApplicationStarter.WoxContextService.CurrentQuery;
        }
    }
}