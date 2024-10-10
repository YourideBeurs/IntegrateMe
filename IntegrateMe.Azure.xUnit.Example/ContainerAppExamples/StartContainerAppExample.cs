using IntegrateMe.Azure.ContainerApp;

namespace IntegrateMe.Azure.xUnit.Example.ContainerAppExamples;

public class StartContainerAppExample
{
    [Fact]
    public async Task StartContainerApp()
    {
        await Given()
            .ContainerApp("Container App")
            .SubscriptionId("Subscription Id")
            .ResourceGroup("Resource Group")
            .Name("Name")
            .When()
            .ContainerApp("Container App")
            .Start()
            .RunAsync();
    }
}