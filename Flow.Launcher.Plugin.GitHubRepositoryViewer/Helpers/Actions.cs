using System.Diagnostics;

namespace Flow.Launcher.Plugin.GitHubRepositoryViewer.Helpers;

public static class Actions
{
    public static void OpenUrl(string url)
    {
        ProcessStartInfo processStartInfo = new() { FileName = url, UseShellExecute = true };

        _ = Process.Start(processStartInfo);
    }
}
