using Azure.Containers.ContainerRegistry;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.ContainerRegistry;
using Azure.ResourceManager.Resources;
using Docker.DotNet;
using Docker.DotNet.Models;
using IntegrateMe.Core;

namespace IntegrateMe.Azure.ContainerRegistry;

public class ContainerRegistryAbstractStep : AbstractStep, IContainerRegistry
{
    private readonly AbstractStep _parent;
    private string? _subscriptionId;
    private string? _resourceGroup;
    private string? _repository;
    private string? _name;
    private readonly ArmClient _armClient;

    public ContainerRegistryAbstractStep(AbstractStep parent) : base(parent)
    {
        _parent = parent;
        _armClient = new(new DefaultAzureCredential());
    }

    public ContainerRegistryAbstractStep SubscriptionId(string subscriptionId)
    {
        _subscriptionId = subscriptionId;
        return this;
    }

    public ContainerRegistryAbstractStep ResourceGroup(string resourceGroup)
    {
        _resourceGroup = resourceGroup;
        return this;
    }

    public ContainerRegistryAbstractStep Name(string name)
    {
        _name = name;
        return this;
    }

    public ContainerRegistryAbstractStep Repository(string repository)
    {
        _repository = repository;
        return this;
    }

    public string GetRepository()
    {
        return $"{_name}.azurecr.io/{_repository}";
    }

    public ContainerRegistryAbstractStep Publish()
    {
        MainDsl.AddAction(async () =>
        {
            var subscription =
                _armClient.GetSubscriptionResource(new ResourceIdentifier($"/subscriptions/{_subscriptionId}"));
            ResourceGroupCollection resourceGroups = subscription.GetResourceGroups();
            ResourceGroupResource resourceGroup = await resourceGroups.GetAsync(_resourceGroup);
            ContainerRegistryResource containerRegistry = await resourceGroup.GetContainerRegistryAsync(_name);

            var loginServer = containerRegistry.Data.LoginServer;

            using var client = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();
            var localImageName = "myapp:latest";
            var acrImageName = $"{containerRegistry.Data.LoginServer}/myapp:latest";
            await client.Images.TagImageAsync(localImageName, new ImageTagParameters
            {
                RepositoryName = acrImageName,
                Tag = "latest"
            });

            var pushParameters = new ImagePushParameters
            {
                Tag = "latest"
            };

            await client.Images.PushImageAsync(acrImageName, pushParameters, new()
            {
                ServerAddress = loginServer
            }, new Progress<JSONMessage>());
        });


        return this;
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