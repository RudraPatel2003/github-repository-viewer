using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flow.Launcher.Plugin.GitHubRepositoryViewer.Helpers;

public static class Messages
{
    public static Task<List<Result>> GetMissingTokenMessage(PluginInitContext? context)
    {
        Result missingTokenResult = new()
        {
            Title = "Missing API Token",
            SubTitle = "Please provide a GitHub API token in the settings",
            Score = 100,
            IcoPath = Constants.IconPath,
            Action = (e) =>
            {
                context?.API.OpenSettingDialog();
                return true;
            },
        };

        Result reloadPluginResult = GetReloadPluginResult(context);

        return Task.FromResult(new List<Result>() { missingTokenResult, reloadPluginResult });
    }

    public static Task<List<Result>> GetLoadingMessage()
    {
        Result loadingResult = new()
        {
            Title = "Fetching repositories...",
            SubTitle = "Please wait...",
            IcoPath = Constants.IconPath,
        };

        return Task.FromResult(new List<Result>() { loadingResult });
    }

    public static Task<List<Result>> GetErrorMessage(PluginInitContext? context)
    {
        Result errorResult = new()
        {
            Title = "Error",
            SubTitle = "An error occurred while fetching repositories",
            Score = 100,
            IcoPath = Constants.IconPath,
        };

        Result reloadPluginResult = GetReloadPluginResult(context);

        return Task.FromResult(new List<Result>() { errorResult, reloadPluginResult });
    }

    public static Task<List<Result>> GetNoResultsMessage(PluginInitContext? context)
    {
        Result noResultsResult = new()
        {
            Title = "No results",
            SubTitle = "No results found",
            Score = 100,
            IcoPath = Constants.IconPath,
        };

        Result reloadPluginResult = GetReloadPluginResult(context);

        return Task.FromResult(new List<Result>() { noResultsResult, reloadPluginResult });
    }

    private static Result GetReloadPluginResult(PluginInitContext? context)
    {
        Result reloadPluginResult = new()
        {
            Title = "Reload plugin",
            SubTitle = "Reload plugin",
            Score = 1,
            IcoPath = Constants.IconPath,
            Action = (e) =>
            {
                _ = context?.API.ReloadAllPluginData();
                return true;
            },
        };

        return reloadPluginResult;
    }
}
