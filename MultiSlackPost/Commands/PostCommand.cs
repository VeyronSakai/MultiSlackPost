using System.Diagnostics.CodeAnalysis;
using Cocona;
using Microsoft.Extensions.Logging;
using MultiSlackPost.Domains;
using MultiSlackPost.UseCases;

namespace MultiSlackPost.Commands;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("Performance", "CA1822:Mark members as static")]
public class PostCommand
{
    [Command(Description = "post message to channels")]
    public async Task PostAsync([Argument(Description = "body message")] string body,
        [FromService] IConfigRepository configRepository,
        [FromService] ISlackClientFactory slackClientFactory,
        [FromService] ILogger<ChannelCommand> logger)
    {
        var useCase = new PostUseCase(configRepository, slackClientFactory, logger);
        await useCase.PostToAllChannelsAsync(body);
    }
}