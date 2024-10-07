using IntegrateMe.Core;

namespace IntegrateMe.Azure.BlobStorage;

public class BlobStorageStep : IStep
{
    private string? _connectionString;
    private string? _containerName;
    private string? _sasToken;
    private readonly IStep _parent;
    public Dsl MainDsl => _parent.MainDsl;

    public BlobStorageStep(IStep parent)
    {
        _parent = parent;
    }

    public BlobStorageStep ConnectionString(string connectionString)
    {
        _connectionString = connectionString;
        return this;
    }

    public BlobStorageStep SasToken(string sasToken)
    {
        _sasToken = sasToken;
        return this;
    }

    public BlobStorageStep ContainerName(string containerName)
    {
        _containerName = containerName;
        return this;
    }

    public BlobStorageStep UploadBlob(string blobName, byte[] data)
    {
        return this;
    }

    public BlobStorageStep BlobExists(string blobName)
    {
        return this;
    }

    public BlobStorageStep Custom(Action action)
    {
        return this;
    }

    public BlobStorageStep Custom<T>(Func<T> action)
    {
        return this;
    }

    public BlobStorageStep Custom(Action<BlobStorageStep> action)
    {
        return this;
    }

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
        await Task.CompletedTask;
    }

    public async Task SetupAsync()
    {
        await _parent.SetupAsync();
    }

    public async Task TearDownAsync()
    {
        await _parent.TearDownAsync();
    }

    public T Get<T>(string key)
    {
        return MainDsl.Get<T>(key);
    }
}