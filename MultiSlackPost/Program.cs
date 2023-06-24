using System.Diagnostics.CodeAnalysis;
using Cocona;
using Cysharp.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MultiSlackPost.Commands;
using MultiSlackPost.Domains;
using MultiSlackPost.Infrastructures;
using ZLogger;

namespace MultiSlackPost;

[HasSubCommands(typeof(ConfigCommand), "config", Description = "config set command")]
[HasSubCommands(typeof(PostCommand), "post", Description = "post message command")]
[HasSubCommands(typeof(PrintCommand), "print", Description = "print latest message user posted")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("Performance", "CA1822:Mark members as static")]
public class Program
{
    private static void Main(string[] args)
    {
        CoconaApp
            .CreateHostBuilder()
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddZLoggerConsole(options =>
                {
                    var prefixFormat = ZString.PrepareUtf8<LogLevel>("[{0}]: ");
                    options.PrefixFormatter = (writer, info) =>
                    {
                        if (info.LogLevel != LogLevel.None)
                        {
                            prefixFormat.FormatTo(ref writer, info.LogLevel);
                        }
                    };
                });
            })
            .ConfigureServices(services =>
            {
                services.AddSingleton<IConfigRepository, ConfigRepository>();
                services.AddSingleton<IConfigFactory, ConfigFactory>();
                services.AddSingleton<ISlackClientFactory, SlackClientFactory>();
            })
            .Run<Program>(args);
    }
}