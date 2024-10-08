using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Logic;
using Azure.ResourceManager.Logic.Models;
using Azure.ResourceManager.Resources;
using IntegrateMe.Core;

namespace IntegrateMe.Azure.LogicApp;

public class LogicAppAbstractStep : AbstractStep
{
    private readonly ArmClient _armClient;
    private readonly AbstractStep _parent;
    private string? _resourceGroup;
    private string? _name;
    private string? _subscriptionId;
    private int _runCount;
    private bool _prepared;

    public LogicAppAbstractStep(AbstractStep parent) : base(parent)
    {
        _parent = parent;
        _armClient = new(new DefaultAzureCredential());
    }

    public LogicAppAbstractStep Name(string name)
    {
        if (MainDsl.Verbose)
        {
            Console.WriteLine($"[{DateTime.Now}] Setting name to: {name}");
        }

        _name = name;
        return this;
    }

    public LogicAppAbstractStep ResourceGroup(string resourceGroup)
    {
        if (MainDsl.Verbose)
        {
            Console.WriteLine($"[{DateTime.Now}] Setting resource group to: {resourceGroup}");
        }

        _resourceGroup = resourceGroup;
        return this;
    }

    public LogicAppAbstractStep DisableListening()
    {
        _prepared = true;
        return this;
    }

    public LogicAppAbstractStep SubscriptionId(string subscriptionId)
    {
        if (MainDsl.Verbose)
        {
            Console.WriteLine($"[{DateTime.Now}] Setting subscription ID to: {subscriptionId}");
        }

        _subscriptionId = subscriptionId;
        return this;
    }

    public LogicAppAbstractStep Trigger()
    {
        throw new NotImplementedException();
    }

    public LogicAppAbstractStep Enable()
    {
        MainDsl.AddAction(async () =>
        {
            if (MainDsl.Verbose)
            {
                Console.WriteLine("Enabling Logic App");
            }

            var subscription =
                _armClient.GetSubscriptionResource(new ResourceIdentifier($"/subscriptions/{_subscriptionId}"));
            ResourceGroupCollection resourceGroups = subscription.GetResourceGroups();
            ResourceGroupResource resourceGroup = await resourceGroups.GetAsync(_resourceGroup);
            LogicWorkflowResource workflow = await resourceGroup.GetLogicWorkflowAsync(_name);
            await workflow.EnableAsync();

            if (MainDsl.Verbose)
            {
                Console.WriteLine("Logic App enabled");
            }
        });
        return this;
    }

    public LogicAppAbstractStep Disable()
    {
        MainDsl.AddAction(async () =>
        {
            if (MainDsl.Verbose)
            {
                Console.WriteLine("Disabling Logic App");
            }

            var subscription =
                _armClient.GetSubscriptionResource(new ResourceIdentifier($"/subscriptions/{_subscriptionId}"));
            ResourceGroupCollection resourceGroups = subscription.GetResourceGroups();
            ResourceGroupResource resourceGroup = await resourceGroups.GetAsync(_resourceGroup);
            LogicWorkflowResource workflow = await resourceGroup.GetLogicWorkflowAsync(_name);
            await workflow.DisableAsync();

            if (MainDsl.Verbose)
            {
                Console.WriteLine("Logic App disabled");
            }
        });
        return this;
    }

    public LogicAppAbstractStep IsEnabled()
    {
        MainDsl.AddAction(async () =>
        {
            if (MainDsl.Verbose)
            {
                Console.WriteLine("Checking whether the Logic App is enabled");
            }

            var subscription =
                _armClient.GetSubscriptionResource(new ResourceIdentifier($"/subscriptions/{_subscriptionId}"));
            ResourceGroupCollection resourceGroups = subscription.GetResourceGroups();
            ResourceGroupResource resourceGroup = await resourceGroups.GetAsync(_resourceGroup);
            LogicWorkflowResource workflow = await resourceGroup.GetLogicWorkflowAsync(_name);

            if (workflow.Data.State != LogicWorkflowState.Enabled)
            {
                throw new Exception("Logic App is not enabled");
            }

            if (MainDsl.Verbose)
            {
                Console.WriteLine("Logic App is enabled");
            }
        });
        return this;
    }

    public LogicAppAbstractStep IsDisabled()
    {
        MainDsl.AddAction(async () =>
        {
            if (MainDsl.Verbose)
            {
                Console.WriteLine("Checking whether the Logic App is disabled");
            }

            var subscription =
                _armClient.GetSubscriptionResource(new ResourceIdentifier($"/subscriptions/{_subscriptionId}"));
            ResourceGroupCollection resourceGroups = subscription.GetResourceGroups();
            ResourceGroupResource resourceGroup = await resourceGroups.GetAsync(_resourceGroup);
            LogicWorkflowResource workflow = await resourceGroup.GetLogicWorkflowAsync(_name);

            if (workflow.Data.State != LogicWorkflowState.Disabled)
            {
                throw new Exception("Logic App is not disabled");
            }

            if (MainDsl.Verbose)
            {
                Console.WriteLine("Logic App is disabled");
            }
        });
        return this;
    }

    public async Task ListenAsync()
    {
        var subscription =
            _armClient.GetSubscriptionResource(new ResourceIdentifier($"/subscriptions/{_subscriptionId}"));
        ResourceGroupCollection resourceGroups = subscription.GetResourceGroups();
        ResourceGroupResource resourceGroup = await resourceGroups.GetAsync(_resourceGroup);
        LogicWorkflowResource workflow = await resourceGroup.GetLogicWorkflowAsync(_name);

        // Get the workflow runs
        var workflowRuns = workflow.GetLogicWorkflowRuns();

        // Count the runs
        var runCount = 0;
        await foreach (var run in workflowRuns)
        {
            runCount++;
        }

        _runCount = runCount;
    }

    public LogicAppAbstractStep WaitForRun(int runs = 1, int millisecondsDelay = 5000, int retries = 5)
    {
        // TODO: Implement waiting for multiple runs
        MainDsl.AddAction(async () =>
        {
            if (MainDsl.Verbose)
            {
                Console.WriteLine("Counting Logic App runs");
            }

            var subscription =
                _armClient.GetSubscriptionResource(new ResourceIdentifier($"/subscriptions/{_subscriptionId}"));
            ResourceGroupCollection resourceGroups = subscription.GetResourceGroups();
            ResourceGroupResource resourceGroup = await resourceGroups.GetAsync(_resourceGroup);
            LogicWorkflowResource workflow = await resourceGroup.GetLogicWorkflowAsync(_name);

            var iteration = 0;
            while (iteration < retries)
            {
                // Get the workflow runs
                var workflowRuns = workflow.GetLogicWorkflowRuns();

                // Count the runs
                var runCount = 0;
                await foreach (var run in workflowRuns)
                {
                    runCount++;
                }

                if (runCount > _runCount)
                {
                    _runCount = runCount;
                    return;
                }

                iteration++;
                await Task.Delay(millisecondsDelay);
            }

            throw new Exception("Run was not found");
        });
        return this;
    }

    public LogicAppAbstractStep Custom(Action action)
    {
        throw new NotImplementedException();
    }

    public LogicAppAbstractStep Custom<T>(Func<T> action)
    {
        throw new NotImplementedException();
    }

    public LogicAppAbstractStep Custom(Action<LogicAppAbstractStep> action)
    {
        throw new NotImplementedException();
    }
}