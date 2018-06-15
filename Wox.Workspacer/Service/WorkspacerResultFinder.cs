using System;
using System.Collections.Generic;
using System.Linq;
using Wox.Workspacer.Core.Service;
using Wox.Workspacer.DomainModel;

namespace Wox.Workspacer.Service
{
    public class WorkspacerResultFinder : IWoxResultFinder
    {
        private IWoxContextService WoxContextService { get; set; }
        private IWorkspacerService WorkspacerService { get; set; }
        public WorkspacerResultFinder(IWoxContextService woxContextService, IWorkspacerService workspacerService)
        {
            WoxContextService = woxContextService;
            WorkspacerService = workspacerService;
        }

        public IEnumerable<WoxResult> GetResults(WoxQuery query)
        {
            switch (query.FirstTerm)
            {
                case "config":
                    return GetConfig(query);

                default:
                    var commands = GetCommandHelp(query.FirstTerm);
                    if (commands.Any())
                    {
                        return commands;
                    }
                    // return GetList(query.SearchTerms);
                    return commands;
            }
        }

        private Dictionary<string, KeyValuePair<Func<WorkspacerConfiguration, string>, Action<WorkspacerConfiguration, string>>> _configNames = new Dictionary<string, KeyValuePair<Func<WorkspacerConfiguration, string>, Action<WorkspacerConfiguration, string>>>()
        {
            {
                "OpenDirCommand",
                new KeyValuePair<Func<WorkspacerConfiguration, string>, Action<WorkspacerConfiguration, string>>
                (
                    new Func<WorkspacerConfiguration, string>(c=>c.Launcher),
                    new Action<WorkspacerConfiguration, string>((c,v)=>{c.Launcher = v; })
                )
            },
        };

        private IEnumerable<WoxResult> GetConfig(WoxQuery query)
        {
            var configuration = WorkspacerService.GetConfiguration();

            string configName = null;

            if (query.SearchTerms.Length > 1)
            {
                configName = query.SearchTerms[1];
            }
            else
            {
                configName = "";
            }

            string configValue = null;
            if (configName.Contains("="))
            {
                var position = configName.IndexOf("=");
                configValue = configName.Substring(position + 1, configName.Length - position - 1);
                configName = configName.Substring(0, position - 1);
            }

            if (_configNames.ContainsKey(configName))
            {
                var reader = _configNames[configName].Key;
                var writer = _configNames[configName].Value;

                if (configValue == null)
                {
                    yield return GetCompletionResult("work config " + configName, string.Format("Select {0}={1}", configName, reader(configuration)), () => string.Format("config {0}={1}", configName, reader(configuration)));
                }
                else
                {
                    yield return GetActionResult("work config " + configName, string.Format("Set the value for {0} to {1}", configName, reader(configuration)), () => writer(configuration, configValue));
                }
            }
            else
            {
                if (configValue == null)
                {
                    foreach (var configNameReference in _configNames.Keys.OrderBy(k => k))
                    {
                        if (PatternMatch(configName, configNameReference))
                        {
                            yield return GetCompletionResult("work config " + configNameReference, string.Format("{0}={1}", configNameReference, _configNames[configNameReference].Key(configuration)), () => "config " + configNameReference);
                        }
                    }
                }
                else
                {
                    yield return GetCompletionResultFinal("Configuration error", string.Format("There is no configuration item named {0}", configName), () => "config " + configName);
                }
            }
        }


        private bool PatternMatch(string pattern, string command) => string.IsNullOrEmpty(pattern) || command.Contains(pattern);

        private WoxResult _helpList = null;
        private WoxResult _helpConfig = null;
        private WoxResult _helpCreate = null;
        private WoxResult _helpChangeDir = null;
        private WoxResult _helpArchive = null;
        private WoxResult _helpName = null;
        private WoxResult HelpList => _helpList ?? (_helpList = GetCompletionResult("work list", "List all the available repositories", () => "list"));
        private WoxResult HelpConfig => _helpConfig ?? (_helpConfig = GetCompletionResult("work config KEY VALUE", "View/Change workspacer configuration", () => "config"));
        private WoxResult HelpCreate => _helpCreate ?? (_helpCreate = GetCompletionResult("work cr NAME TITLE", "Create a new workspace directory in the repository NAME", () => "cr"));
        private WoxResult HelpChangeDir => _helpChangeDir ?? (_helpChangeDir = GetCompletionResult("work cd NAME [PATTERN] [PATTERN]", "Change to a workspace directory", () => "cd"));
        private WoxResult HelpArchive => _helpArchive ?? (_helpArchive = GetCompletionResult("work ar NAME [PATTERN] [PATTERN]", "Archive a workspace directory", () => "ar"));
        private WoxResult HelpName => _helpName ?? (_helpName = GetCompletionResult("work name NAME DIRECTORY", "Name a new repository", () => "name"));

        private IEnumerable<WoxResult> GetCommandHelp(string pattern)
        {
            if (PatternMatch(pattern, "cr")) yield return HelpCreate;
            if (PatternMatch(pattern, "cd")) yield return HelpChangeDir;
            if (PatternMatch(pattern, "ar")) yield return HelpArchive;
            if (PatternMatch(pattern, "name")) yield return HelpName;
            if (PatternMatch(pattern, "list")) yield return HelpList;
            if (PatternMatch(pattern, "config")) yield return HelpConfig;
        }

        public WoxResult GetActionResult(string title, string subTitle, Action action) => new WoxResult
        {
            Title = title,
            SubTitle = subTitle,
            Action = () =>
            {
                action();
                WoxContextService.ChangeQuery("");
            },
            ShouldClose = true,
        };

        public WoxResult GetCompletionResult(string title, string subTitle, Func<string> getNewQuery) => new WoxResult
        {
            Title = title,
            SubTitle = subTitle,
            Action = () => WoxContextService.ChangeQuery(WoxContextService.ActionKeyword + WoxContextService.Seperater + getNewQuery() + WoxContextService.Seperater),
            ShouldClose = false,
        };

        public WoxResult GetCompletionResultFinal(string title, string subTitle, Func<string> getNewQuery) => new WoxResult
        {
            Title = title,
            SubTitle = subTitle,
            Action = () => WoxContextService.ChangeQuery(WoxContextService.ActionKeyword + WoxContextService.Seperater + getNewQuery()),
            ShouldClose = false,
        };
    }
}