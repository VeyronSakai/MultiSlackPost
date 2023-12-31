using Cocona;
using Microsoft.Extensions.Logging;
using MultiSlackPost.Domains;
using ZLogger;

namespace MultiSlackPost.UseCases;

public class PostUseCase
{
    private readonly IConfigRepository _configRepository;
    private readonly ISlackClientFactory _slackClientFactory;
    private readonly ILogger _logger;

    public PostUseCase(IConfigRepository configRepository, ISlackClientFactory slackClientFactory, ILogger logger)
    {
        _configRepository = configRepository;
        _slackClientFactory = slackClientFactory;
        _logger = logger;
    }

    public async Task PostToAllChannelsAsync(string body)
    {
        var config = await _configRepository.GetAsync();

        var tasks = new List<Task>();

        foreach (var (workspace, channels) in config.Channels)
        {
            config.Tokens.TryGetValue(workspace, out var token);
            if (token == null)
            {
                throw new CommandExitedException($"token is not registered. workspace = {workspace}", 1);
            }

            foreach (var channel in channels)
            {
                tasks.Add(PostToChannelAsync(channel, workspace, body, token));
            }
        }

        await Task.WhenAll(tasks);
    }

    private async Task PostToChannelAsync(
        string channel,
        string workspace,
        string body,
        string token)
    {
        var slackClient = _slackClientFactory.Create(token);
        var response = await slackClient.PostMessageAsync(channel, body);
        if (response.ok)
        {
            _logger.ZLogInformation($"Successfully posted message to #{channel} in workspace {workspace}");
        }
        else
        {
            throw new CommandExitedException(
                $"An error occurred when posting to #{channel} in workspace {workspace}. Error: {response.error}", 1);
        }
    }
}