using Xunit.Abstractions;

namespace IntegrateMe.xUnit.Logger.Example.XUnitLoggerExamples;

public class UseLoggerExample(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public async Task Test1()
    {
        await Given()
            .UseLogger(testOutputHelper)
            .When()
            .Then()
            .RunAsync();
    }
}