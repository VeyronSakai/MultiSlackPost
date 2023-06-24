using System.Diagnostics.CodeAnalysis;
using Cocona;
using Microsoft.Extensions.Logging;
using MultiSlackPost.Domain;
using MultiSlackPost.UseCase;

namespace MultiSlackPost.Command;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("Performance", "CA1822:Mark members as static")]
public class PostCommand
{
    [Command(Description = "post message to channels")]
    public async Task PostAsync([Argument(Description = "body message")] string body,
        [FromService] IConfigRepository configRepository,
        [FromService] ILogger<ChannelCommand> logger)
    {
        var useCase = new PostUseCase(configRepository, logger);
        await useCase.PostToAllChannelsAsync(body);
    }
}