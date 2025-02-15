using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Infrastructure.Services;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Tests;

public class BlobStorageServiceTests
{
    [Fact]
    public async Task UploadImageAsync_ValidImage_ReturnsBlobUrl()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(config => config["AzureBlobStorage:ConnectionString"]).Returns("UseDevelopmentStorage=true");
        mockConfiguration.Setup(config => config["AzureBlobStorage:ContainerName"]).Returns("test-container");

        var blobStorageService = new BlobStorageService(mockConfiguration.Object);

        var imageFileMock = new Mock<IFormFile>();
        var content = "Fake image content";
        var fileName = "test-image.png";

        using var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(content);
        writer.Flush();
        ms.Position = 0;

        imageFileMock.Setup(f => f.OpenReadStream()).Returns(ms);
        imageFileMock.Setup(f => f.FileName).Returns(fileName);
        imageFileMock.Setup(f => f.ContentType).Returns("image/png");

        // Act
        var result = await blobStorageService.UploadImageAsync(imageFileMock.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("http", result);  // Check that the result is a URL
    }

    [Fact]
    public async Task UploadImageAsync_NullImage_ThrowsArgumentNullException()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(config => config["AzureBlobStorage:ConnectionString"]).Returns("UseDevelopmentStorage=true");
        mockConfiguration.Setup(config => config["AzureBlobStorage:ContainerName"]).Returns("test-container");

        var blobStorageService = new BlobStorageService(mockConfiguration.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => blobStorageService.UploadImageAsync(null));
    }
}
