using System.Diagnostics.CodeAnalysis;
using Cocona;
using MultiSlackPost.Domain;
using SlackAPI;

namespace MultiSlackPost.Commands;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("Performance", "CA1822:Mark members as static")]
public class Post
{
    public async Task PostAsync([Argument(Description = "Body message")] string body,
        [FromService] IConfigRepository configRepository)
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
                var slackClient = new SlackTaskClient(token);
                tasks.Add(slackClient.PostMessageAsync(channel, body));
            }
        }

        await Task.WhenAll(tasks);
    }
}