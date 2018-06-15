using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wox.Workspacer.Core.Service;
using Wox.Workspacer.DomainModel;

namespace Wox.Workspacer.Service
{
    public class WorkspacerService : IWorkspacerService
    {
        public WorkspacerConfiguration GetConfiguration()
        {
            return new WorkspacerConfiguration { Launcher = "%1" };
        }

        public void Init()
        {

        }
    }
}