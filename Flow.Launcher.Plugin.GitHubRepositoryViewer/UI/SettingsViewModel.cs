using System.Collections.ObjectModel;
using System.Linq;
using Flow.Launcher.Plugin.GitHubRepositoryViewer.Models;

namespace Flow.Launcher.Plugin.GitHubRepositoryViewer.UI;

public class SettingsViewModel : BaseModel
{
    public Settings Settings { get; }

    public string ApiToken
    {
        get => Settings.ApiToken;
        set
        {
            Settings.ApiToken = value;
            OnPropertyChanged();
        }
    }

    public bool SearchByRepositoryFullName
    {
        get => Settings.SearchByRepositoryFullName;
        set
        {
            Settings.SearchByRepositoryFullName = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<string> ExcludedOwnersCollection { get; }

    public SettingsViewModel(Settings settings)
    {
        Settings = settings;
        ExcludedOwnersCollection = new ObservableCollection<string>(Settings.ExcludedOwners);
    }

    public void SyncExcludedOwnersToSettings()
    {
        Settings.ExcludedOwners = ExcludedOwnersCollection
            .Select(u => u.Trim())
            .Where(u => u.Length > 0)
            .ToHashSet();
    }
}
