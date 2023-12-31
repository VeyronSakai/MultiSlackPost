using MultiSlackPost.Domains;

namespace MultiSlackPost.Infrastructures;

public class ConfigFactory : IConfigFactory
{
    private readonly IConfigRepository _configRepository;

    public ConfigFactory(IConfigRepository configRepository)
    {
        _configRepository = configRepository;
    }

    public async Task<Config> CreateAsync()
    {
        var config = _configRepository.Exists()
            ? await _configRepository.GetAsync()
            : new Config();

        return config;
    }
}