namespace MultiSlackPost.Domain;

internal static class Def
{
    internal static readonly string ConfigDirPath = Path.Join(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        ".config",
        "multi-slack-post");

    internal static readonly string ConfigFilePath = Path.Join(
        ConfigDirPath,
        "config.json");
}