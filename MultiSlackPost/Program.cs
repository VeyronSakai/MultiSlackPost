using System.Diagnostics.CodeAnalysis;
using Cocona;
using MultiSlackPost.Commands;
using Microsoft.Extensions.DependencyInjection;
using MultiSlackPost.Domain;
using MultiSlackPost.Infrastructure;

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
            .ConfigureServices(services => { services.AddTransient<IConfigRepository, ConfigRepository>(); })
            .Run<Program>(args);
    }
}