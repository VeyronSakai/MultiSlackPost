using System.Diagnostics.CodeAnalysis;
using Cocona;
using Microsoft.Extensions.Logging;
using MultiSlackPost.Domain;
using ZLogger;

namespace MultiSlackPost.Commands;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("Performance", "CA1822:Mark members as static")]
[SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
public class Channel
{
    [Command("add", Description = "add channel info to config file")]
    public async Task AddChannelAsync([Option('w')] string workspace,
        [Option('c')] string channel,
        [FromService] IConfigRepository configRepository,
        [FromService] ILogger<Channel> logger)
    {
        var config = configRepository.Exists() ? await configRepository.GetAsync() : new Domain.Config();
        config.AddChannel(workspace, channel);
        await configRepository.SaveAsync(config);
        logger.ZLogInformation("Successfully added channel info to config file.");
    }

    [Command("remove", Description = "remove channel info to config file")]
    public async Task RemoveChannelAsync([Option('w')] string workspace,
        [Option('c')] string channel,
        [FromService] IConfigRepository configRepository,
        [FromService] ILogger<Channel> logger)
    {
        var config = configRepository.Exists()
            ? await configRepository.GetAsync()
            : throw new CommandExitedException("Config file does not exist.", 1);

        config.RemoveChannel(workspace, channel);

        await configRepository.SaveAsync(config);

        logger.ZLogInformation("Successfully removed channel info in config file.");
    }
}