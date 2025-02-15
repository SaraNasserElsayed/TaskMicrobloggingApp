using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace Tests;

public class PostsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
	private readonly HttpClient _client;

	public PostsControllerTests(WebApplicationFactory<Program> factory)
	{
		_client = factory.CreateClient();
	}

	[Fact]
	public async Task GetAllPosts_WithValidToken_ReturnsOkResponse()
	{
		// Arrange
		var token = await GetJwtTokenAsync("admin", "password");
		_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

		// Act
		var response = await _client.GetAsync("/api/posts");

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
	}

	[Fact]
	public async Task CreatePost_ValidRequest_ReturnsCreatedPost()
	{
		// Arrange
		var token = await GetJwtTokenAsync("admin", "password");
		_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

		var formData = new MultipartFormDataContent();
		formData.Add(new StringContent("Integration test post"), "Text");

		var imageData = Encoding.UTF8.GetBytes("Fake image content");
		var imageContent = new ByteArrayContent(imageData);
		imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/png");
		formData.Add(imageContent, "ImageFile", "test-image.png");

		// Act
		var response = await _client.PostAsync("/api/posts", formData);
		var responseString = await response.Content.ReadAsStringAsync();

		// Assert
		response.EnsureSuccessStatusCode();
		Assert.Contains("Integration test post", responseString);
	}

	[Fact]
	public async Task CreatePost_TextExceeds140Characters_ReturnsBadRequest()
	{
		// Arrange
		var token = await GetJwtTokenAsync("admin", "password");
		_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

		var longText = new string('A', 141);
		var formData = new MultipartFormDataContent();
		formData.Add(new StringContent(longText), "Text");

		// Act
		var response = await _client.PostAsync("/api/posts", formData);

		// Assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
	}

	private async Task<string> GetJwtTokenAsync(string username, string password)
	{
		var loginRequest = new
		{
			Username = username,
			Password = password
		};
		var content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");

		var response = await _client.PostAsync("/api/auth/login", content);
		response.EnsureSuccessStatusCode();

		var responseString = await response.Content.ReadAsStringAsync();
		var responseObject = JsonConvert.DeserializeObject<dynamic>(responseString);

		return responseObject.token;
	}
}
