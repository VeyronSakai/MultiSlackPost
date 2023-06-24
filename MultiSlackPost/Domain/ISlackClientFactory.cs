using SlackAPI;

namespace MultiSlackPost.Domain;

public interface ISlackClientFactory
{
    public SlackTaskClient Create(string token);
}