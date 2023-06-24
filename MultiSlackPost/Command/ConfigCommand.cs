using System.Diagnostics.CodeAnalysis;
using Cocona;

namespace MultiSlackPost.Command;

[HasSubCommands(typeof(TokenCommand), "token", Description = "token set command")]
[HasSubCommands(typeof(ChannelCommand), "channel", Description = "channel set command")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("Performance", "CA1822:Mark members as static")]
public class ConfigCommand
{
}