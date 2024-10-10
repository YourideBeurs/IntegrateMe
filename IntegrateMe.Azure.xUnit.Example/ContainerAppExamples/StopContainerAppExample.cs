using IntegrateMe.Azure.ContainerApp;

namespace IntegrateMe.Azure.xUnit.Example.ContainerAppExamples;

public class StopContainerAppExample
{
    [Fact]
    public async Task StopContainerApp()
    {
        await Given()
            .ContainerApp("Container App")
            .SubscriptionId("Subscription Id")
            .ResourceGroup("Resource Group")
            .Name("Name")
            .When()
            .ContainerApp("Container App")
            .Stop()
            .RunAsync();
    }
}