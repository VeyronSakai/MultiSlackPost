using System.Diagnostics.CodeAnalysis;
using Cocona;
using MultiSlackPost.Domain;

namespace MultiSlackPost.Commands;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("Performance", "CA1822:Mark members as static")]
[SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
public class Channel
{
    [Command("add", Description = "add channel info to config file")]
    public async Task AddChannelAsync([Option('w')] string workspace,
        [Option('c')] string channel,
        [FromService] IConfigRepository configRepository)
    {
        var config = configRepository.Exists() ? await configRepository.GetAsync() : new Domain.Config();
        config.AddChannel(workspace, channel);
        await configRepository.SaveAsync(config);

        Console.WriteLine("Successfully added channel info to config file.");
    }

    [Command("remove", Description = "remove channel info to config file")]
    public async Task RemoveChannel([Option('w')] string workspace,
        [Option('c')] string channel,
        [FromService] IConfigRepository configRepository)
    {
        var config = configRepository.Exists()
            ? await configRepository.GetAsync()
            : throw new CommandExitedException("Config file does not exist.", 1);

        config.RemoveChannel(workspace, channel);

        await configRepository.SaveAsync(config);

        Console.WriteLine("Successfully removed channel info in config file.");
    }
}