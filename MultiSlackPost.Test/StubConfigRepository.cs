using Cocona;
using MultiSlackPost.Domain;

namespace MultiSlackPost.Test;

public class StubConfigRepository : IConfigRepository
{
    private readonly bool _existsConfig;
    internal Config? Config;

    public StubConfigRepository(bool existsConfig)
    {
        _existsConfig = existsConfig;
    }

    public Task SaveAsync(Config config)
    {
        Config = config;
        return Task.CompletedTask;
    }

    public Task<Config> GetAsync()
    {
        if (Config == null)
        {
            throw new CommandExitedException("Config is null.", 1);
        }

        return Task.FromResult(Config);
    }

    public bool Exists()
    {
        return _existsConfig;
    }
}