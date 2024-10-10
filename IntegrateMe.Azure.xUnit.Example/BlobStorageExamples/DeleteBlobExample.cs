using IntegrateMe.Azure.BlobStorage;

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
}