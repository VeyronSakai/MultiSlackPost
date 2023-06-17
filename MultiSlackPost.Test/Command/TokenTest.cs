using MultiSlackPost.Commands;

namespace MultiSlackPost.Test.Command;

[TestFixture]
public class TokenTest
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
}