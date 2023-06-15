namespace MultiSlackPost.Domain;

public class Config
{
    public Dictionary<string, List<string>> Channels = new();

    public Dictionary<string, string> Tokens = new();

    internal void AddChannel(string workspace, string channel)
    {
        if (Channels.TryGetValue(workspace, out var channels))
        {
            channels.Add(channel);
        }
        else
        {
            Channels.Add(workspace, new List<string> { channel });
        }
    }

    internal void AddToken(string workspace, string token)
    {
        Tokens.TryAdd(workspace, token);
    }

    internal void RemoveToken(string workspace)
    {
        Tokens.Remove(workspace);
    }
}