namespace MultiSlackPost.Domains;

public interface IConfigFactory
{
    public Task<Config> CreateAsync();
}