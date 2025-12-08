using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Flow.Launcher.Plugin.GitHubRepositoryViewer.API;
using Flow.Launcher.Plugin.GitHubRepositoryViewer.Helpers;
using Flow.Launcher.Plugin.GitHubRepositoryViewer.Models;
using Flow.Launcher.Plugin.GitHubRepositoryViewer.UI;
using Octokit;

namespace Flow.Launcher.Plugin.GitHubRepositoryViewer;

public class GitHubRepositoryViewer : IAsyncPlugin, ISettingProvider, IAsyncReloadable
{
    private PluginInitContext? _context;
    private Settings? _settings;
    private GitHubAPI? _gitHubAPI;

    public Task InitAsync(PluginInitContext context)
    {
        _context = context;
        _settings = context.API.LoadSettingJsonStorage<Settings>();
        _gitHubAPI = new GitHubAPI(_settings.ApiToken, context);

        return _gitHubAPI.FetchRepositories();
    }

    public Task<List<Result>> QueryAsync(Query query, CancellationToken token)
    {
        if (_context is null || _gitHubAPI is null)
        {
            return Messages.GetLoadingMessage();
        }

        if (!_gitHubAPI.HasToken)
        {
            return Messages.GetMissingTokenMessage(_context);
        }

        if (_gitHubAPI.IsLoadingRepositories)
        {
            return Messages.GetLoadingMessage();
        }

        if (_gitHubAPI.ErroredOut)
        {
            return Messages.GetErrorMessage(_context);
        }

        return GetResults(query.Search);
    }

    public Control CreateSettingPanel()
    {
        SettingsViewModel settingsViewModel = new(_settings ?? new Settings());

        return new SettingsView(settingsViewModel);
    }

    public Task ReloadDataAsync()
    {
        if (_context is null)
        {
            return Task.CompletedTask;
        }

        return InitAsync(_context);
    }

    private Task<List<Result>> GetResults(string query)
    {
        query = query.ToLowerInvariant().Trim();

        if (_gitHubAPI is null)
        {
            return Task.FromResult(new List<Result>());
        }

        if (string.IsNullOrWhiteSpace(query))
        {
            return Messages.GetKeepTypingMessage();
        }

        List<Repository> repositories = _gitHubAPI.Repositories;

        var scoredRepositories = repositories
            .Select(repository => new
            {
                Repo = repository,
                Score = FuzzyScore.Score(repository.FullName, query),
            })
            .Where(repository => repository.Score > 0)
            .OrderByDescending(repository => repository.Score)
            .ThenByDescending(repository => repository.Repo.UpdatedAt)
            .ToList();

        if (scoredRepositories.Count == 0)
        {
            return Messages.GetNoResultsMessage(_context);
        }

        List<Result> results = scoredRepositories
            .Select(repository => new Result
            {
                Title = repository.Repo.FullName,
                SubTitle = repository.Repo.Description,
                IcoPath = Constants.IconPath,
                Score = repository.Score,
                Action = _ =>
                {
                    Actions.OpenUrl(repository.Repo.HtmlUrl);
                    return true;
                },
            })
            .ToList();

        return Task.FromResult(results);
    }
}
