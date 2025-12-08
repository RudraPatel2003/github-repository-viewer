<h1 align="center">
  <br>
    <img src="Flow.Launcher.Plugin.GitHubRepositoryViewer/Assets/repository-banner.png" alt="Repository Banner" width="25%">  
  <br>
    Github Repository Viewer
</h1>

## Description

A plugin for the [Flow launcher](https://github.com/Flow-Launcher/Flow.Launcher) that allows you to quickly navigate to your personal GitHub repositories.

## How Does It Work?

Provide a GitHub API token in the Plugin's settings, and on initial load, the plugin will fetch all of your repositories and store it.

Then, type a query and the plugin will fuzzy search from your repositories and display the results. Select a repository to open it in your browser!

If you have a specific user or organization you want to exclude, you can exclude them in the plugin's settings as well.

## Getting Started

### Setup

Ensure you have `.NET 10` installed on your machine.

You must have a Windows machine with the [Flow launcher](https://github.com/Flow-Launcher/Flow.Launcher) installed.

### Local Development

To run the plugin locally, make your changes and then run the `debug.ps1` script. This will relaunch the Flow launcher with the plugin loaded.

To generate a release build, run the `release.ps1` script and find it under the `Flow.Launcher.Plugin.GitHubRepositoryViewer/bin/` directory.
