using System.Windows.Controls;

namespace Flow.Launcher.Plugin.GitHubRepositoryViewer.UI;

public partial class SettingsView : UserControl
{
    public SettingsView(SettingsViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
