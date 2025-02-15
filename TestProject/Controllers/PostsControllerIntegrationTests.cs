using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Tests.Controllers;

public class PostsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly Mock<IBlobStorageService> _mockBlobStorageService;

    public PostsControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _mockBlobStorageService = new Mock<IBlobStorageService>();
        _mockBlobStorageService.Setup(x => x.UploadImageAsync(It.IsAny<IFormFile>()))
                               .ReturnsAsync("https://fakeurl.com/image.webp");

        var customFactory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_mockBlobStorageService.Object);
            });
        });

        _client = customFactory.CreateClient();
    }

    [Fact]
    public async Task GetAllPosts_ShouldReturnEmptyListInitially()
    {
        // Act
        var response = await _client.GetAsync("/api/posts");
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        responseString.Should().Be("[]");
    }

    [Fact]
    public async Task CreatePost_ShouldAddPostSuccessfully()
    {
        // Arrange
        var createPostRequest = new
        {
            Text = "Integration Test Post"
        };
        var content = new StringContent(JsonConvert.SerializeObject(createPostRequest), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/posts", content);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        responseString.Should().Contain("Integration Test Post");
    }
}
