using System.Diagnostics.CodeAnalysis;
using Cocona;
using Microsoft.Extensions.Logging;
using MultiSlackPost.Domain;
using SlackAPI;
using ZLogger;

namespace MultiSlackPost.Command;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("Performance", "CA1822:Mark members as static")]
public class PostCommand
{
    [Command(Description = "post message to channels")]
    public async Task PostAsync([Argument(Description = "body message")] string body,
        [FromService] IConfigRepository configRepository,
        [FromService] ILogger<ChannelCommand> logger)
    {
        if (!configRepository.Exists())
        {
            throw new CommandExitedException("configuration does not exist", 1);
        }

        var config = await configRepository.GetAsync();

        var tasks = new List<Task>();

        foreach (var (workspace, channels) in config.Channels)
        {
            config.Tokens.TryGetValue(workspace, out var token);
            if (token == null)
            {
                throw new CommandExitedException($"token is not registered. workspace = {workspace}", 1);
            }

            foreach (var channel in channels)
            {
                tasks.Add(SendAsync(channel, workspace, body, token, logger));
            }
        }

        await Task.WhenAll(tasks);
    }

    private static async Task SendAsync(string channel, string workspace, string body, string token, ILogger logger)
    {
        var slackClient = new SlackTaskClient(token);
        var response = await slackClient.PostMessageAsync(channel, body);
        if (response.ok)
        {
            logger.ZLogInformation($"Successfully posted message to #{channel} in workspace {workspace}");
        }
        else
        {
            throw new CommandExitedException(
                $"An error occurred when posting to #{channel} in workspace {workspace}. Error: {response.error}", 1);
        }
    }
}