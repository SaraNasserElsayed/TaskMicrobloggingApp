using Core.Interfaces;
using FluentAssertions;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Tests.Services;

public class PostServiceTests
{
    [Fact]
    public void CreatePost_WithValidData_ShouldSavePostToDatabase()
    {
        // Arrange
        var context = TestHelpers.CreateInMemoryDbContext();
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
