namespace IntegrateMe.Core;

public class Dsl(AbstractStep parent) : AbstractStep(parent)
{
    public Dictionary<string, AbstractStep> Entities { get; set; } = new();
    private readonly List<Func<Task>> _setups = [];
    private readonly List<Func<Task>> _actions = [];
    private readonly List<Func<Task>> _tearDowns = [];
    public bool Verbose { get; private set; }

    public Dsl VerboseOutput(bool value = true)
    {
        Verbose = value;

        return this;
    }

    public new async Task RunAsync()
    {
        foreach (var action in _actions)
        {
            await action();
        }
    }

    public new async Task SetupAsync()
    {
        await Task.CompletedTask;
    }

    public new async Task TearDownAsync()
    {
        await Task.CompletedTask;
    }

    public new T Get<T>(string key) where T : AbstractStep
    {
        if (Entities.TryGetValue(key, out var value))
        {
            return (T)value;
        }

        throw new Exception("Key not found");
    }

    public async Task ValidateAsync()
    {
        await Task.CompletedTask;
    }

    public void AddSetup(Func<Task> action)
    {
        _setups.Add(action);
    }

    public void AddAction(Func<Task> action)
    {
        _actions.Add(action);
    }

    public void AddTearDown(Func<Task> action)
    {
        _tearDowns.Add(action);
    }
}