using Cocona;

namespace MultiSlackPost.Domain;

public class Config
{
    public Dictionary<string, List<string>> Channels = new();

    public Dictionary<string, string> Tokens = new();

    internal void AddChannel(string workspace, string channel)
    {
        if (channel[0] == '#')
        {
            channel = channel[1..];
        }

        if (Channels.TryGetValue(workspace, out var channels))
        {
            if (!channels.Contains(channel))
            {
                channels.Add(channel);
            }
        }
        else
        {
            Channels.Add(workspace, new List<string> { channel });
        }
    }

    internal void RemoveChannel(string workspace, string channel)
    {
        Channels.TryGetValue(workspace, out var channels);
        if (channels != null && channels.Contains(channel))
        {
            channels.Remove(channel);
        }

        if (channels == null || channels.Count == 0)
        {
            Channels.Remove(workspace);
        }
    }

    internal void AddToken(string workspace, string token)
    {
        Tokens[workspace] = token;
    }

    internal void RemoveToken(string workspace)
    {
        if (!Tokens.ContainsKey(workspace))
        {
            throw new CommandExitedException("Config file does not contain the workspace in Tokens.", 1);
        }

        Tokens.Remove(workspace);
    }
}