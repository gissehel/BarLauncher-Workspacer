using System;

namespace Wox.Workspacer.DomainModel
{
    public class WoxResult
    {
        public string Title { get; set; }

        public string SubTitle { get; set; }

        public Action Action { get; set; }

        public Func<bool> CtrlAction { get; set; }

        public Func<bool> WinAction { get; set; }

        public bool ShouldClose { get; set; } = true;
    }
}