using IntegrateMe.Core.Logging;
using Xunit.Abstractions;

namespace IntegrateMe.xUnit.Logger;

public class XUnitLogHandler(ITestOutputHelper testOutputHelper) : ILogHandler
{
    public void WriteLine(string message)
    {
        testOutputHelper.WriteLine(message);
    }
}