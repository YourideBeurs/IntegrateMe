﻿using IntegrateMe.Azure.BlobStorage;
using IntegrateMe.Core.Retry;

namespace IntegrateMe.Azure.xUnit.Example.BlobStorageExamples;

public class DeleteBlobExample
{
    [Fact]
    public async Task DeletelobWithConnectionString()
    {
        await Given()
            .BlobStorage("Storage")
            .ConnectionString("UseDevelopmentStorage=true")
            .When()
            .BlobStorage("Storage")
            .DeleteBlob("test.txt")
            .Then()
            .BlobStorage("Storage")
            .BlobExists("test.txt", false)
            .RunAsync();
    }

    [Fact]
    public async Task DeleteBlobWithSASToken()
    {
        await Given()
            .BlobStorage("Storage")
            .SasToken("ThisIsMySasToken")
            .When()
            .BlobStorage("Storage")
            .DeleteBlob("test.txt")
            .Then()
            .BlobStorage("Storage")
            .BlobExists("test.txt", false)
            .RunAsync();
    }

    [Fact]
    public async Task DeletelobWithConnectionStringWithRetry()
    {
        await Given()
            .BlobStorage("Storage")
            .ConnectionString("UseDevelopmentStorage=true")
            .When()
            .BlobStorage("Storage")
            .DeleteBlob(
                blobName: "test.txt",
                retryHandler: new RetryHandler
                {
                    MaxRetries = 3,
                    Delay = TimeSpan.FromSeconds(1),
                })
            .Then()
            .BlobStorage("Storage")
            .BlobExists("test.txt", false)
            .RunAsync();
    }

    [Fact]
    public async Task DeleteBlobWithSASTokenWithRetry()
    {
        await Given()
            .BlobStorage("Storage")
            .SasToken("ThisIsMySasToken")
            .When()
            .BlobStorage("Storage")
            .DeleteBlob(
                blobName: "test.txt",
                retryHandler: new RetryHandler
                {
                    MaxRetries = 3,
                    Delay = TimeSpan.FromSeconds(1),
                })
            .Then()
            .BlobStorage("Storage")
            .BlobExists("test.txt", false)
            .RunAsync();
    }
}