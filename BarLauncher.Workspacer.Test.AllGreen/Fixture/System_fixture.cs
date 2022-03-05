using AllGreen.Lib;
using System;
using System.Globalization;
using BarLauncher.EasyHelper;
using BarLauncher.Workspacer.Test.AllGreen.Helper;

namespace BarLauncher.Workspacer.Test.AllGreen.Fixture
{
    public class System_fixture : FixtureBase<WorkspacerContext>
    {
        public bool The_path__exists(string path) => Context.ApplicationStarter.WorkspacerSystemService.DirectoryExists(path);

        public void The_current_date_time_is(string dateTime)
        {
            if (DateTime.TryParse(dateTime, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime currentDateTime))
            {
                Context.ApplicationStarter.WorkspacerSystemService.Now = currentDateTime;
                return;
            }
            throw new AllGreenException("The date time [{0}] isn't well formatted".FormatWith(dateTime));
        }
    }
}