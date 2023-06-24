using MultiSlackPost.Domain;
using SlackAPI;

namespace MultiSlackPost.Infrastructure;

public class SlackClientFactory : ISlackClientFactory
{
    public SlackTaskClient Create(string token)
    {
        return new SlackTaskClient(token);
    }
}