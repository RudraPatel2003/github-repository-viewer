using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Flow.Launcher.Plugin.GitHubRepositoryViewer.Models;
using Flow.Launcher.Plugin.GitHubRepositoryViewer.UI;

namespace Flow.Launcher.Plugin.GitHubRepositoryViewer;

public class GitHubRepositoryViewer : IAsyncPlugin, ISettingProvider
{
    private const string IconPath = "Assets/icon.png";
    private PluginInitContext? _context;
    private Settings? _settings;

    public async Task InitAsync(PluginInitContext context)
    {
        _context = context;
        _settings = context.API.LoadSettingJsonStorage<Settings>();
    }

    public async Task<List<Result>> QueryAsync(Query query, CancellationToken token)
    {
        if (_context is null)
        {
            return await Task.FromResult(new List<Result>());
        }

        Result result = new()
        {
            Title = "GitHub Repository Viewer",
            SubTitle = _settings?.ApiToken ?? "No API Token",
            IcoPath = IconPath,
        };

        return await Task.FromResult(new List<Result>() { result });
    }

    public Control CreateSettingPanel()
    {
        SettingsViewModel settingsViewModel = new(_settings ?? new Settings());

        return new SettingsView(settingsViewModel);
    }
}
