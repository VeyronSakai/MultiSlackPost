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
        [Option('c')] string channel)
    {
        Domain.Config config;
        if (File.Exists(Def.ConfigFilePath))
        {
            var oldJson = await File.ReadAllTextAsync(Def.ConfigFilePath);
            config = JsonSerializer.Deserialize<Domain.Config>(oldJson,
                         new JsonSerializerOptions { IncludeFields = true }) ??
                     throw new CommandExitedException("Deserialization resulted in null.", 1);

            config.AddChannel(workspace, channel);
        }
        else
        {
            Directory.CreateDirectory(Def.ConfigDirPath);

            config = new Domain.Config();
            config.AddToken(workspace, channel);
        }

        await WriteConfigAsync(config);

        Console.WriteLine("Successfully added channel info to config file.");
    }

    [Command("remove", Description = "remove channel info to config file")]
    public async Task RemoveChannel([Option('w')] string workspace, [Option('c')] string channel)
    {
        Domain.Config config;
        if (File.Exists(Def.ConfigFilePath))
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

        await WriteConfigAsync(config);

        Console.WriteLine("Successfully removed channel info in config file.");
    }

    private static async Task WriteConfigAsync(Domain.Config config)
    {
        var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { IncludeFields = true });
        await File.WriteAllTextAsync(Def.ConfigFilePath, json);
    }
}