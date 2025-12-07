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

public class GitHubRepositoryViewer : IAsyncPlugin, ISettingProvider
{
    private const string IconPath = "Assets/icon.png";
    private PluginInitContext? _context;
    private Settings? _settings;
    private GitHubAPI? _gitHubAPI;

    public async Task InitAsync(PluginInitContext context)
    {
        _context = context;
        _settings = context.API.LoadSettingJsonStorage<Settings>();
        _gitHubAPI = new GitHubAPI(_settings.ApiToken, context);

        await _gitHubAPI.FetchRepositories();
    }

    public async Task<List<Result>> QueryAsync(Query query, CancellationToken token)
    {
        if (_context is null || _gitHubAPI is null)
        {
            return await GetLoadingMessage();
        }

        if (!_gitHubAPI.HasToken)
        {
            return await GetMissingTokenMessage();
        }

        if (_gitHubAPI.IsLoadingRepositories)
        {
            return await GetLoadingMessage();
        }

        if (_gitHubAPI.ErroredOut)
        {
            return await GetErrorMessage();
        }

        return await GetResults(query.Search);
    }

    public Control CreateSettingPanel()
    {
        SettingsViewModel settingsViewModel = new(_settings ?? new Settings());

        return new SettingsView(settingsViewModel);
    }

    private async Task<List<Result>> GetMissingTokenMessage()
    {
        Result missingTokenResult = new()
        {
            Title = "Missing API Token",
            SubTitle = "Please provide a GitHub API token in the settings",
            IcoPath = IconPath,
            Action = (e) =>
            {
                _context?.API.OpenSettingDialog();
                return true;
            },
        };

        return await Task.FromResult(new List<Result>() { missingTokenResult });
    }

    private static Task<List<Result>> GetLoadingMessage()
    {
        Result loadingResult = new()
        {
            Title = "Fetching repositories...",
            SubTitle = "Please wait...",
            IcoPath = IconPath,
        };

        return Task.FromResult(new List<Result>() { loadingResult });
    }

    private static Task<List<Result>> GetErrorMessage()
    {
        Result errorResult = new()
        {
            Title = "Error",
            SubTitle = "An error occurred while fetching repositories",
            IcoPath = IconPath,
        };

        return Task.FromResult(new List<Result>() { errorResult });
    }

    private static Task<List<Result>> GetKeepTypingMessage()
    {
        Result keepTypingResult = new()
        {
            Title = "Keep Typing",
            SubTitle = "Please keep typing to search for a repository",
            IcoPath = IconPath,
        };

        return Task.FromResult(new List<Result>() { keepTypingResult });
    }

    private async Task<List<Result>> GetResults(string query)
    {
        if (_gitHubAPI is null)
        {
            return await Task.FromResult(new List<Result>());
        }

        if (string.IsNullOrWhiteSpace(query))
        {
            return await GetKeepTypingMessage();
        }

        List<Repository> repositories = _gitHubAPI.Repositories;

        var scoredRepositories = repositories
            .Select(repository => new
            {
                Repo = repository,
                Score = FuzzyScore.Score(repository.FullName, query),
            })
            .Where(x => x.Score > 0)
            .OrderByDescending(x => x.Score)
            .ToList();

        List<Result> results = scoredRepositories
            .Select(repository => new Result
            {
                Title = repository.Repo.FullName,
                SubTitle =
                    $"{repository.Repo.Description ?? "No description"}  —  Score: {repository.Score}",
                IcoPath = IconPath,
                Score = repository.Score,
                Action = _ =>
                {
                    Actions.OpenUrl(repository.Repo.HtmlUrl);
                    return true;
                },
            })
            .ToList();

        return await Task.FromResult(results);
    }
}
