using IntegrateMe.EntityFramework.Core;
using Microsoft.EntityFrameworkCore;

namespace IntegrateMe.EntityFrameworkCore.xUnit.Example.EntityFrameworkCoreExamples;

public class CustomEntityFrameworkCoreExample
{
    [Fact]
    public async Task Test1()
    {
        DbContext context = new DbContext(new DbContextOptionsBuilder().Options);
        await Given()
            .EntityFramework("Entity Framework")
            .DbContext(context)
            .When()
            .EntityFramework("Entity Framework")
            .Custom(async x => { await x.DisposeAsync(); })
            .Custom(x => x.Dispose())
            .Then()
            .RunAsync();
    }
}