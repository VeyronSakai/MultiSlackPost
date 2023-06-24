using Cocona;

namespace MultiSlackPost.Domain;

public class Config
{
    public Dictionary<string, List<string>> Channels = new();

    public Dictionary<string, string> Tokens = new();

    public void AddChannel(string workspace, string channel)
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

    public void RemoveChannel(string workspace, string channel)
    {
        Channels.TryGetValue(workspace, out var channels);
        if (channels == null)
        {
            throw new CommandExitedException(1);
        }

        if (channels.Contains(channel))
        {
            channels.Remove(channel);
        }

        if (channels.Count == 0)
        {
            Channels.Remove(workspace);
        }
    }

    public void AddToken(string workspace, string token)
    {
        Tokens[workspace] = token;
    }

    public void RemoveToken(string workspace)
    {
        if (!Tokens.ContainsKey(workspace))
        {
            throw new CommandExitedException("Config file does not contain the workspace in Tokens.", 1);
        }

        Tokens.Remove(workspace);
    }
}