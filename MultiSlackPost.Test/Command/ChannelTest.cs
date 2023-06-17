using Cocona;
using MultiSlackPost.Commands;
using Config = MultiSlackPost.Domain.Config;

namespace MultiSlackPost.Test.Command;

[TestFixture]
public class ChannelTest
{
    [Test]
    public async Task AddChannelTest_IfConfigNotExists()
    {
        var channel = new Channel();
        var configRepository = new StubConfigRepository(false);
        await channel.AddChannelAsync("workspace", "channel", configRepository);
        var config = configRepository.Config;
        Assert.That(config?.Channels["workspace"], Is.EqualTo(new List<string> { "channel" }));
    }

    [Test]
    public async Task AddChannelTest_IfConfigExists()
    {
        var channel = new Channel();
        var configRepository = new StubConfigRepository(true);
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

        await channel.AddChannelAsync("workspace", "channel", configRepository);
        var config = configRepository.Config;
        Assert.That(config?.Channels["workspace"], Is.EqualTo(new List<string> { "channel" }));
    }

    [Test]
    public Task RemoveChannelTest_IfConfigNExists()
    {
        var channel = new Channel();
        var configRepository = new StubConfigRepository(false);
        Assert.That(async () => await channel.RemoveChannel("workspace", "channel", configRepository),
            Throws.TypeOf<CommandExitedException>());
        return Task.CompletedTask;
    }
}