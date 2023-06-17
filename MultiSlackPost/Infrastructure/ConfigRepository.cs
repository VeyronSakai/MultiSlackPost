using System.Text.Json;
using MultiSlackPost.Domain;

namespace MultiSlackPost.Infrastructure;

public class ConfigRepository : IConfigRepository
{
    public async Task SaveAsync(Config config)
    {
        var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { IncludeFields = true });
        await File.WriteAllTextAsync(Def.ConfigFilePath, json);
    }

    public Task<Config> LoadAsync()
    {
        throw new NotImplementedException();
    }
}