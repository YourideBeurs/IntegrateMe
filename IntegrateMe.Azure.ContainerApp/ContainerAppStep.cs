using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.AppContainers;
using Azure.ResourceManager.Resources;
using IntegrateMe.Core;

namespace IntegrateMe.Azure.ContainerApp;

public class ContainerAppStep(AbstractStep parent) : AbstractStep(parent)
{
    private readonly ArmClient _armClient = new(new DefaultAzureCredential());
    private readonly Dictionary<string, string> _secrets = new();
    private string? _suffix;
    private string? _tag;
    private string? _subscriptionId;
    private string? _repository;
    private string? _resourceGroup;
    private string? _name;

    public ContainerAppStep RandomSuffix()
    {
        var rand = new Random();

        var stringlen = rand.Next(4, 10);
        var suffix = "";
        for (var i = 0; i < stringlen; i++)
        {
            // Generating a random number. 
            var randValue = rand.Next(0, 26);

            // Generating random character by converting 
            // the random number into character. 
            var letter = Convert.ToChar(randValue + 65);

            // Appending the letter to string. 
            suffix += letter;
        }

        _suffix = suffix.ToLower();
        if (MainDsl.Verbose)
        {
            Console.WriteLine($"[{DateTime.Now}] Random suffix being set to: {_suffix}");
        }

        return this;
    }

    public ContainerAppStep Tag(string tag)
    {
        if (MainDsl.Verbose)
        {
            Console.WriteLine($"[{DateTime.Now}] Tag being set to: {tag}");
        }

        _tag = tag;
        return this;
    }

    public ContainerAppStep Tag(Func<AbstractStep, string> action)
    {
        if (MainDsl.Verbose)
        {
            Console.WriteLine($"[{DateTime.Now}] Tag retrieved from previous step");
        }

        _tag = action.Invoke(this);

        return this;
    }

    public ContainerAppStep SubscriptionId(string subscriptionId)
    {
        if (MainDsl.Verbose)
        {
            Console.WriteLine($"[{DateTime.Now}] SubscriptionId being set to: {subscriptionId}");
        }

        _subscriptionId = subscriptionId;
        return this;
    }

    public ContainerAppStep Secret(string name, string value)
    {
        if (MainDsl.Verbose)
        {
            Console.WriteLine($"[{DateTime.Now}] Secret with name {name} added");
        }

        _secrets.Add(name, value);
        return this;
    }

    public ContainerAppStep ResourceGroup(string resourceGroup)
    {
        if (MainDsl.Verbose)
        {
            Console.WriteLine($"[{DateTime.Now}] ResourceGroup being set to: {resourceGroup}");
        }

        _resourceGroup = resourceGroup;

        return this;
    }


    public ContainerAppStep Name(string name)
    {
        if (MainDsl.Verbose)
        {
            Console.WriteLine($"[{DateTime.Now}] Name being set to: {name}");
        }

        _name = name;

        return this;
    }

    public ContainerAppStep CreateNewRevision()
    {
        MainDsl.AddAction(async () =>
        {
            ValidateStep();

            var subscription =
                _armClient.GetSubscriptionResource(new ResourceIdentifier($"/subscriptions/{_subscriptionId}"));
            ResourceGroupCollection resourceGroups = subscription.GetResourceGroups();
            ResourceGroupResource resourceGroup = await resourceGroups.GetAsync(_resourceGroup);
            ContainerAppResource containerApp = await resourceGroup.GetContainerAppAsync(_name);

            var currentTemplate = containerApp.Data.Template;
            var currentConfig = containerApp.Data.Configuration;

            currentTemplate.Containers[0].Image = $"{_repository}:{_tag}"; // New image
            currentTemplate.RevisionSuffix = _suffix; // New revision suffix

            foreach (var (key, value) in _secrets)
            {
                var secret = currentConfig.Secrets.FirstOrDefault(x => x.Name.Equals(key));
                if (secret is not null)
                {
                    secret.Value = value;
                }
                else
                {
                    currentConfig.Secrets.Add(new()
                    {
                        Name = key,
                        Value = value
                    });
                }
            }

            if (MainDsl.Verbose)
            {
                Console.WriteLine($"[{DateTime.Now}] Creating new revision");
            }

            var updateOperation = await containerApp.UpdateAsync(WaitUntil.Completed, containerApp.Data);

            if (MainDsl.Verbose)
            {
                Console.WriteLine($"[{DateTime.Now}] New revision created successfully");
            }
        });

        return this;
    }

    public ContainerAppStep Start()
    {
        MainDsl.AddAction(async () =>
        {
            if (MainDsl.Verbose)
            {
                Console.WriteLine($"[{DateTime.Now}] Starting container app");
            }


            var subscription =
                _armClient.GetSubscriptionResource(new ResourceIdentifier($"/subscriptions/{_subscriptionId}"));
            ResourceGroupCollection resourceGroups = subscription.GetResourceGroups();
            ResourceGroupResource resourceGroup = await resourceGroups.GetAsync(_resourceGroup);
            ContainerAppResource containerApp = await resourceGroup.GetContainerAppAsync(_name);

            await containerApp.StartAsync(WaitUntil.Completed);


            if (MainDsl.Verbose)
            {
                Console.WriteLine($"[{DateTime.Now}] Container app started");
            }
        });


        return this;
    }

    public ContainerAppStep Stop()
    {
        MainDsl.AddAction(async () =>
        {
            if (MainDsl.Verbose)
            {
                Console.WriteLine($"[{DateTime.Now}] Stopping container app");
            }

            var subscription =
                _armClient.GetSubscriptionResource(new ResourceIdentifier($"/subscriptions/{_subscriptionId}"));
            ResourceGroupCollection resourceGroups = subscription.GetResourceGroups();
            ResourceGroupResource resourceGroup = await resourceGroups.GetAsync(_resourceGroup);
            ContainerAppResource containerApp = await resourceGroup.GetContainerAppAsync(_name);

            await containerApp.StopAsync(WaitUntil.Completed);

            if (MainDsl.Verbose)
            {
                Console.WriteLine($"[{DateTime.Now}] Container app stopped");
            }
        });


        return this;
    }

    public ContainerAppStep Call(Func<string, Task> action)
    {
        MainDsl.AddAction(async () =>
        {
            var subscription =
                _armClient.GetSubscriptionResource(new ResourceIdentifier($"/subscriptions/{_subscriptionId}"));
            ResourceGroupCollection resourceGroups = subscription.GetResourceGroups();
            ResourceGroupResource resourceGroup = await resourceGroups.GetAsync(_resourceGroup);
            ContainerAppResource containerApp = await resourceGroup.GetContainerAppAsync(_name);

            var ingress = containerApp.Data.Configuration.Ingress;

            if (ingress != null && !string.IsNullOrEmpty(ingress.Fqdn))
            {
                var url = $"https://{ingress.Fqdn}";
                await action.Invoke(url);
            }
            else
            {
                throw new Exception("Ingress not found");
            }
        });

        return this;
    }

    public ContainerAppStep Repository(string repository)
    {
        if (MainDsl.Verbose)
        {
            Console.WriteLine($"[{DateTime.Now}] Repository being set to: {repository}");
        }

        _repository = repository;
        return this;
    }

    public ContainerAppStep Repository(Func<AbstractStep, IContainerRegistry> action)
    {
        if (MainDsl.Verbose)
        {
            Console.WriteLine($"[{DateTime.Now}] Repository retrieved from container registry");
        }

        _repository = action.Invoke(this).GetRepository();
        return this;
    }

    private void ValidateStep()
    {
        if (_subscriptionId is null)
        {
            throw new ArgumentException("SubscriptionId is missing");
        }

        if (_repository is null)
        {
            throw new ArgumentException("Repository is missing");
        }

        if (_suffix is null)
        {
            throw new ArgumentException("Suffix is missing");
        }

        if (_tag is null)
        {
            throw new ArgumentException("Tag is missing");
        }
    }
}