using System;
using System.Linq;
using Core.Interfaces;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Tests;

public class PostServiceTests
{
    private AppDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Unique database for each test
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public void CreatePost_WithValidData_ShouldSavePostToDatabase()
    {
        // Arrange
        var context = CreateInMemoryDbContext();
        var mockBlobStorageService = new Mock<IBlobStorageService>();
        mockBlobStorageService.Setup(x => x.UploadImageAsync(It.IsAny<IFormFile>()))
                              .ReturnsAsync("https://fakeurl.com/image.webp");

        var postService = new PostService(context, mockBlobStorageService.Object);

        // Act
        var result = postService.CreatePost("Hello World", null, "testuser");

        // Assert
        result.Should().NotBeNull();
        result.Text.Should().Be("Hello World");
        result.Username.Should().Be("testuser");
        context.Posts.Count().Should().Be(1);
    }
}
