<h1 align="center">
  <br>
    <img src="Flow.Launcher.Plugin.GitHubRepositoryViewer/Assets/repository-banner.png" alt="Repository Banner" width="25%">  
  <br>
    Github Repository Viewer
</h1>

## Description

A plugin for the [Flow launcher](https://github.com/Flow-Launcher/Flow.Launcher) that allows you to quickly navigate to your personal GitHub repositories.

## Usage

1. Provide a GitHub API token in the Plugin's settings, and on initial load, the plugin will fetch all of your repositories and store it.

2. Type a query and the plugin will fuzzy search from your repositories and display the results. Select a repository to open it in your browser!

3. Configure the plugin's settings to tailor it to your needs

    - If you have a specific user or organization you do not want to appear in the results, you can exclude them

    - You can include the repository's full name in the search, which will look at the owner as well as the repository name

        - For example, consider a repository called "owner/repository" with the search query "n". With the setting disabled, the plugin will not show the repository because "repository" does not contain the letter "n". With the setting enabled, the plugin will show the repository because "owner/repository" contains the letter "n".

## Development

### Setup

Ensure you have `.NET 10` installed on your machine.

You must have a Windows machine with the [Flow launcher](https://github.com/Flow-Launcher/Flow.Launcher) installed.

### Scripts

To run the plugin locally, make your changes and then run the `debug.ps1` script. This will relaunch the Flow launcher with the plugin loaded.

To generate a release build, run the `release.ps1` script and find it under the `Flow.Launcher.Plugin.GitHubRepositoryViewer/bin/` directory.
