namespace IntegrateMe.Core;

public abstract class AbstractStep(AbstractStep parent)
{
    public Dsl MainDsl => parent.MainDsl;

    public AbstractStep When()
    {
        return this;
    }

    public AbstractStep Then()
    {
        return this;
    }

    public async Task RunAsync()
    {
        await MainDsl.RunAsync();
    }

    public async Task SetupAsync()
    {
        await MainDsl.SetupAsync();
    }

    public async Task TearDownAsync()
    {
        await MainDsl.TearDownAsync();
    }

    public T Get<T>(string key) where T : AbstractStep
    {
        return MainDsl.Get<T>(key);
    }
}