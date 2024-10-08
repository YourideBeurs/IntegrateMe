using IntegrateMe.Core;
using Microsoft.EntityFrameworkCore;

namespace IntegrateMe.EntityFramework.Core;

public class EntityFrameworkCoreStep(AbstractStep parent) : AbstractStep(parent)
{
    private DbContext? _dbContext;

    public EntityFrameworkCoreStep DbContext(DbContext dbContext)
    {
        _dbContext = dbContext;
        return this;
    }

    public string? ConnectionString()
    {
        return _dbContext?.Database.GetConnectionString();
    }

    public EntityFrameworkCoreStep Custom(Func<DbContext, Task> action)
    {
        if (_dbContext == null)
        {
            throw new InvalidOperationException("DbContext is not set");
        }

        MainDsl.AddAction(async () => await action.Invoke(_dbContext));
        return this;
    }

    public EntityFrameworkCoreStep Custom<T>(Func<T, Task> action) where T : class
    {
        if (_dbContext == null)
        {
            throw new InvalidOperationException("DbContext is not set");
        }

        MainDsl.AddAction(async () => await action.Invoke(_dbContext as T ?? throw new InvalidOperationException()));
        return this;
    }
}