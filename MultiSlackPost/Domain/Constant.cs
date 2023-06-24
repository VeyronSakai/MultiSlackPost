namespace MultiSlackPost.Domain;

internal static class Constant
{
    internal static readonly string ConfigDirPath = Path.Join(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        ".config",
        "mslack");

    internal static readonly string ConfigFilePath = Path.Join(
        ConfigDirPath,
        "config.json");
}