using System.Diagnostics.CodeAnalysis;
using Cocona;
using MultiSlackPost.Commands;

namespace MultiSlackPost;

[HasSubCommands(typeof(Config), "config", Description = "config set command")]
[HasSubCommands(typeof(Post), "post", Description = "post message command")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("Performance", "CA1822:Mark members as static")]
public class Program
{
    private static void Main(string[] args)
    {
        CoconaApp.Run<Program>(args);
    }
}