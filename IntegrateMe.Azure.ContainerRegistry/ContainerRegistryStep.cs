using Azure.Containers.ContainerRegistry;
using Azure.Identity;
using IntegrateMe.Core;

namespace IntegrateMe.Azure.ContainerRegistry;

public class ContainerRegistryStep : IStep, IContainerRegistry
{
    private readonly IStep _parent;
    private string? _repository;
    private string? _name;
    private readonly List<Func<Task>> _actions = [];

    public ContainerRegistryStep(IStep parent)
    {
        _parent = parent;
    }

    public Dsl MainDsl => _parent.MainDsl;

    public IStep When()
    {
        return this;
    }

    public IStep Then()
    {
        return this;
    }

    public ContainerRegistryStep Name(string name)
    {
        _name = name;
        return this;
    }

    public ContainerRegistryStep Repository(string repository)
    {
        _repository = repository;
        return this;
    }

    public async Task RunAsync()
    {
        foreach (var action in _actions)
        {
            await action.Invoke();
        }

        await _parent.RunAsync();
    }

    public async Task SetupAsync()
    {
        await _parent.SetupAsync();
    }

    public async Task TearDownAsync()
    {
        await _parent.TearDownAsync();
    }

    public T Get<T>(string key)
    {
        return MainDsl.Get<T>(key);
    }

    public string GetRepository()
    {
        return $"{_name}.azurecr.io/{_repository}";
    }

    public List<string> ListTags()
    {
        var endpoint = new Uri($"https://{_name}.azurecr.io");
        var client = new ContainerRegistryClient(endpoint, new DefaultAzureCredential());

        var repository = client.GetRepository(_repository);
        var manifests = repository.GetAllManifestProperties();
        return manifests.SelectMany(manifest => manifest.Tags).ToList();
    }

    public string LatestTag()
    {
        var endpoint = new Uri($"https://{_name}.azurecr.io");
        var client = new ContainerRegistryClient(endpoint, new DefaultAzureCredential());

        var repository = client.GetRepository(_repository);
        var manifests = repository.GetAllManifestProperties();
        var tags = manifests.SelectMany(manifest => manifest.Tags).ToList();
        return tags.OrderDescending().First();
    }
}