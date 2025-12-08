using System.Collections.Generic;

namespace Flow.Launcher.Plugin.GitHubRepositoryViewer.Models;

public class Settings
{
    public string ApiToken { get; set; } = string.Empty;
    public HashSet<string> ExcludedOwners { get; set; } = new();
}
