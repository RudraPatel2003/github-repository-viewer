using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Flow.Launcher.Plugin.GitHubRepositoryViewer.Models;

namespace Flow.Launcher.Plugin.GitHubRepositoryViewer.UI;

public partial class SettingsView : UserControl
{
    private readonly PluginInitContext _context;
    private readonly SettingsViewModel _viewModel;

    public SettingsView(PluginInitContext context, SettingsViewModel viewModel)
    {
        _context = context;
        _viewModel = viewModel;

        DataContext = viewModel;

        InitializeComponent();
    }

    private void Save()
    {
        _viewModel.SyncExcludedOwnersToSettings();
        _context.API.SaveSettingJsonStorage<Settings>();
    }

    private void ButtonAdd_Click(object sender, RoutedEventArgs e)
    {
        string newUser = ExcludedOwnerInput.Text.Trim();

        ExcludedOwnerInput.Clear();

        if (string.IsNullOrWhiteSpace(newUser))
        {
            return;
        }

        if (_viewModel.ExcludedOwnersCollection.Contains(newUser, StringComparer.OrdinalIgnoreCase))
        {
            return;
        }

        _viewModel.ExcludedOwnersCollection.Add(newUser);

        Save();
    }

    private void ButtonDelete_Click(object sender, RoutedEventArgs e)
    {
        foreach (string? selected in ExcludedOwnersList.SelectedItems.Cast<string>().ToList())
        {
            _ = _viewModel.ExcludedOwnersCollection.Remove(selected);
        }

        Save();
    }
}
