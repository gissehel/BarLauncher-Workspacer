using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wox.Workspacer.DomainModel;

namespace Wox.Workspacer.Core.Service
{
    public interface IWorkspacerConfigurationRepository
    {
        void Init();

        WorkspacerConfiguration GetConfiguration();

        void SaveConfiguration(WorkspacerConfiguration configuration);
    }
}