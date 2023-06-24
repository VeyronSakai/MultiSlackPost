using System.Text.Json;
using Cocona;
using MultiSlackPost.Domain;

namespace MultiSlackPost.Infrastructure;

public class ConfigRepository : IConfigRepository
{
    public async Task SaveAsync(Config config)
    {
        Directory.CreateDirectory(Def.ConfigDirPath);
        var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { IncludeFields = true });
        await File.WriteAllTextAsync(Def.ConfigFilePath, json);
    }

    public async Task<Config> GetAsync()
    {
        if (!Exists())
        {
            throw new CommandExitedException("Config file does not exist.", 1);
        }

        var oldJson = await File.ReadAllTextAsync(Def.ConfigFilePath);
        return JsonSerializer.Deserialize<Config>(oldJson,
                   new JsonSerializerOptions { IncludeFields = true }) ??
               throw new CommandExitedException("Deserialization resulted in null.", 1);
    }

    public bool Exists()
    {
        return File.Exists(Def.ConfigFilePath);
    }
}