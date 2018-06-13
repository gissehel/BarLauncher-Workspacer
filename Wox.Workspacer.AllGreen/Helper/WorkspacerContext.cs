using AllGreen.Lib.Core;
using AllGreen.Lib.Core.DomainModel.Script;
using AllGreen.Lib.Core.DomainModel.ScriptResult;
using AllGreen.Lib.DomainModel;
using AllGreen.Lib.DomainModel.ScriptResult;
using System.IO;
using System.Text;

namespace Wox.Workspacer.AllGreen.Helper
{
    public class WorkspacerContext : IContext<WorkspacerContext>
    {
        public ITestScript<WorkspacerContext> TestScript { get; set; }
        public ITestScriptResult<WorkspacerContext> TestScriptResult { get; set; }

        public ApplicationStarter ApplicationStarter { get; set; }

        public void OnTestStart()
        {
            ApplicationStarter = new ApplicationStarter();
            ApplicationStarter.Init(TestScript.Name);
        }

        public void OnTestStop()
        {
            var testScriptResult = TestScriptResult as TestScriptResult<WorkspacerContext>;
            var path = Path.Combine(ApplicationStarter.TestPath, string.Format("{0}.agout", testScriptResult.TestScript.Name));
            using (var writer = new StreamWriter(path, false, new UTF8Encoding(false)))
            {
                writer.Write(testScriptResult.GetPipedName(PipedNameOptions.Detailled));
            }
        }
    }
}