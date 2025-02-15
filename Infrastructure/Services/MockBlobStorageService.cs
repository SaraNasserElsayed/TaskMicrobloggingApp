using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Core.Interfaces;

namespace Infrastructure.Services;
public class MockBlobStorageService : IBlobStorageService
{
    public Task<string> UploadImageAsync(IFormFile imageFile)
    {
        // Return a fake URL for testing
        return Task.FromResult("https://mockstorage.com/fake-image-url.png");
    }
}
