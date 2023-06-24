using Cocona;
using MultiSlackPost.Commands;
using MultiSlackPost.Infrastructures;
using MultiSlackPost.Test.TestDoubles;
using Config = MultiSlackPost.Domains.Config;

namespace MultiSlackPost.Test.Commands;

[TestFixture]
public class TokenCommandTest
{
    [Test]
    public async Task AddTokenTest_IfConfigNotExists()
    {
        var token = new TokenCommand();
        var configRepository = new SpyConfigRepository(false);
        var configFactory = new ConfigFactory(configRepository);
        await token.AddTokenAsync(
            "workspace",
            "token",
            configFactory,
            configRepository,
            Helper.GetLogger<TokenCommand>()
        );
        var config = await configRepository.GetAsync();
        Assert.That(config.Tokens["workspace"], Is.EqualTo("token"));
    }

    [Test]
    public async Task AddToken_IfConfigExists()
    {
        var token = new TokenCommand();
        var configRepository = new SpyConfigRepository(true);
        var configFactory = new ConfigFactory(configRepository);
        await configRepository.SaveAsync(new Config()
        {
            Channels = new Dictionary<string, List<string>>
            {
                {
                    "workspace0", new List<string>
                    {
                        "channel0"
                    }
                }
            },
            Tokens = new Dictionary<string, string>
            {
                {
                    "workspace0", "token0"
                }
            }
        });
        await token.AddTokenAsync("workspace", "token", configFactory, configRepository,
            Helper.GetLogger<TokenCommand>());
        var config = await configRepository.GetAsync();
        Assert.That(config?.Tokens["workspace"], Is.EqualTo("token"));
    }

    [Test]
    public Task RemoveTokenTest_IfConfigNotExists()
    {
        var token = new TokenCommand();
        var configRepository = new SpyConfigRepository(false);
        var configFactory = new ConfigFactory(configRepository);
        Assert.That(async () => await token.RemoveTokenAsync(
                "workspace",
                configFactory,
                configRepository,
                Helper.GetLogger<TokenCommand>()
            ),
            Throws.TypeOf<CommandExitedException>());
        return Task.CompletedTask;
    }

    [Test]
    public async Task RemoveTokenTest_IfConfigExists()
    {
        var token = new TokenCommand();
        var configRepository = new SpyConfigRepository(true);
        var configFactory = new ConfigFactory(configRepository);
        await configRepository.SaveAsync(new Config()
        {
            Channels = new Dictionary<string, List<string>>
            {
                {
                    "workspace0", new List<string>
                    {
                        "channel0"
                    }
                }
            },
            Tokens = new Dictionary<string, string>
            {
                {
                    "workspace0", "token0"
                }
            }
        });

        await token.RemoveTokenAsync(
            "workspace0",
            configFactory,
            configRepository,
            Helper.GetLogger<TokenCommand>()
        );
        var config = await configRepository.GetAsync();
        Assert.That(config.Tokens.ContainsKey("workspace0"), Is.False);
    }
}