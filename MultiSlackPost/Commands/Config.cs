using System.Diagnostics.CodeAnalysis;
using Cocona;
using MultiSlackPost.Domain;
using Newtonsoft.Json;

namespace MultiSlackPost.Commands;

[HasSubCommands(typeof(Token), "token", Description = "token set command")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("Performance", "CA1822:Mark members as static")]
public class Config
{
    [Command("add-ch", Description = "add channel info to config file")]
    public async Task AddChannel([Option('w')] string workspace,
        [Option('c')] string channel)
    {
        Domain.Config config;
        if (File.Exists(Def.ConfigFilePath))
        {
            var oldJson = await File.ReadAllTextAsync(Def.ConfigFilePath);
            config = JsonConvert.DeserializeObject<Domain.Config>(oldJson);
            config.AddChannel(workspace, channel);
        }
        else
        {
            Directory.CreateDirectory(Def.ConfigDirPath);

            config = new Domain.Config();
            config.AddToken(workspace, channel);
        }

        // await WriteConfigAsync(config);

        Console.WriteLine("Successfully added channel info to config file.");
    }
}