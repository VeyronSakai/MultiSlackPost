using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Cocona;
using MultiSlackPost.Domain;

namespace MultiSlackPost.Commands;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("Performance", "CA1822:Mark members as static")]
public class Token
{
    [Command("add", Description = "add token info to config file")]
    public async Task Add([Option('w')] string workspace,
        [Option('t')] string token,
        [FromService] IConfigRepository configRepository)
    {
        Domain.Config config;
        if (File.Exists(Def.ConfigFilePath))
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

        config.AddToken(workspace, token);
        await configRepository.SaveAsync(config);

        Console.WriteLine("Successfully added token info to config file.");
    }

    [Command("remove", Description = "remove token info in config file")]
    public async Task RemoveToken([Option('w')] string workspace, [FromService] IConfigRepository configRepository)
    {
        Domain.Config config;
        if (File.Exists(Def.ConfigFilePath))
        {
            var oldJson = await File.ReadAllTextAsync(Def.ConfigFilePath);
            config = JsonSerializer.Deserialize<Domain.Config>(oldJson,
                         new JsonSerializerOptions { IncludeFields = true }) ??
                     throw new CommandExitedException("Deserialization resulted in null.", 1);

            if (config.Tokens.ContainsKey(workspace))
            {
                config.RemoveToken(workspace);
            }
            else
            {
                Console.WriteLine("Config file does not contain the workspace.");
                throw new CommandExitedException(1);
            }
        }
        else
        {
            Console.WriteLine("Config file does not exist.");
            throw new CommandExitedException(1);
        }

        await configRepository.SaveAsync(config);

        Console.WriteLine("Successfully removed token info in config file.");
    }
}