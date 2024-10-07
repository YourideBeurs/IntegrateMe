namespace IntegrateMe.Core;

public interface IStep
{
    public Dsl MainDsl { get; }
    IStep When();
    IStep Then();
    Task RunAsync();
    Task SetupAsync();
    Task TearDownAsync();
    T Get<T>(string key);
}