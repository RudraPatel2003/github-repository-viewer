using Octokit;

namespace Flow.Launcher.Plugin.GitHubRepositoryViewer.Models;

public class ScoredRepository
{
    public Repository Repository { get; set; }
    public int Score { get; set; }

    public ScoredRepository(Repository repository, int score)
    {
        Repository = repository;
        Score = score;
    }
}
