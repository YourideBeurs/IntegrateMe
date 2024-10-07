using System.Linq.Expressions;

namespace IntegrateMe.Core;

public class Dsl : IStep
{
    public Dictionary<string, IStep> Entities { get; set; } = new();
    public List<Expression> Expressions { get; } = [];
    public bool Verbose { get; private set; }
    public Dsl MainDsl => this;

    public IStep When()
    {
        return this;
    }

    public IStep Then()
    {
        return this;
    }

    public IStep VerboseOutput(bool value = true)
    {
        Verbose = value;
        return this;
    }

    public async Task RunAsync()
    {
        await Task.CompletedTask;
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

    public void AddExpression(Expression expression)
    {
        Expressions.Add(expression);
    }
}