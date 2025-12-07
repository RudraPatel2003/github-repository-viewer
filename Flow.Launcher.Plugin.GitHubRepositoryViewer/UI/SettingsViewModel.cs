using Flow.Launcher.Plugin.GitHubRepositoryViewer.Models;

namespace Flow.Launcher.Plugin.GitHubRepositoryViewer.UI;

public class SettingsViewModel : BaseModel
{
    public Settings Settings { get; init; }

    public SettingsViewModel(Settings settings)
    {
        Settings = settings;
    }

    public string ApiToken
    {
        get => Settings.ApiToken;
        set
        {
            Settings.ApiToken = value;
            OnPropertyChanged();
        }
    }
}
