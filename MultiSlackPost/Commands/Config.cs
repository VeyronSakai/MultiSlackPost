using System.Diagnostics.CodeAnalysis;
using Cocona;

namespace MultiSlackPost.Commands;

[HasSubCommands(typeof(Token), "token", Description = "token set command")]
[HasSubCommands(typeof(Channel), "channel", Description = "channel set command")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("Performance", "CA1822:Mark members as static")]
public class Config
{
}