using IntegrateMe.Core;

namespace IntegrateMe.Azure.BlobStorage;

public class BlobStorageAbstractStep(AbstractStep parent) : AbstractStep(parent)
{
    private string? _connectionString;
    private string? _containerName;
    private string? _sasToken;

    public BlobStorageAbstractStep ConnectionString(string connectionString)
    {
        _connectionString = connectionString;
        return this;
    }

    public BlobStorageAbstractStep SasToken(string sasToken)
    {
        _sasToken = sasToken;
        return this;
    }

    public BlobStorageAbstractStep ContainerName(string containerName)
    {
        _containerName = containerName;
        return this;
    }

    public BlobStorageAbstractStep UploadBlob(string blobName, byte[] data)
    {
        return this;
    }

    public BlobStorageAbstractStep BlobExists(string blobName)
    {
        return this;
    }

    public BlobStorageAbstractStep Custom(Action action)
    {
        return this;
    }

    public BlobStorageAbstractStep Custom<T>(Func<T> action)
    {
        return this;
    }

    public BlobStorageAbstractStep Custom(Action<BlobStorageAbstractStep> action)
    {
        return this;
    }
}