using Cocona;
using MultiSlackPost.Command;
using MultiSlackPost.Infrastructure;
using MultiSlackPost.Test.TestDouble;
using Config = MultiSlackPost.Domain.Config;

namespace MultiSlackPost.Test.Command;

[TestFixture]
public class ChannelCommandTest
{
    [Test]
    public async Task AddChannelTest_IfConfigNotExists()
    {
        var channel = new ChannelCommand();
        var configRepository = new SpyConfigRepository(false);
        var configFactory = new ConfigFactory(configRepository);

        await channel.AddChannelAsync(
            "workspace",
            "channel",
            configFactory,
            configRepository,
            Helper.GetLogger<ChannelCommand>()
        );

        var config = await configRepository.GetAsync();

        Assert.That(config.Channels["workspace"], Is.EqualTo(new List<string> { "channel" }));
    }

    [Test]
    public async Task AddChannelTest_IfConfigExists()
    {
        var channel = new ChannelCommand();
        var configRepository = new SpyConfigRepository(true);
        var configFactory = new ConfigFactory(configRepository);
        await configRepository.SaveAsync(new Config
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
        });

        await channel.AddChannelAsync(
            "workspace",
            "channel",
            configFactory,
            configRepository,
            Helper.GetLogger<ChannelCommand>());

        var config = configRepository.Config;
        Assert.That(config?.Channels["workspace"], Is.EqualTo(new List<string> { "channel" }));
    }

    [Test]
    public Task RemoveChannelTest_IfConfigNotExists()
    {
        var channel = new ChannelCommand();
        var configRepository = new SpyConfigRepository(false);
        var configFactory = new ConfigFactory(configRepository);
        Assert.That(
            async () => await channel.RemoveChannelAsync(
                "workspace",
                "channel",
                configFactory,
                configRepository,
                Helper.GetLogger<ChannelCommand>()),
            Throws.TypeOf<CommandExitedException>());
        return Task.CompletedTask;
    }
}