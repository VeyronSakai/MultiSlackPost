using Microsoft.Extensions.Logging;
using ZLogger;

namespace MultiSlackPost.Test;

public class Util
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