# xUnit.Logger

The xUnit.Logger contains a logger that can be used in xUnit tests. The logger is based on the xUnit `ITestOutputHelper`
interface.

## Usage

The logger can be used in xUnit tests by using the `UseLogger` extension method. The `UseLogger` extension method is
used to set the `ITestOutputHelper` instance that the logger will use to write log messages.

```csharp
[Fact]
public async Task Test1()
{
    await Given()
        .UseLogger(testOutputHelper)
        .When()
        .Then()
        .RunAsync();
}
```

## Project structure

The xUnit.Logger project contains the following files:

* `XUnitLoggerExtensions.cs`: Contains the `UseLogger` extension method. The `UseLogger` extension method is used to set
  the
  `ITestOutputHelper` instance that the logger will use to write log messages.
* `XUnitLogger.cs`: Contains the `XUnitLogger` class that implements the `ILogger` interface. The `XUnitLogger` class
  writes log messages to the `ITestOutputHelper` instance that is set using the `UseLogger` extension method.
