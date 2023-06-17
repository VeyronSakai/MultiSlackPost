namespace MultiSlackPost.Domain;

public interface IConfigRepository
{
    Task SaveAsync(Config config);
    Task<Config> LoadAsync();
}