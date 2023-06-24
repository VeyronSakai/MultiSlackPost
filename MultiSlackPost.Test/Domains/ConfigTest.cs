using MultiSlackPost.Domains;

namespace MultiSlackPost.Test.Domains;

[TestFixture]
public class ConfigTest
{
    [Test]
    public void AddChannelTest()
    {
        var config = new Config();
        config.AddChannel("workspace", "channel");
        Assert.That(config.Channels["workspace"], Is.EqualTo(new List<string> { "channel" }));
    }

    [Test]
    public void RemoveChannelTest()
    {
        var config = new Config();
        config.AddChannel("workspace", "channel");
        config.RemoveChannel("workspace", "channel");
        Assert.That(config.Channels, Is.Empty);
    }

    [Test]
    public void AddTokenTest()
    {
        var config = new Config();
        config.AddToken("workspace", "token");
        Assert.That(config.Tokens["workspace"], Is.EqualTo("token"));
    }

    [Test]
    public void RemoveTokenTest()
    {
        var config = new Config();
        config.AddToken("workspace", "token");
        config.RemoveToken("workspace");
        Assert.That(config.Tokens, Is.Empty);
    }
}