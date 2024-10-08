using IntegrateMe.Core;
using Microsoft.EntityFrameworkCore;

namespace IntegrateMe.EntityFramework.Core;

public class EntityFrameworkCoreAbstractStep(AbstractStep parent) : AbstractStep(parent)
{
    private DbContext? _dbContext;

    public EntityFrameworkCoreAbstractStep DbContext(DbContext dbContext)
    {
        _dbContext = dbContext;
        return this;
    }

    public EntityFrameworkCoreAbstractStep Custom(Func<DbContext, Task> action)
    {
        if (_dbContext == null)
        {
            throw new InvalidOperationException("DbContext is not set");
        }

        MainDsl.AddAction(async () => await action.Invoke(_dbContext));
        return this;
    }

    public EntityFrameworkCoreAbstractStep Custom<T>(Func<T, Task> action) where T : class
    {
        if (_dbContext == null)
        {
            throw new InvalidOperationException("DbContext is not set");
        }

        MainDsl.AddAction(async () => await action.Invoke(_dbContext as T ?? throw new InvalidOperationException()));
        return this;
    }
}