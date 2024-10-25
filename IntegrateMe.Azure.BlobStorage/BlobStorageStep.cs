using Azure;
using Azure.Storage.Blobs;
using IntegrateMe.Core;
using IntegrateMe.Core.Retry;

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
        return UploadBlob(blobName, data, RetryHandler.DefaultRetryHandler());
    }

    public BlobStorageStep UploadBlob(string blobName, string data, RetryHandler retryHandler)
    {
        MainDsl.AddAction(async () =>
        {
            await retryHandler.RunAsync(async () =>
            {
                try
                {
                    var container = new BlobContainerClient(_connectionString, _containerName);
                    var blobClient = container.GetBlobClient(blobName);
                    await blobClient.UploadAsync(data);
                    retryHandler.Success();
                }
                catch (RequestFailedException)
                {
                }
            });

            if (!retryHandler.IsSuccess())
            {
                throw new Exception($"Failed to upload blob with name {blobName}");
            }
        });
        return this;
    }

    public BlobStorageStep UploadBlob(string blobName, Stream data)
    {
        return UploadBlob(blobName, data, RetryHandler.DefaultRetryHandler());
    }

    public BlobStorageStep UploadBlob(string blobName, Stream data, RetryHandler retryHandler)
    {
        MainDsl.AddAction(async () =>
        {
            await retryHandler.RunAsync(async () =>
            {
                try
                {
                    var container = new BlobContainerClient(_connectionString, _containerName);
                    var blobClient = container.GetBlobClient(blobName);
                    await blobClient.UploadAsync(data);
                    retryHandler.Success();
                }
                catch (RequestFailedException)
                {
                }
            });

            if (!retryHandler.IsSuccess())
            {
                throw new Exception($"Failed to upload blob with name {blobName}");
            }
        });
        return this;
    }

    public BlobStorageStep BlobExists(string blobName, bool exists = true)
    {
        return BlobExists(blobName, exists, RetryHandler.DefaultRetryHandler());
    }

    public BlobStorageStep BlobExists(string blobName, RetryHandler retryHandler)
    {
        return BlobExists(blobName, true, retryHandler);
    }

    public BlobStorageStep BlobExists(string blobName, bool exists, RetryHandler retryHandler)
    {
        MainDsl.AddAction(async () =>
        {
            var responseValue = !exists;

            var container = new BlobContainerClient(_connectionString, _containerName);
            var blobClient = container.GetBlobClient(blobName);

            await retryHandler.RunAsync(async () =>
            {
                var response = await blobClient.ExistsAsync();
                if (response.Value == exists)
                {
                    retryHandler.Success();
                }

                responseValue = response.Value;
            });

            if (!retryHandler.IsSuccess())
            {
                throw new Exception($"Blob {blobName} exists: {responseValue} but expected {exists}");
            }
        });

        return this;
    }

    public BlobStorageStep DeleteBlob(string blobName)
    {
        return DeleteBlob(blobName, RetryHandler.DefaultRetryHandler());
    }

    public BlobStorageStep DeleteBlob(string blobName, RetryHandler retryHandler)
    {
        MainDsl.AddAction(async () =>
        {
            var container = new BlobContainerClient(_connectionString, _containerName);
            var blobClient = container.GetBlobClient(blobName);
            await retryHandler.RunAsync(async () =>
            {
                try
                {
                    await blobClient.DeleteIfExistsAsync();
                    retryHandler.Success();
                }
                catch (RequestFailedException)
                {
                }

                retryHandler.Success();
            });
        });

        if (!retryHandler.IsSuccess())
        {
            throw new Exception($"Failed to delete blob with name {blobName}");
        }

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