using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Cocona;
using MultiSlackPost.Domain;
using Newtonsoft.Json;

namespace MultiSlackPost.Commands;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("Performance", "CA1822:Mark members as static")]
public class Config
{
    [Command("add-token", Description = "add token info to config file")]
    public async Task AddToken([Option('w')] string workspace,
        [Option('t')] string token)
    {
        Domain.Config config;
        if (File.Exists(Def.ConfigFilePath))
        {
            var oldJson = await File.ReadAllTextAsync(Def.ConfigFilePath);
            config = JsonConvert.DeserializeObject<Domain.Config>(oldJson);
            config.AddToken(workspace, token);
        }
        else
        {
            var configDirPath = Def.ConfigDirPath;
            Directory.CreateDirectory(configDirPath);

            config = new Domain.Config();
            config.AddToken(workspace, token);
        }

        await WriteConfigAsync(config);

        Console.WriteLine("Successfully added token info to config file.");
    }

    private static async Task WriteConfigAsync(Domain.Config config)
    {
        var options = new JsonSerializerOptions
        {
            IncludeFields = true,
        };

        var json = System.Text.Json.JsonSerializer.Serialize(config, options);
        await File.WriteAllTextAsync(Def.ConfigFilePath, json);
    }
}