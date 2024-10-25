using IntegrateMe.Core;
using Xunit.Abstractions;

namespace IntegrateMe.xUnit.Logger;

public static class XUnitLoggerExtensions
{
    public static AbstractStep UseLogger(this AbstractStep step, ITestOutputHelper testOutputHelper)
    {
        step.MainDsl.LogHandler = new XUnitLogHandler(testOutputHelper);
        return step;
    }
}