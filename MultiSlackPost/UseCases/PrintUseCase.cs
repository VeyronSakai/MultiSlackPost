using Cocona;
using Microsoft.Extensions.Logging;
using MultiSlackPost.Domains;
using ZLogger;

namespace MultiSlackPost.UseCases;

public class PrintUseCase
{
    private readonly IConfigRepository _configRepository;
    private readonly ISlackClientFactory _slackClientFactory;
    private readonly ILogger _logger;

    public PrintUseCase(IConfigRepository configRepository, ISlackClientFactory clientFactory, ILogger logger)
    {
        _configRepository = configRepository;
        _slackClientFactory = clientFactory;
        _logger = logger;
    }

    public async Task PrintAsync()
    {
        var config = await _configRepository.GetAsync();

        foreach (var (workspace, channels) in config.Channels)
        {
            config.Tokens.TryGetValue(workspace, out var token);
            if (token == null)
            {
                throw new CommandExitedException($"token is not registered. workspace = {workspace}", 1);
            }

            foreach (var channelName in channels)
            {
                var slackClient = _slackClientFactory.Create(token);
                var channelsResponse = await slackClient.GetConversationsListAsync();
                var channel = channelsResponse.channels.FirstOrDefault(c => c.name == channelName);
                if (channel == null)
                {
                    throw new CommandExitedException($"channel is not found. channel = {channelName}", 1);
                }

                var historyResponse = await slackClient.GetConversationsHistoryAsync(channel);
                var latestMessage = historyResponse.messages.FirstOrDefault();
                if (latestMessage == null)
                {
                    throw new CommandExitedException("message is not found", 1);
                }

                _logger.ZLog(LogLevel.None, $"{latestMessage.text} in #{channelName} in {workspace}");
            }
        }
    }
}