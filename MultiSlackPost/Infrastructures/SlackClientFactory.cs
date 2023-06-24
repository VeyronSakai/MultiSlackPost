using MultiSlackPost.Domains;
using SlackAPI;

namespace MultiSlackPost.Infrastructures;

public class SlackClientFactory : ISlackClientFactory
{
    public SlackTaskClient Create(string token)
    {
        return new SlackTaskClient(token);
    }
}