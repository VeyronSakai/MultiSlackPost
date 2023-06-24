namespace MultiSlackPost.Domain;

public interface IConfigFactory
{
    public Task<Config> CreateAsync();
}