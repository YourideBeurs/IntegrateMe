using IntegrateMe.Core;
using Microsoft.EntityFrameworkCore;

namespace IntegrateMe.EntityFramework.Core;

public class EntityFrameworkCoreStep : IStep
{
    private readonly List<Func<Task>> _actions = [];

    public EntityFrameworkCoreStep(IStep parent)
    {
        _parent = parent;
    }

    private DbContext _dbContext;

    public EntityFrameworkCoreStep DbContext(DbContext dbContext)
    {
        _dbContext = dbContext;
        return this;
    }

    private readonly IStep _parent;
    public Dsl MainDsl => _parent.MainDsl;

    public IStep When()
    {
        return this;
    }

    public IStep Then()
    {
        return this;
    }

    public async Task RunAsync()
    {
        await _parent.SetupAsync();

        foreach (var action in _actions)
        {
            await action.Invoke();
        }

        await _parent.RunAsync();
        await _parent.TearDownAsync();
    }

    public async Task SetupAsync()
    {
        await Task.CompletedTask;
    }

    public async Task TearDownAsync()
    {
        await Task.CompletedTask;
    }

    public T Get<T>(string key)
    {
        return _parent.Get<T>(key);
    }

    public EntityFrameworkCoreStep Custom(Func<DbContext, Task> action)
    {
        _actions.Add(async () => await action.Invoke(_dbContext));

        return this;
    }

    public EntityFrameworkCoreStep Custom<T>(Func<T, Task> action) where T : class
    {
        _actions.Add(async () => await action.Invoke(_dbContext as T ?? throw new InvalidOperationException()));

        return this;
    }
}