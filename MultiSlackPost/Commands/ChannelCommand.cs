using System.Diagnostics.CodeAnalysis;
using Cocona;
using Microsoft.Extensions.Logging;
using MultiSlackPost.Domains;
using ZLogger;

namespace MultiSlackPost.Commands;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("Performance", "CA1822:Mark members as static")]
[SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
public class ChannelCommand
{
    [Command("add", Description = "add channel info to config file")]
    public async Task AddChannelAsync([Option('w')] string workspace,
        [Option('c')] string channel,
        [FromService] IConfigFactory configFactory,
        [FromService] IConfigRepository configRepository,
        [FromService] ILogger<ChannelCommand> logger)
    {
        var config = await configFactory.CreateAsync();
        config.AddChannel(workspace, channel);
        await configRepository.SaveAsync(config);
        logger.ZLogInformation("Successfully added channel info to config file.");
    }

    [Command("remove", Description = "remove channel info to config file")]
    public async Task RemoveChannelAsync([Option('w')] string workspace,
        [Option('c')] string channel,
        [FromService] IConfigFactory configFactory,
        [FromService] IConfigRepository configRepository,
        [FromService] ILogger<ChannelCommand> logger)
    {
        var config = await configFactory.CreateAsync();
        config.RemoveChannel(workspace, channel);
        await configRepository.SaveAsync(config);
        logger.ZLogInformation("Successfully removed channel info in config file.");
    }
}