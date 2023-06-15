using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Cocona;
using MultiSlackPost.Domain;
using SlackAPI;
using File = System.IO.File;

namespace MultiSlackPost.Commands;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("Performance", "CA1822:Mark members as static")]
public class Post
{
    public async Task PostAsync([Option('b', Description = "body message")] string body)
    {
        if (!File.Exists(Def.ConfigFilePath))
        {
            throw new CommandExitedException("configuration does not exist", 1);
        }

        var oldJson = await File.ReadAllTextAsync(Def.ConfigFilePath);
        var config = JsonSerializer.Deserialize<Domain.Config>(oldJson,
                         new JsonSerializerOptions { IncludeFields = true }) ??
                     throw new CommandExitedException("Deserialization resulted in null.", 1);

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