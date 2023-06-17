using System.Diagnostics.CodeAnalysis;
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
        var config = ConfigService.Exists() ? await configRepository.GetAsync() : new Domain.Config();
        config.AddToken(workspace, token);
        await configRepository.SaveAsync(config);

        Console.WriteLine("Successfully added token info to config file.");
    }

    [Command("remove", Description = "remove token info in config file")]
    public async Task RemoveToken([Option('w')] string workspace, [FromService] IConfigRepository configRepository)
    {
        if (!ConfigService.Exists())
        {
            throw new CommandExitedException("Config file does not exist.", 1);
        }

        var config = await configRepository.GetAsync();
        config.RemoveToken(workspace);
        await configRepository.SaveAsync(config);

        Console.WriteLine("Successfully removed token info in config file.");
    }
}