using System;
using System.Collections.Generic;
using System.Linq;
using Wox.Workspacer.Core.Service;
using Wox.Workspacer.DomainModel;
using Wox.Workspacer.Tool;

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

        private Dictionary<string, KeyValuePair<Func<WorkspacerConfiguration, string>, Action<WorkspacerConfiguration, string>>> _configNames = null;

        private Dictionary<string, KeyValuePair<Func<WorkspacerConfiguration, string>, Action<WorkspacerConfiguration, string>>> ConfigNames => _configNames ?? (_configNames = new Dictionary<string, KeyValuePair<Func<WorkspacerConfiguration, string>, Action<WorkspacerConfiguration, string>>>()
        {
            {
                "OpenDirCommand",
                new KeyValuePair<Func<WorkspacerConfiguration, string>, Action<WorkspacerConfiguration, string>>
                (
                    new Func<WorkspacerConfiguration, string>(c=>c.Launcher),
                    new Action<WorkspacerConfiguration, string>((c,v)=>c.Launcher = v)
                )
            },
        });

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
                configName = string.Join(" ", query.SearchTerms.Skip(1).ToArray());
                configValue = configName.Substring(position + 1, configName.Length - position - 1);
                configName = configName.Substring(0, position);
            }

            if (ConfigNames.ContainsKey(configName))
            {
                var reader = ConfigNames[configName].Key;
                var writer = ConfigNames[configName].Value;

                if (configValue == null)
                {
                    yield return GetCompletionResultFinal
                    (
                        "work config {0}".FormatWith(configName),
                        "Select {0}={1}".FormatWith(configName, reader(configuration)),
                        () => "config {0}={1}".FormatWith(configName, reader(configuration))
                    );
                }
                else
                {
                    if (reader(configuration) == configValue)
                    {
                        yield return GetCompletionResultFinal
                        (
                            "work config {0}".FormatWith(configName),
                            "The current value for {0} is {1}".FormatWith(configName, reader(configuration)),
                            () => "config {0}={1}".FormatWith(configName, reader(configuration))
                        );
                    }
                    else
                    {
                        yield return GetActionResult
                        (
                            "work config {0}".FormatWith(configName),
                            "Set the value for {0} to {1}".FormatWith(configName, configValue),
                            () =>
                            {
                                writer(configuration, configValue);
                                WorkspacerService.SaveConfiguration(configuration);
                            }
                        );
                    }
                }
            }
            else
            {
                if (configValue == null)
                {
                    foreach (var configNameReference in ConfigNames.Keys.OrderBy(k => k))
                    {
                        if (PatternMatch(configName, configNameReference))
                        {
                            yield return GetCompletionResult
                            (
                                "work config {0}".FormatWith(configNameReference),
                                "{0}={1}".FormatWith(configNameReference, ConfigNames[configNameReference].Key(configuration)),
                                () => "config {0}".FormatWith(configNameReference)
                            );
                        }
                    }
                }
                else
                {
                    yield return GetCompletionResultFinal
                    (
                        "Configuration error",
                        "There is no configuration item named {0}".FormatWith(configName),
                        () => "config {0}".FormatWith(configName)
                    );
                }
            }
        }

        private bool PatternMatch(string pattern, string command) => string.IsNullOrEmpty(pattern) || command.ToLower().Contains(pattern.ToLower());

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