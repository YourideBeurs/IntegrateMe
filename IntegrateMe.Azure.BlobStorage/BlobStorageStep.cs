using Azure.Storage.Blobs;
using IntegrateMe.Core;

namespace IntegrateMe.Azure.BlobStorage;

public class BlobStorageStep(AbstractStep parent) : AbstractStep(parent)
{
    private string? _connectionString;
    private string? _containerName;
    private string? _sasToken;

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

    public BlobStorageStep UploadBlob(string blobName, string data)
    {
        MainDsl.AddAction(async () =>
        {
            var container = new BlobContainerClient(_connectionString, _containerName);
            var blobClient = container.GetBlobClient(blobName);
            await blobClient.UploadAsync(data);
        });
        return this;
    }

    public BlobStorageStep UploadBlob(string blobName, Stream data)
    {
        MainDsl.AddAction(async () =>
        {
            var container = new BlobContainerClient(_connectionString, _containerName);
            var blobClient = container.GetBlobClient(blobName);
            await blobClient.UploadAsync(data);
        });
        return this;
    }

    public BlobStorageStep BlobExists(string blobName, bool exists = true)
    {
        MainDsl.AddAction(async () =>
        {
            var container = new BlobContainerClient(_connectionString, _containerName);
            var blobClient = container.GetBlobClient(blobName);
            var response = await blobClient.ExistsAsync();
            if (response.Value != exists)
            {
                throw new Exception($"Blob {blobName} exists: {response.Value} but expected {exists}");
            }
        });
        return this;
    }

    public BlobStorageStep DeleteBlob(string blobName)
    {
        MainDsl.AddAction(async () =>
        {
            var container = new BlobContainerClient(_connectionString, _containerName);
            var blobClient = container.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
        });
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
}