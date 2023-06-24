using System.Text.Json;
using Cocona;
using MultiSlackPost.Domains;

namespace MultiSlackPost.Infrastructures;

public class ConfigRepository : IConfigRepository
{
    public async Task SaveAsync(Config config)
    {
        Directory.CreateDirectory(Constant.ConfigDirPath);
        var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { IncludeFields = true });
        await File.WriteAllTextAsync(Constant.ConfigFilePath, json);
    }

    public async Task<Config> GetAsync()
    {
        if (!Exists())
        {
            throw new CommandExitedException("Config file does not exist.", 1);
        }

        var configJson = await File.ReadAllTextAsync(Constant.ConfigFilePath);
        var config = JsonSerializer.Deserialize<Config>(configJson, new JsonSerializerOptions { IncludeFields = true });
        if (config == null)
        {
            throw new CommandExitedException("Deserialization result is null.", 1);
        }

        return config;
    }

    public bool Exists()
    {
        return File.Exists(Constant.ConfigFilePath);
    }
}