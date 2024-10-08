using IntegrateMe.Core;
using Microsoft.EntityFrameworkCore;

namespace IntegrateMe.EntityFramework.Core;

public class EntityFrameworkCoreAbstractStep : AbstractStep
{
    private readonly AbstractStep _parent;

    public EntityFrameworkCoreAbstractStep(AbstractStep parent) : base(parent)
    {
        _parent = parent;
    }

    private DbContext _dbContext;

    public EntityFrameworkCoreAbstractStep DbContext(DbContext dbContext)
    {
        _dbContext = dbContext;
        return this;
    }


    public EntityFrameworkCoreAbstractStep Custom(Func<DbContext, Task> action)
    {
        MainDsl.AddAction(async () => await action.Invoke(_dbContext));
        return this;
    }

    public EntityFrameworkCoreAbstractStep Custom<T>(Func<T, Task> action) where T : class
    {
        MainDsl.AddAction(async () => await action.Invoke(_dbContext as T ?? throw new InvalidOperationException()));
        return this;
    }
}