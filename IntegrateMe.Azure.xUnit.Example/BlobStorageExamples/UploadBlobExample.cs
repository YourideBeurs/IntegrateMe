using IntegrateMe.Azure.BlobStorage;

namespace IntegrateMe.Azure.xUnit.Example.BlobStorageExamples;

public class UploadBlobExample
{
    /// <summary>
    /// This test validates the ability to upload a file to Azure Blob Storage using a connection string.
    /// It initializes the Blob Storage with a connection string for local development storage,
    /// uploads a text file named "test.txt" with the content "Hello World",
    /// and verifies that the file exists in the storage after the upload operation.
    /// </summary>
    [Fact]
    public async Task UploadBlobWithConnectionString()
    {
        await Given()
            .BlobStorage("Storage")
            .ConnectionString("UseDevelopmentStorage=true")
            .When()
            .BlobStorage("Storage")
            .UploadBlob("test.txt", "Hello World")
            .Then()
            .BlobStorage("Storage")
            .BlobExists("test.txt")
            .RunAsync();
    }

    /// <summary>
    /// This test validates the ability to upload a file to Azure Blob Storage using a SAS token.
    /// It initializes the Blob Storage with a SAS token, uploads a text file named "test.txt" 
    /// with the content "Hello World", and verifies that the file exists in the storage after 
    /// the upload operation.
    /// </summary>
    [Fact]
    public async Task UploadBlobWithSASToken()
    {
        await Given()
            .BlobStorage("Storage")
            .SasToken("ThisIsMySasToken")
            .When()
            .BlobStorage("Storage")
            .UploadBlob("test.txt", "Hello World")
            .Then()
            .BlobStorage("Storage")
            .BlobExists("test.txt")
            .RunAsync();
    }
}