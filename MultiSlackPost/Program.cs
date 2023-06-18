using System.Diagnostics.CodeAnalysis;
using Cocona;
using Cysharp.Text;
using MultiSlackPost.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MultiSlackPost.Domain;
using MultiSlackPost.Infrastructure;
using ZLogger;

namespace MultiSlackPost;

[HasSubCommands(typeof(Commands.Config), "config", Description = "config set command")]
[HasSubCommands(typeof(Post), "post", Description = "post message command")]
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
                    options.PrefixFormatter = (writer, info) => prefixFormat.FormatTo(ref writer, info.LogLevel);
                });
            })
            .ConfigureServices(services => { services.AddTransient<IConfigRepository, ConfigRepository>(); })
            .Run<Program>(args);
    }
}