namespace MultiSlackPost.Domains;

public interface IConfigRepository
{
    Task SaveAsync(Config config);
    Task<Config> GetAsync();
    bool Exists();
}