using System.Diagnostics.CodeAnalysis;
using Cocona;
using Microsoft.Extensions.Logging;
using MultiSlackPost.Domain;
using ZLogger;

namespace MultiSlackPost.Command;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("Performance", "CA1822:Mark members as static")]
[SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
public class TokenCommand
{
    [Command("add", Description = "add token info to config file")]
    public async Task AddTokenAsync([Option('w')] string workspace,
        [Option('t')] string token,
        [FromService] IConfigRepository configRepository,
        [FromService] ILogger<TokenCommand> logger)
    {
        var config = configRepository.Exists() ? await configRepository.GetAsync() : new Domain.Config();
        config.AddToken(workspace, token);
        await configRepository.SaveAsync(config);

        logger.ZLogInformation("Successfully added token info to config file.");
    }

    [Command("remove", Description = "remove token info in config file")]
    public async Task RemoveTokenAsync([Option('w')] string workspace,
        [FromService] IConfigRepository configRepository,
        [FromService] ILogger<TokenCommand> logger)
    {
        if (!configRepository.Exists())
        {
            throw new CommandExitedException("Config file does not exist.", 1);
        }

        var config = await configRepository.GetAsync();
        config.RemoveToken(workspace);
        await configRepository.SaveAsync(config);

        logger.ZLogInformation("Successfully removed token info in config file.");
    }
}