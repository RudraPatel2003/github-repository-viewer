using System;
using System.Collections.Generic;

namespace Flow.Launcher.Plugin.GitHubRepositoryViewer;

public class GitHubRepositoryViewer : IPlugin
{
    private PluginInitContext? _context;

    public void Init(PluginInitContext context)
    {
        _context = context;
    }

    public List<Result> Query(Query query)
    {
        if (_context is null)
        {
            return new List<Result>();
        }

        Console.WriteLine(_context.API.FuzzySearch("Hello World", "Hello"));

        return new List<Result>();
    }
}
