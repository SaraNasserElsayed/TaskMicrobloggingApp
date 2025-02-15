using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Core.Interfaces;

namespace Infrastructure.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly string _connectionString;
    private readonly string _containerName;

    public BlobStorageService(IConfiguration configuration)
    {
        _connectionString = configuration["AzureBlobStorage:ConnectionString"];
        _containerName = configuration["AzureBlobStorage:ContainerName"];
    }

    public async Task<string> UploadImageAsync(IFormFile imageFile)
    {
        var blobServiceClient = new BlobServiceClient(_connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        var blobName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
        var blobClient = containerClient.GetBlobClient(blobName);

        await using var stream = imageFile.OpenReadStream();
        await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = imageFile.ContentType });

        return blobClient.Uri.ToString();
    }
}
