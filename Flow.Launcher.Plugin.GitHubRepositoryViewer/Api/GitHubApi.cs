using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Octokit;

namespace Flow.Launcher.Plugin.GitHubRepositoryViewer.API;

public class GitHubAPI
{
    public string Token { get; }
    public bool HasToken { get; }
    public bool IsLoadingRepositories { get; private set; }
    public bool ErroredOut { get; private set; }
    public List<Repository> Repositories { get; private set; } = new();

    private readonly PluginInitContext _context;

    public GitHubAPI(string token, PluginInitContext context)
    {
        Token = token;
        HasToken = !string.IsNullOrEmpty(token);
        _context = context;
    }

    public async Task FetchRepositories()
    {
        if (!HasToken)
        {
            ErroredOut = true;

            Exception exception = new MissingFieldException("No GitHub API token provided");
            _context.API.ShowMsg(exception.Message, exception.ToString());

            Repositories = new List<Repository>();

            return;
        }

        IsLoadingRepositories = true;
        ErroredOut = false;

        try
        {
            GitHubClient client = new(
                new ProductHeaderValue("Flow.Launcher.Plugin.GitHubRepositoryViewer")
            )
            {
                Credentials = new Credentials(Token),
            };

            List<Repository> repositories = new();

            int page = 1;
            while (true)
            {
                ApiOptions options = new()
                {
                    PageSize = 100,
                    PageCount = 1,
                    StartPage = page,
                };

                IReadOnlyList<Repository> repos = await client.Repository.GetAllForCurrent(options);

                if (repos.Count == 0)
                {
                    break;
                }

                repositories.AddRange(repos);

                page++;
            }

            Repositories = repositories;

            return;
        }
        catch (Exception ex)
        {
            ErroredOut = true;

            _context.API.ShowMsg(ex.Message, ex.ToString());

            Repositories = new List<Repository>();
        }
        finally
        {
            IsLoadingRepositories = false;
        }
    }
}
