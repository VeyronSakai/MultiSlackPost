using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Cocona;
using MultiSlackPost.Domain;

namespace MultiSlackPost.Commands;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("Performance", "CA1822:Mark members as static")]
public class Channel
{
    [Command("add", Description = "add channel info to config file")]
    public async Task AddChannel([Option('w')] string workspace,
        [Option('c')] string channel,
        [FromService] IConfigRepository configRepository)
    {
        Domain.Config config;
        if (ConfigService.Exists())
        {
            var oldJson = await File.ReadAllTextAsync(Def.ConfigFilePath);
            config = JsonSerializer.Deserialize<Domain.Config>(oldJson,
                         new JsonSerializerOptions { IncludeFields = true }) ??
                     throw new CommandExitedException("Deserialization resulted in null.", 1);
        }
        else
        {
            config = new Domain.Config();
        }

        config.AddChannel(workspace, channel);
        await configRepository.SaveAsync(config);

        Console.WriteLine("Successfully added channel info to config file.");
    }

    [Command("remove", Description = "remove channel info to config file")]
    public async Task RemoveChannel([Option('w')] string workspace,
        [Option('c')] string channel,
        [FromService] IConfigRepository configRepository)
    {
        Domain.Config config;
        if (ConfigService.Exists())
        {
            var oldJson = await File.ReadAllTextAsync(Def.ConfigFilePath);
            config = JsonSerializer.Deserialize<Domain.Config>(oldJson,
                         new JsonSerializerOptions { IncludeFields = true }) ??
                     throw new CommandExitedException("Deserialization resulted in null.", 1);

            config.RemoveChannel(workspace, channel);
        }
        else
        {
            Console.WriteLine("Config file does not exist.");
            throw new CommandExitedException(1);
        }

        await configRepository.SaveAsync(config);

        Console.WriteLine("Successfully removed channel info in config file.");
    }
}