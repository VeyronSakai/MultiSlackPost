using Microsoft.Extensions.Logging;
using ZLogger;

namespace MultiSlackPost.Test;

public static class Helper
{
    public static ILogger<T> GetLogger<T>()
    {
        var factory = LoggerFactory.Create(x =>
        {
            x.ClearProviders();
            x.AddZLoggerConsole();
        });
        return factory.CreateLogger<T>();
    }
}