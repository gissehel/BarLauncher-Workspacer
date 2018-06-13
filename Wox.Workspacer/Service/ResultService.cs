using System.Collections.Generic;
using Wox.Plugin;
using Wox.Workspacer.Core.Service;
using Wox.Workspacer.DomainModel;

namespace Wox.Workspacer.Service
{
    public class ResultService : IResultService
    {
        public List<Result> MapResults(IEnumerable<WoxResult> results)
        {
            var resultList = new List<Result>();
            foreach (var result in results)
            {
                var action = result.Action;
                resultList.Add(new Result
                {
                    Title = result.Title,
                    SubTitle = result.SubTitle,
                    IcoPath = "Wox.Workspacer-64.png",
                    Action = e =>
                    {
                        if (e.SpecialKeyState.CtrlPressed)
                        {
                            if (result.CtrlAction != null)
                            {
                                return result.CtrlAction();
                            }
                            return false;
                        }
                        else if (e.SpecialKeyState.WinPressed)
                        {
                            if (result.WinAction != null)
                            {
                                return result.WinAction();
                            }
                            return false;
                        }
                        else
                        {
                            action();
                            return result.ShouldClose;
                        }
                    }
                });
            }
            return resultList;
        }
    }
}