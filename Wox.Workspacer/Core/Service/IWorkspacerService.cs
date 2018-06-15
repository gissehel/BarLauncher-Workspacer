using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wox.Workspacer.DomainModel;

namespace Wox.Workspacer.Core.Service
{
    public interface IWorkspacerService
    {
        void Init();
        WorkspacerConfiguration GetConfiguration();
    }
}