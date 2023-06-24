using Cocona;
using MultiSlackPost.Domain;

namespace MultiSlackPost.Test.TestDouble;

public class SpyConfigRepository : IConfigRepository
{
    private readonly bool _existsConfig;
    internal Config? Config;

    public SpyConfigRepository(bool existsConfig)
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