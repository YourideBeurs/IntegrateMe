using IntegrateMe.Azure.BlobStorage;
using IntegrateMe.Core.Retry;

namespace IntegrateMe.Azure.xUnit.Example.BlobStorageExamples;

public class BlobExistsExample
{
    [Fact]
    public async Task BlobExists()
    {
        await Given()
            .BlobStorage("Storage")
            .ConnectionString("UseDevelopmentStorage=true")
            .ContainerName("Container")
            .When()
            .BlobStorage("Storage")
            .UploadBlob("test.txt", "Hello, World!")
            .Then()
            .BlobStorage("Storage")
            .BlobExists("test.txt")
            .RunAsync();
    }

    [Fact]
    public async Task BlobDoesNotExist()
    {
        await Given()
            .BlobStorage("Storage")
            .ConnectionString("UseDevelopmentStorage=true")
            .ContainerName("Container")
            .When()
            .Then()
            .BlobStorage("Storage")
            .BlobExists("test.txt", false)
            .RunAsync();
    }

    [Fact]
    public async Task BlobExistsWithRetry()
    {
        await Given()
            .BlobStorage("Storage")
            .ConnectionString("UseDevelopmentStorage=true")
            .ContainerName("Container")
            .When()
            .BlobStorage("Storage")
            .UploadBlob("test.txt", "Hello, World!")
            .Then()
            .BlobStorage("Storage")
            .BlobExists(
                blobName: "test.txt",
                retryHandler: new RetryHandler
                {
                    Delay = TimeSpan.FromSeconds(5),
                    MaxRetries = 3,
                })
            .RunAsync();
    }

    [Fact]
    public async Task BlobDoesNotExistWithRetry()
    {
        await Given()
            .BlobStorage("Storage")
            .ConnectionString("UseDevelopmentStorage=true")
            .ContainerName("Container")
            .When()
            .Then()
            .BlobStorage("Storage")
            .BlobExists(
                blobName: "test.txt",
                exists: false,
                retryHandler: new RetryHandler
                {
                    Delay = TimeSpan.FromSeconds(5),
                    MaxRetries = 3,
                })
            .RunAsync();
    }
}