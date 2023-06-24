using SlackAPI;

namespace MultiSlackPost.Domains;

public interface ISlackClientFactory
{
    public SlackTaskClient Create(string token);
}