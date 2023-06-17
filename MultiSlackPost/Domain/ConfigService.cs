namespace MultiSlackPost.Domain;

public static class ConfigService
{
    public static bool Exists()
    {
        return File.Exists(Def.ConfigFilePath);
    }
}