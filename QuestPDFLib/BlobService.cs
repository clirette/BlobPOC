using System.Text;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

namespace QuestPDFLib;

public class BlobService
{
    private readonly StorageAccountConfiguration _storageAccountConfiguration;

    public BlobService(IConfiguration configuration)
    {
        _storageAccountConfiguration = new StorageAccountConfiguration
        {
            ConnectionString = configuration.GetValue<string>("StorageAccount:ConnectionString"),
            Container = configuration.GetValue<string>("StorageAccount:Container")
        };
    }

    public async Task Upload(string fileName, string title)
    {
        var containerClient = new BlobServiceClient(_storageAccountConfiguration.ConnectionString)
            .GetBlobContainerClient(_storageAccountConfiguration.Container);

        var pdfBytes = DocumentWriter.WritePDF(title);
        using var stream = new MemoryStream(pdfBytes);

        await containerClient.UploadBlobAsync(fileName, stream);
    }
}

public class StorageAccountConfiguration
{
    public string ConnectionString { get; set; }
    public string Container { get; set; }
}