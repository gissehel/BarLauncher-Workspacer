using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BarLauncher.EasyHelper;
using BarLauncher.EasyHelper.Core.Service;
using BarLauncher.Workspacer.Lib.Core.Service;
using BarLauncher.Workspacer.Lib.DomainModel;
using BarLauncher.Workspacer.Lib.Tool;

namespace BarLauncher.Workspacer.Lib.Service
{
    public class WorkspacerResultFinder : BarLauncherResultFinder
    {
        private IWorkspacerService WorkspacerService { get; set; }

        public WorkspacerResultFinder(IBarLauncherContextService barLauncherContextService, IWorkspacerService workspacerService) : base(barLauncherContextService)
        {
            WorkspacerService = workspacerService;
        }

        public override void Init()
        {
            WorkspacerService.Init();

            AddCommand("create", "create NAME TITLE", "Create a new workspace directory in the repository NAME", GetCreate);
            AddCommand("cd", "cd NAME [PATTERN] [PATTERN]", "Change to a workspace directory", GetChangeDir);
            AddCommand("archive", "archive NAME [PATTERN] [PATTERN]", "Archive a workspace directory", GetArchive);
            AddCommand("name", "name NAME DIRECTORY", "Name a new repository", GetName);
            AddCommand("list", "list", "List all the available repositories", GetList);
            if (! WorkspacerService.UseStandardDirectoryOpener)
            {
                AddCommand("config", "config KEY VALUE", "View/Change workspacer configuration", GetConfig);
            }
            AddDefaultCommand(GetChangeDir);
        }

        public override void Dispose()
        {
            WorkspacerService.Dispose();
        }

        private IEnumerable<BarLauncherResult> GetArchive(BarLauncherQuery query, int position)
        {
            string name = query.GetTermOrEmpty(position);

            string value = query.GetAllSearchTermsStarting(position + 1);

            string actualPath = WorkspacerService.GetPathByName(name);
            if (value == null && actualPath == null)
            {
                bool foundRepo = false;
                var repos = WorkspacerService.GetRepos();
                foreach (var repo in repos)
                {
                    if (repo.Name.MatchPatternCaseInvariant(name))
                    {
                        foundRepo = true;
                        yield return GetCompletionResult
                        (
                            "archive {0} [PATTERN] [PATTERN]".FormatWith(repo.Name),
                            "Archive a workspace in the {0} repo".FormatWith(repo.Name),
                            () => "archive {0}".FormatWith(repo.Name)
                        );
                    }
                }
                if (!foundRepo)
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        yield return GetEmptyCommandResult("ar", CommandInfos);
                    }
                }
            }
            else
            {
                if (actualPath != null)
                {
                    query.SearchTerms.Skip(2);
                    var workspaces = WorkspacerService.GetWorspaces(actualPath);
                    foreach (var workspace in workspaces)
                    {
                        if (query.SearchTerms.Skip(2).All(term => workspace.MatchPatternCaseInvariant(term)))
                        {
                            yield return GetActionResult
                            (
                                "archive {0} {1}".FormatWith(name, workspace),
                                "Archive {1}".FormatWith(name, workspace),
                                () => WorkspacerService.Archive(actualPath, workspace)
                            );
                        }
                    }
                }
                else
                {
                    yield return GetEmptyCommandResult("ar", CommandInfos);
                }
            }
        }

        private IEnumerable<BarLauncherResult> GetChangeDir(BarLauncherQuery query, int position)
        {
            string name = query.GetTermOrEmpty(position);

            string value = query.GetAllSearchTermsStarting(position + 1);

            string actualPath = WorkspacerService.GetPathByName(name);
            if (value == null && actualPath == null)
            {
                bool foundRepo = false;
                var repos = WorkspacerService.GetRepos();
                foreach (var repo in repos)
                {
                    if (repo.Name.MatchPatternCaseInvariant(name))
                    {
                        foundRepo = true;
                        yield return GetCompletionResult
                        (
                            "cd {0} [PATTERN] [PATTERN]".FormatWith(repo.Name),
                            "Search the {0} repo".FormatWith(repo.Name),
                            () => "cd {0}".FormatWith(repo.Name)
                        );
                    }
                }
                if (!foundRepo)
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        yield return GetEmptyCommandResult("cd", CommandInfos);
                    }
                }
            }
            else
            {
                if (actualPath != null)
                {
                    query.SearchTerms.Skip(2);
                    var workspaces = WorkspacerService.GetWorspaces(actualPath);
                    foreach (var workspace in workspaces)
                    {
                        if (query.SearchTerms.Skip(2).All(term => workspace.MatchPatternCaseInvariant(term)))
                        {
                            yield return GetActionResult
                            (
                                "cd {0} {1}".FormatWith(name, workspace),
                                "Go to {1}".FormatWith(name, workspace),
                                () => WorkspacerService.OpenDir(Path.Combine(actualPath, workspace))
                            );
                        }
                    }
                }
                else
                {
                    yield return GetEmptyCommandResult("cd", CommandInfos);
                }
            }
        }

        private IEnumerable<BarLauncherResult> GetCreate(BarLauncherQuery query, int position)
        {
            string name = query.GetTermOrEmpty(position);

            string value = query.GetAllSearchTermsStarting(position + 1);

            if (value == null)
            {
                bool foundRepo = false;
                var repos = WorkspacerService.GetRepos();
                foreach (var repo in repos)
                {
                    if (repo.Name.MatchPatternCaseInvariant(name))
                    {
                        foundRepo = true;
                        yield return GetCompletionResult
                        (
                            "create {0}".FormatWith(repo.Name),
                            "Create a new workspace in the {0} repo".FormatWith(repo.Name),
                            () => "create {0}".FormatWith(repo.Name)
                        );
                    }
                }
                if (!foundRepo)
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        yield return GetEmptyCommandResult("create", CommandInfos);
                    }
                }
            }
            else
            {
                string actualPath = WorkspacerService.GetPathByName(name);
                if (actualPath != null)
                {
                    yield return GetActionResult
                    (
                        "create {0} {1}".FormatWith(name, value),
                        "Create new workspace \"{1}\" in repo {0}".FormatWith(name, value),
                        () => WorkspacerService.CreateDir(actualPath, value)
                    );
                }
                else
                {
                    yield return GetEmptyCommandResult("create", CommandInfos);
                }
            }
        }

        private IEnumerable<BarLauncherResult> GetName(BarLauncherQuery query, int position)
        {
            string name = query.GetTermOrEmpty(position);

            string value = query.GetAllSearchTermsStarting(position + 1);

            if (value == null)
            {
                bool foundRepo = false;
                var repos = WorkspacerService.GetRepos();
                foreach (var repo in repos)
                {
                    if (repo.Name.MatchPatternCaseInvariant(name))
                    {
                        foundRepo = true;
                        yield return GetCompletionResult
                        (
                            "name {0} {1}".FormatWith(repo.Name, repo.Path),
                            "The current Path for name {0} is {1}".FormatWith(repo.Name, repo.Path),
                            () => "name {0} {1}".FormatWith(repo.Name, repo.Path)
                        );
                    }
                }
                if (!foundRepo)
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        yield return GetCompletionResult
                        (
                            "name {0}".FormatWith(name),
                            "There is no repo named {0} yet".FormatWith(name),
                            () => "name {0}".FormatWith(name)
                        );
                    }
                }
            }
            else
            {
                string actualPath = WorkspacerService.GetPathByName(name);
                if (actualPath != value)
                {
                    if (actualPath == null)
                    {
                        yield return GetActionResult
                        (
                            "name {0} {1}".FormatWith(name, value),
                            "Set repo name {0} to path {1}".FormatWith(name, value),
                            () =>
                            {
                                WorkspacerService.SetPathByName(name, value);
                            }
                        );
                    }
                    else
                    {
                        yield return GetActionResult
                        (
                            "name {0} {1}".FormatWith(name, value),
                            "Replace repo name {0} to path {1} (actual value is {2})".FormatWith(name, value, actualPath),
                            () =>
                            {
                                WorkspacerService.SetPathByName(name, value);
                            }
                        );
                    }
                }
                else
                {
                    yield return GetCompletionResultFinal
                    (
                        "name {0} {1}".FormatWith(name, value),
                        "The current name {0} points to path {1}".FormatWith(name, value),
                        () => "name {0} {1}".FormatWith(name, value)
                    );
                }
            }
        }

        private IEnumerable<BarLauncherResult> GetList(BarLauncherQuery query, int position)
        {
            string name = query.GetTermOrEmpty(position);

            var repos = WorkspacerService.GetRepos();
            foreach (var repo in repos)
            {
                if (repo.Name.MatchPatternCaseInvariant(name))
                {
                    yield return GetActionResult
                    (
                        "list {0}".FormatWith(repo.Name),
                        "Go to {0}".FormatWith(repo.Path),
                        () => WorkspacerService.OpenDir(repo.Path)
                    );
                }
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

        private IEnumerable<BarLauncherResult> GetConfig(BarLauncherQuery query, int position)
        {
            var configuration = WorkspacerService.GetConfiguration();
            var configName = query.GetTermOrEmpty(position);

            string configValue = null;
            if (configName.Contains("="))
            {
                configName = query.GetAllSearchTermsStarting(position);
                var equalPosition = configName.IndexOf("=");
                configValue = configName.Substring(equalPosition + 1, configName.Length - equalPosition - 1);
                configName = configName.Substring(0, equalPosition);
            }

            if (ConfigNames.ContainsKey(configName))
            {
                var reader = ConfigNames[configName].Key;
                var writer = ConfigNames[configName].Value;

                if (configValue == null)
                {
                    yield return GetCompletionResultFinal
                    (
                        "config {0}".FormatWith(configName),
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
                            "config {0}".FormatWith(configName),
                            "The current value for {0} is {1}".FormatWith(configName, reader(configuration)),
                            () => "config {0}={1}".FormatWith(configName, reader(configuration))
                        );
                    }
                    else
                    {
                        yield return GetActionResult
                        (
                            "config {0}".FormatWith(configName),
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
                        if (configNameReference.ToLowerInvariant().MatchPatternCaseInvariant(configName.ToLowerInvariant()))
                        {
                            yield return GetCompletionResult
                            (
                                "config {0}".FormatWith(configNameReference),
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
    }
}