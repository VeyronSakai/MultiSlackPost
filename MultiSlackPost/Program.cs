using Cocona;
using MultiSlackPost.Commands;

namespace MultiSlackPost;

[HasSubCommands(typeof(Config), "config", Description = "config set command")]
public class Program
{
    private static void Main(string[] args)
    {
        CoconaApp.Run<Program>(args);
    }
}