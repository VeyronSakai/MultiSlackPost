using Cocona;
using MultiSlackPost.Command;
using MultiSlackPost.Infrastructure;
using MultiSlackPost.Test.TestDouble;
using Config = MultiSlackPost.Domain.Config;

namespace MultiSlackPost.Test.Command;

[TestFixture]
public class TokenCommandTest
{
    [Test]
    public async Task AddTokenTest_IfConfigNotExists()
    {
        var token = new TokenCommand();
        var configRepository = new StubConfigRepository(false);
        var configFactory = new ConfigFactory(configRepository);
        await token.AddTokenAsync(
            "workspace",
            "token",
            configFactory,
            configRepository,
            Util.GetLogger<TokenCommand>()
        );
        var config = await configRepository.GetAsync();
        Assert.That(config.Tokens["workspace"], Is.EqualTo("token"));
    }

    [Test]
    public async Task AddToken_IfConfigExists()
    {
        var token = new TokenCommand();
        var configRepository = new StubConfigRepository(true);
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
            Util.GetLogger<TokenCommand>());
        var config = await configRepository.GetAsync();
        Assert.That(config?.Tokens["workspace"], Is.EqualTo("token"));
    }

    [Test]
    public Task RemoveTokenTest_IfConfigNotExists()
    {
        var token = new TokenCommand();
        var configRepository = new StubConfigRepository(false);
        var configFactory = new ConfigFactory(configRepository);
        Assert.That(async () => await token.RemoveTokenAsync(
                "workspace",
                configFactory,
                configRepository,
                Util.GetLogger<TokenCommand>()
            ),
            Throws.TypeOf<CommandExitedException>());
        return Task.CompletedTask;
    }

    [Test]
    public async Task RemoveTokenTest_IfConfigExists()
    {
        var token = new TokenCommand();
        var configRepository = new StubConfigRepository(true);
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
            Util.GetLogger<TokenCommand>()
        );
        var config = await configRepository.GetAsync();
        Assert.That(config.Tokens.ContainsKey("workspace0"), Is.False);
    }
}