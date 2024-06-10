using System.Diagnostics.CodeAnalysis;
using Cocona;
using Microsoft.Extensions.DependencyInjection;
using Cysharp.Text;
using Microsoft.Extensions.Logging;
using MultiSlackPost.Commands;
using MultiSlackPost.Domains;
using MultiSlackPost.Infrastructures;
using ZLogger;

namespace MultiSlackPost;

[HasSubCommands(typeof(ConfigCommand), "config", Description = "config set command")]
[HasSubCommands(typeof(PostCommand), "post", Description = "post message to channels")]
[HasSubCommands(typeof(PrintCommand), "print", Description = "print messages already posted to each channel")]
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
                    options.UsePlainTextFormatter(formatter =>
                    {
                        formatter.SetPrefixFormatter($"[{0}]: ", (in MessageTemplate template, in LogInfo info) =>
                        {
                            if (info.LogLevel != LogLevel.None)
                            {
                                template.Format(info.Category);
                            }
                        });
                    });
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