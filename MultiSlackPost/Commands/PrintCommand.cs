using System.Diagnostics.CodeAnalysis;
using Cocona;
using Microsoft.Extensions.Logging;
using MultiSlackPost.Domains;
using MultiSlackPost.UseCases;

namespace MultiSlackPost.Commands;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("Performance", "CA1822:Mark members as static")]
public class PrintCommand
{
    [Command(Description = "Print latest message which user posted to channels")]
    public async Task PrintAsync(
        [FromService] IConfigRepository configRepository,
        [FromService] ISlackClientFactory slackClientFactory,
        [FromService] ILogger<PrintCommand> logger)
    {
        var useCase = new PrintUseCase(configRepository, slackClientFactory, logger);
        await useCase.PrintAsync();
    }
}